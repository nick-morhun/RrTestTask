using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace RrTestTask
{
    public sealed class TextHealthView : StatView
    {
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private float tweenDuration;
        [SerializeField] private int minDisplayValue;

        private int? currentValue;
        [CanBeNull] private Tween currentTween;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(valueText);
        }

        protected override void Show(int value)
        {
            if (currentValue.HasValue)
            {
                currentTween.Kill();
                currentTween = DOTween
                    .To(() => currentValue.Value, SetCurrent, value, tweenDuration);
            }
            else
            {
                SetCurrent(value);
            }
        }

        private void SetCurrent(int value)
        {
            currentValue = Mathf.Max(value, minDisplayValue);
            valueText.text = currentValue.Value.ToString();
        }
    }
}