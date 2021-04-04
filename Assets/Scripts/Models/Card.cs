using UniRx;

namespace RrTestTask
{
    public class Card
    {
        public Card(int icon)
        {
            Icon = icon;
        }

        public int Icon { get; }
        public IReactiveProperty<int> Attack { get; } = new ReactiveProperty<int>();
        public IReactiveProperty<int> Mana { get; } = new ReactiveProperty<int>();
        public IReactiveProperty<int> Health { get; } = new ReactiveProperty<int>();
        public IReactiveProperty<bool> IsDead { get; } = new BoolReactiveProperty();
        public IReactiveProperty<bool> IsMoving { get; } = new BoolReactiveProperty();
        public IReactiveProperty<bool> IsDragged { get; } = new BoolReactiveProperty();
        public IReactiveProperty<bool> IsDropped { get; } = new BoolReactiveProperty();
        public bool CanDrag => !IsMoving.Value && !IsDead.Value && !IsDropped.Value;
    }
}