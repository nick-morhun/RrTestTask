using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace RrTestTask
{
    public class CardGenerator
    {
        private readonly ICardSettings cardSettings;

        public CardGenerator([NotNull] ICardSettings cardSettings)
        {
            this.cardSettings = cardSettings ?? throw new ArgumentNullException(nameof(cardSettings));
        }

        public IEnumerable<Card> Create(int iconCount)
        {
            for (var iconIndex = 0; iconIndex < iconCount; iconIndex++)
            {
                var playerCard = new Card(iconIndex);
                playerCard.Attack.Value = Random.Range(cardSettings.MinStatValue, cardSettings.MaxStatValue + 1);
                playerCard.Health.Value = Random.Range(cardSettings.MinHealthAliveValue, cardSettings.MaxStatValue + 1);
                playerCard.Mana.Value = Random.Range(cardSettings.MinStatValue, cardSettings.MaxStatValue + 1);
                yield return playerCard;
            }
        }
    }
}