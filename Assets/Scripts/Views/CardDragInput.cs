using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RrTestTask
{
    public sealed class CardDragInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Vector3 rotationWhenDragged;
        [SerializeField] private float repositionDurationSec;
        private Sequence sequence;
        private Card model;
        private Vector3? startPosition;
        private Quaternion? startRotation;
        private Transform startParent;
        private Transform cachedTransform;
        private Transform dropTarget;

        public void SetModel([CanBeNull] Card card)
        {
            model = card;
        }

        public void SetDropTarget(Transform target)
        {
            dropTarget = target;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!model.CanDrag)
            {
                return;
            }

            startPosition = startPosition ?? cachedTransform.localPosition;
            startRotation = startRotation ?? cachedTransform.localRotation;
            startParent = startParent ? startParent : cachedTransform.parent;
            model.IsDragged.Value = true;
            RotateTo(Quaternion.Euler(rotationWhenDragged));
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!model.IsDragged.Value)
            {
                return;
            }

            transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!model.IsDragged.Value)
            {
                return;
            }

            if (dropTarget)
            {
                transform.SetParent(dropTarget);
                MoveTo(Vector3.zero, Quaternion.identity, true);
                return;
            }

            if (startPosition != null && startRotation != null)
            {
                transform.SetParent(startParent);
                MoveTo(startPosition.Value, startRotation.Value, false);
            }
        }

        private void Start()
        {
            cachedTransform = transform;
        }

        private void OnDestroy()
        {
            sequence.Kill();
        }

        private void MoveTo(Vector3 position, Quaternion rotation, bool drop)
        {
            sequence.Kill();

            model.IsDragged.Value = true;
            sequence = DOTween.Sequence();
            Tween positionTween = DOTween.To(() => cachedTransform.localPosition, current => cachedTransform.localPosition = current, position, repositionDurationSec);
            Tween rotationTween = DOTween.To(() => cachedTransform.localRotation, current => cachedTransform.localRotation = current, rotation.eulerAngles, repositionDurationSec);
            sequence.Join(positionTween).Join(rotationTween)
                .OnComplete(() =>
                {
                    if (drop)
                    {
                        model.IsDropped.Value = true;
                    }

                    model.IsDragged.Value = false;
                });
        }

        private void RotateTo(Quaternion rotation)
        {
            sequence.Kill();
            sequence = DOTween.Sequence();

            Tween rotationTween = DOTween.To(() => cachedTransform.localRotation, current => cachedTransform.localRotation = current, rotation.eulerAngles, repositionDurationSec);
            sequence.Join(rotationTween);
        }
    }
}