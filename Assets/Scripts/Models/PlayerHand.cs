using System.Collections.Generic;
using UniRx;

namespace RrTestTask
{
    public sealed class PlayerHand
    {
        public ReactiveCollection<Card> Cards { get; }

        public PlayerHand(IEnumerable<Card> cards)
        {
            Cards = new ReactiveCollection<Card>(cards);
        }
    }
}