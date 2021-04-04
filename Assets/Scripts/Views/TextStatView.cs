using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace RrTestTask
{
    public sealed class TextStatView : StatView
    {
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private float tweenDuration;
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
                    .To(() => currentValue.Value, current =>
                    {
                        currentValue = current;
                        valueText.text = current.ToString();
                    }, value, tweenDuration);
            }
            else
            {
                valueText.text = value.ToString();
                currentValue = value;
            }
        }
    }
}