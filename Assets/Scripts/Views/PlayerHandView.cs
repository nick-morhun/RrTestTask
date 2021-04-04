using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace RrTestTask
{
    public sealed class PlayerHandView : MonoBehaviour
    {
        [SerializeField] private Transform cardsParent;
        [SerializeField] private float radius = 1f;
        [SerializeField] private float cardAngularSize = 5f;
        [SerializeField] private float cardDestructionDelaySec;

        private readonly PlayerHandLayout layout = new PlayerHandLayout();
        private readonly Dictionary<Card, CardView> cardViews = new Dictionary<Card, CardView>();
        private readonly CompositeDisposable collectionSubscriptions = new CompositeDisposable();
        private readonly Dictionary<Card,IDisposable> subscriptions = new Dictionary<Card,IDisposable>();
        private IReadOnlyReactiveCollection<Card> model;
        private CancellationTokenSource removalCancellation;

        public IReadOnlyDictionary<Card, CardView> CardViews => cardViews;

        public void SetModel([NotNull] IReadOnlyReactiveCollection<Card> cards, [NotNull] CardViewFactory cardViewFactory)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));
            if (cardViewFactory == null) throw new ArgumentNullException(nameof(cardViewFactory));

            ClearModel();
            model = cards;
            removalCancellation = new CancellationTokenSource();

            foreach (Card card in cards)
            {
                SubscribeToCard(card);
                CardView view = cardViewFactory.Create(card);
                cardViews.Add(card, view);
            }

            cards.ObserveRemove().Subscribe(OnRemoved).AddTo(collectionSubscriptions);
            PositionCards();
        }

        private void PositionCards()
        {
            IReadOnlyList<Tuple<Vector3, Quaternion>> layouts = layout.Calculate(radius, cardAngularSize, model.Count);
            var i = 0;

            foreach (Card card in model)
            {
                (Vector3 position, Quaternion rotation) = layouts[i];
                CardView view = cardViews[card];
                Transform viewTransform = view.transform;
                viewTransform.SetParent(cardsParent);
                viewTransform.localPosition = position;
                viewTransform.localRotation = rotation;
                i++;
            }
        }

        public void ClearModel()
        {
            foreach (CardView cardView in cardViews.Values)
            {
                cardView.ClearModel();
                Destroy(cardView.gameObject);
            }

            cardViews.Clear();

            foreach (IDisposable subscription in subscriptions.Values)
            {
                subscription.Dispose();
            }

            subscriptions.Clear();
            collectionSubscriptions.Clear();
            model = null;

            if (removalCancellation != null && removalCancellation.Token.CanBeCanceled)
            {
                removalCancellation.Cancel();
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(cardsParent);
        }

        private void SubscribeToCard(Card card)
        {
            IDisposable subscription = card.IsDead.Subscribe(vaue => CheckDeadAndRemove(vaue, card));
            subscriptions[card] = subscription;
        }

        private void CheckDeadAndRemove(bool isDead, Card card)
        {
            if (!isDead)
            {
                return;
            }

            subscriptions[card].Dispose();
            subscriptions.Remove(card);
            CardView cardView = cardViews[card];
            cardViews.Remove(card);

            UniTask.Delay(TimeSpan.FromSeconds(cardDestructionDelaySec), DelayType.DeltaTime, cancellationToken: removalCancellation.Token)
                .ContinueWith(() =>
                {
                    cardView.ClearModel();
                    Destroy(cardView.gameObject);
                    RepositionCards();
                });
        }

        private void OnRemoved(CollectionRemoveEvent<Card> removed)
        {
            if (removed.Value.IsDead.Value)
            {
                return;
            }

            Card card = removed.Value;
            subscriptions[card].Dispose();
            subscriptions.Remove(card);
            cardViews.Remove(card);
            RepositionCards();
        }

        private void RepositionCards()
        {
            IReadOnlyList<Tuple<Vector3, Quaternion>> layouts = layout.Calculate(radius, cardAngularSize, model.Count);
            var i = 0;

            foreach (Card card in model)
            {
                (Vector3 position, Quaternion rotation) = layouts[i];
                CardView view = cardViews[card];
                view.MoveTo(position, rotation);
                i++;
            }
        }
    }
}