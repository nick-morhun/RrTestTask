using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace RrTestTask
{
    public sealed class CardDragView : MonoBehaviour
    {
        [SerializeField] private GameObject dragMarker;
        private IDisposable subscription;

        public void SetModel([CanBeNull] Card card)
        {
            subscription?.Dispose();
            subscription = null;

            if (card != null)
            {
                subscription = card.IsDragged.Subscribe(OnIsDragged);
            }
        }

        private void OnIsDragged(bool value)
        {
            dragMarker.SetActive(value);
        }

        private void Awake()
        {
            Assert.IsNotNull(dragMarker);
        }

        private void OnDestroy()
        {
            subscription?.Dispose();
        }
    }
}