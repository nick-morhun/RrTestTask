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
        [SerializeField] private float repositionDurationSec;
        private Sequence positionSequence;

        public void SetModel([CanBeNull] Card card, [NotNull] IReadOnlyList<Sprite> icons)
        {
            if (icons == null) throw new ArgumentNullException(nameof(icons));

            Sprite icon = card != null ? icons[card.Icon] : null;
            iconView.SetModel(icon);
            attackView.SetModel(card?.Attack);
            healthView.SetModel(card?.Health);
            manaView.SetModel(card?.Mana);
        }

        public void ClearModel()
        {
            iconView.SetModel(null);
            attackView.SetModel(null);
            healthView.SetModel(null);
            manaView.SetModel(null);
        }

        public void MoveTo(Vector3 position, Quaternion rotation)
        {
            Transform cachedTransform = transform;
            positionSequence.Kill();
            positionSequence = DOTween.Sequence();
            Tween positionTween = DOTween.To(() => cachedTransform.localPosition, current => cachedTransform.localPosition = current, position, repositionDurationSec);
            Tween rotationTween = DOTween.To(() => cachedTransform.localRotation, current => cachedTransform.localRotation = current, rotation.eulerAngles, repositionDurationSec);
            positionSequence.Join(positionTween).Join(rotationTween);
        }

        private void Awake()
        {
            Assert.IsNotNull(iconView);
            Assert.IsNotNull(attackView);
            Assert.IsNotNull(healthView);
            Assert.IsNotNull(manaView);
        }

        private void OnDestroy()
        {
            positionSequence.Kill();
        }
    }
}