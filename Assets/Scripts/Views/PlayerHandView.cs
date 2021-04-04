using System;
using System.Collections.Generic;
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

        private readonly PlayerHandLayout layout = new PlayerHandLayout();
        private readonly List<CardView> cardViews = new List<CardView>();
        private IDisposable subscription;
        private IReadOnlyReactiveCollection<Card> model;

        public void SetModel([NotNull] IReadOnlyReactiveCollection<Card> cards, [NotNull] CardViewFactory cardViewFactory)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));
            if (cardViewFactory == null) throw new ArgumentNullException(nameof(cardViewFactory));

            ClearModel();
            model = cards;

            foreach (Card card in cards)
            {
                CardView view = cardViewFactory.Create(card);
                cardViews.Add(view);
            }

            subscription = cards.ObserveRemove().Subscribe(OnRemoved);
            PositionCards();
        }

        private void PositionCards()
        {
            IReadOnlyList<Tuple<Vector3, Quaternion>> layouts = layout.Calculate(radius, cardAngularSize, model.Count);

            for (var i = 0; i < cardViews.Count; i++)
            {
                CardView view = cardViews[i];
                (Vector3 position, Quaternion rotation) = layouts[i];
                Transform viewTransform = view.transform;
                viewTransform.SetParent(cardsParent);
                viewTransform.localPosition = position;
                viewTransform.localRotation = rotation;
            }
        }

        public void ClearModel()
        {
            foreach (CardView cardView in cardViews)
            {
                cardView.SetModel(null, Array.Empty<Sprite>());
                Destroy(cardView);
            }

            cardViews.Clear();
            subscription?.Dispose();
            subscription = null;
            model = null;
        }

        private void Awake()
        {
            Assert.IsNotNull(cardsParent);
        }

        private void OnRemoved(CollectionRemoveEvent<Card> removed)
        {
            cardViews[removed.Index].ClearModel();
            PositionCards();
        }
    }
}