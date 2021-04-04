using System;
using JetBrains.Annotations;
using UnityEngine;

namespace RrTestTask
{
    public class CardSpriteFactory
    {
        private readonly uint cardIconHeight;
        private readonly uint cardIconWidth;
        private readonly uint pixelsPerUnit;

        public CardSpriteFactory(uint cardIconHeight, uint cardIconWidth, uint pixelsPerUnit)
        {
            this.cardIconHeight = cardIconHeight;
            this.cardIconWidth = cardIconWidth;
            this.pixelsPerUnit = pixelsPerUnit;
        }

        public Sprite Create([NotNull] Texture2D texture2D)
        {
            if (texture2D == null) throw new ArgumentNullException(nameof(texture2D));

            var rect = new Rect(Vector2.zero, new Vector2(cardIconWidth, cardIconHeight));
            return Sprite.Create(texture2D, rect, Vector2.zero, pixelsPerUnit);
        }
    }
}