using System;
using UniRx;
using UnityEngine;

namespace RrTestTask
{
    public abstract class StatView : MonoBehaviour
    {
        private IDisposable subscription;

        public void SetModel(IObservable<int> value)
        {
            subscription?.Dispose();
            subscription = null;

            if (value != null)
            {
                subscription = value.Subscribe(Show);
            }
        }

        protected abstract void Show(int value);

        protected virtual void Awake()
        {
        }
    }
}