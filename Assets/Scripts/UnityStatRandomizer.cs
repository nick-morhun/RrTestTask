using System;
using JetBrains.Annotations;
using UniRx;
using Random = UnityEngine.Random;

namespace RrTestTask
{
    public sealed class UnityStatRandomizer : IStatRandomizer
    {
        private readonly ICardSettings cardSettings;
        private readonly IReadOnlyReactiveCollection<Card> cards;
        private int currentIndex;
        private IDisposable subscription;

        public UnityStatRandomizer([NotNull] ICardSettings cardSettings, [NotNull] IReadOnlyReactiveCollection<Card> cards)
        {
            this.cardSettings = cardSettings ?? throw new ArgumentNullException(nameof(cardSettings));
            this.cards = cards ?? throw new ArgumentNullException(nameof(cards));
            
            subscription = cards.ObserveRemove().Subscribe(OnRemoved);
        }

        public void Run()
        {
            if (cards.Count == 0)
            {
                return;
            }

            Card card = cards[currentIndex];
            IReactiveProperty<int> stat = GetRandomStat(card);
            stat.Value = Random.Range(cardSettings.MinStatValue, cardSettings.MaxStatValue + 1);
            currentIndex++;

            if (currentIndex == cards.Count)
            {
                currentIndex = 0;
            }
        }

        private IReactiveProperty<int> GetRandomStat(Card card)
        {
            int choice = Random.Range(0, 3);
            return choice switch
            {
                0 => card.Attack,
                1 => card.Health,
                2 => card.Mana,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Dispose()
        {
            subscription?.Dispose();
            subscription = null;
        }

        private void OnRemoved(CollectionRemoveEvent<Card> removed)
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = 0;
            }
        }
    }
}