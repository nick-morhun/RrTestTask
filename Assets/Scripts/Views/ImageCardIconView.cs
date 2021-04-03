using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace RrTestTask
{
    public sealed class ImageCardIconView : CardIconView
    {
        [SerializeField] private Image iconImage;

        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(iconImage);
        }

        public override void SetModel(Sprite icon)
        {
            iconImage.sprite = icon;
        }
    }
}