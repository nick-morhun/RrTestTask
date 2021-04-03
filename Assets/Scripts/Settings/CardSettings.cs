using UnityEngine;

namespace RrTestTask
{
    [CreateAssetMenu(menuName = "Create CardSettings", fileName = "CardSettings", order = 0)]
    public sealed class CardSettings : ScriptableObject, ICardSettings
    {
        [SerializeField] private int minStatValue;
        [SerializeField] private int maxStatValue;
        [SerializeField] private int minHealthAliveValue;

        public int MinStatValue => minStatValue;

        public int MaxStatValue => maxStatValue;

        public int MinHealthAliveValue => minHealthAliveValue;
    }
}