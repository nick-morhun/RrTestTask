using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace RrTestTask
{
    public sealed class DropTarget : MonoBehaviour
    {
        [SerializeField] private float dropRadius;
        private readonly CompositeDisposable cardSubscriptions = new CompositeDisposable();
        private IReadOnlyDictionary<Card, CardView> cardViews;
        private readonly List<Card> draggedCards = new List<Card>();
        private Transform cachedTransform;

        public void SetCardViews([CanBeNull] IReadOnlyDictionary<Card, CardView> cardViews)
        {
            cardSubscriptions.Clear();
            this.cardViews = cardViews;

            if (cardViews == null) return;

            foreach (Card card in cardViews.Keys)
            {
                card.IsDragged.Subscribe(value => OnIsDragged(card, value)).AddTo(cardSubscriptions);
            }
        }

        private void OnIsDragged(Card card, bool value)
        {
            if (value)
            {
                draggedCards.Add(card);
            }
            else
            {
                draggedCards.Remove(card);
            }
        }

        private void Start()
        {
            cachedTransform = transform;
        }

        private void Update()
        {
            foreach (Card card in draggedCards)
            {
                CardView view = cardViews[card];

                if (view)
                {
                    bool isClose = (view.transform.position - cachedTransform.position).sqrMagnitude <= dropRadius * dropRadius;
                    view.DragInput.SetDropTarget(isClose ? cachedTransform : null);
                }
            }
        }

        private void OnDestroy()
        {
            cardSubscriptions.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, dropRadius);
        }
    }
}