using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace RrTestTask
{
    public sealed class StatRandomizerView : MonoBehaviour
    {
        [SerializeField] private Button button;

        private IStatRandomizer model;

        public void SetModel(IStatRandomizer value)
        {
            model = value;
            button.interactable = value != null;
        }

        private void Awake()
        {
            Assert.IsNotNull(button);
            button.onClick.AsObservable().Subscribe(OnClick).AddTo(this);
            button.interactable = false;
        }

        private void OnClick(Unit _)
        {
            model.Run();
        }
    }
}