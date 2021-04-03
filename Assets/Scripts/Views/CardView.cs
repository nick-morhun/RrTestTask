using System;
using System.Collections.Generic;
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

        public void SetModel([CanBeNull] Card card, [NotNull] IReadOnlyList<Sprite> icons)
        {
            if (icons == null) throw new ArgumentNullException(nameof(icons));

            Sprite icon = card != null ? icons[card.Icon] : null;
            iconView.SetModel(icon);
            attackView.SetModel(card?.Attack);
            healthView.SetModel(card?.Health);
            manaView.SetModel(card?.Mana);
        }

        private void Awake()
        {
            Assert.IsNotNull(iconView);
            Assert.IsNotNull(attackView);
            Assert.IsNotNull(healthView);
            Assert.IsNotNull(manaView);
        }
    }
}