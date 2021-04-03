using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace RrTestTask
{
    public sealed class TextStatView : StatView
    {
        [SerializeField] private TMP_Text valueText;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(valueText);
        }

        protected override void Show(int value)
        {
            valueText.text = value.ToString();
        }
    }
}