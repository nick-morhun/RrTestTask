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
        private readonly List<IDisposable> healthSubscriptions = new List<IDisposable>();
        private readonly List<IDisposable> droppedSubscriptions = new List<IDisposable>();
        private readonly CompositeDisposable cardSubscriptions = new CompositeDisposable();
        private readonly CompositeDisposable collectionSubscriptions = new CompositeDisposable();

        public PlayerHand([NotNull] IEnumerable<Card> cards, [NotNull] ICardSettings cardSettings)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));

            this.cardSettings = cardSettings ?? throw new ArgumentNullException(nameof(cardSettings));
            Cards = new ReactiveCollection<Card>(cards);
            Cards.ObserveRemove().Subscribe(OnRemoved).AddTo(collectionSubscriptions);

            foreach (Card card in Cards)
            {
                SubscribeToCard(card);
            }
        }

        public void Dispose()
        {
            cardSubscriptions.Clear();
            collectionSubscriptions.Clear();
        }

        private void CheckHealthAndRemove(int health, Card card)
        {
            if (health < cardSettings.MinHealthAliveValue)
            {
                card.IsDead.Value = true;
                Cards.Remove(card);
            }
        }

        private void CheckDroppedAndRemove(bool dropped, Card card)
        {
            if (dropped)
            {
                Cards.Remove(card);
            }
        }
        
        private void SubscribeToCard(Card card)
        {
            card.Health.Subscribe(health => CheckHealthAndRemove(health, card))
                .AddTo(healthSubscriptions)
                .AddTo(cardSubscriptions);
            card.IsDropped.Subscribe(dropped => CheckDroppedAndRemove(dropped, card))
                .AddTo(droppedSubscriptions)
                .AddTo(cardSubscriptions);
        }

        private void OnRemoved(CollectionRemoveEvent<Card> removed)
        {
            IDisposable healthSubscription = healthSubscriptions[removed.Index];
            healthSubscription.Dispose();
            healthSubscriptions.RemoveAt(removed.Index);
            IDisposable droppedSubscription = droppedSubscriptions[removed.Index];
            droppedSubscription.Dispose();
            droppedSubscriptions.RemoveAt(removed.Index);
            cardSubscriptions.Remove(healthSubscription);
            cardSubscriptions.Remove(droppedSubscription);
        }
    }
}