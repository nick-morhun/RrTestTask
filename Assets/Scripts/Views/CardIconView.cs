using JetBrains.Annotations;
using UnityEngine;

namespace RrTestTask
{
    public abstract class CardIconView : MonoBehaviour
    {
        public abstract void SetModel([CanBeNull] Sprite icon);

        protected virtual void Awake()
        {
        }
    }
}