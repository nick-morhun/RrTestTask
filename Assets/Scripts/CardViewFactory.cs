using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RrTestTask
{
    public class CardViewFactory
    {
        private readonly CardView cardViewPrefab;
        private readonly IReadOnlyList<Sprite> icons;

        public CardViewFactory(CardView cardViewPrefab, IReadOnlyList<Sprite> icons)
        {
            this.cardViewPrefab = cardViewPrefab;
            this.icons = icons;
        }

        public CardView Create([NotNull] Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));

            CardView view = Object.Instantiate(cardViewPrefab);
            view.SetModel(card, icons);
            return view;
        }
    }
}