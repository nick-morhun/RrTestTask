using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace RrTestTask
{
    public sealed class CardView : MonoBehaviour
    {
        [SerializeField] private CardIconView iconView;
        [SerializeField] private StatView attackView;
        [SerializeField] private StatView healthView;
        [SerializeField] private StatView manaView;
        [SerializeField] private CardDragView dragView;
        [SerializeField] private CardDragInput dragInput;
        [SerializeField] private float repositionDurationSec;
        private Sequence positionSequence;
        private Card model;

        public CardDragInput DragInput => dragInput;

        public void SetModel([CanBeNull] Card card, [NotNull] IReadOnlyList<Sprite> icons)
        {
            if (icons == null) throw new ArgumentNullException(nameof(icons));

            Sprite icon = card != null ? icons[card.Icon] : null;
            iconView.SetModel(icon);
            attackView.SetModel(card?.Attack);
            healthView.SetModel(card?.Health);
            manaView.SetModel(card?.Mana);
            dragView.SetModel(card);
            dragInput.SetModel(card);
            model = card;
        }

        public void ClearModel()
        {
            iconView.SetModel(null);
            attackView.SetModel(null);
            healthView.SetModel(null);
            manaView.SetModel(null);
            dragView.SetModel(null);
            dragInput.SetModel(null);
        }

        public void MoveTo(Vector3 position, Quaternion rotation)
        {
            Transform cachedTransform = transform;
            positionSequence.Kill();

            model.IsMoving.Value = true;
            positionSequence = DOTween.Sequence();
            Tween positionTween = DOTween.To(() => cachedTransform.localPosition, current => cachedTransform.localPosition = current, position, repositionDurationSec);
            Tween rotationTween = DOTween.To(() => cachedTransform.localRotation, current => cachedTransform.localRotation = current, rotation.eulerAngles, repositionDurationSec);
            positionSequence.Join(positionTween).Join(rotationTween)
                .OnKill(() => model.IsMoving.Value = false);
        }

        private void Awake()
        {
            Assert.IsNotNull(iconView);
            Assert.IsNotNull(attackView);
            Assert.IsNotNull(healthView);
            Assert.IsNotNull(manaView);
            Assert.IsNotNull(dragView);
        }

        private void OnDestroy()
        {
            positionSequence.Kill();
        }
    }
}