using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;

namespace RrTestTask
{
    public sealed class PlayerHand
    {
        public ReactiveCollection<Card> Cards { get; }

        private readonly ICardSettings cardSettings;
        private readonly List<IDisposable> subscriptions = new List<IDisposable>();
        private IDisposable subscription;

        public PlayerHand([NotNull] IEnumerable<Card> cards, [NotNull] ICardSettings cardSettings)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));

            this.cardSettings = cardSettings ?? throw new ArgumentNullException(nameof(cardSettings));
            Cards = new ReactiveCollection<Card>(cards);

            subscription = Cards.ObserveRemove().Subscribe(OnRemoved);

            foreach (Card card in Cards)
            {
                card.Health.Subscribe(health => CheckAndRemove(health, card)).AddTo(subscriptions);
            }
        }

        public void Dispose()
        {
            foreach (IDisposable subscription in subscriptions)
            {
                subscription.Dispose();
            }

            subscriptions.Clear();
            subscription?.Dispose();
            subscription = null;
        }

        private void CheckAndRemove(int health, Card card)
        {
            if (health < cardSettings.MinHealthAliveValue)
            {
                Cards.Remove(card);
            }
        }

        private void OnRemoved(CollectionRemoveEvent<Card> removed)
        {
            subscriptions[removed.Index].Dispose();
            subscriptions.RemoveAt(removed.Index);
        }
    }
}