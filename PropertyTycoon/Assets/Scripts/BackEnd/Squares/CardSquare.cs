using System.Collections.Generic;
using BackEnd.Squares;
using Data;

namespace BackEnd.Squares
{
    /// <summary>
    /// When a player lands on a CardSquare, they draw a card from the deck and follow the instructions.
    /// The class stores a queue of cards, and when a player lands on the square, the card is drawn and the effect is executed.
    /// </summary>
    public class CardSquare : Square
    {
        private Queue<Card> _deck;

        public CardSquare(SquareData data, Queue<Card> deck) : base(data)
        {
            this._deck = deck;
        }

        /// <summary>
        /// Adds the card drawing effect to the player landing on the square.
        /// </summary>
        public override void PlayerLands()
        {
            if (_deck.Count == 0) return; // Deck with at least one non GetOutOfJail card should not be empty
            var card = _deck.Dequeue();
            card.Effect();
            if (card is not GetOutOfJail) _deck.Enqueue(card);
        }
    }
}