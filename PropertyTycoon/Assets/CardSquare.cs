using System.Collections.Generic;

/// <summary>
/// When a player lands on a CardSquare, they draw a card from the deck and follow the instructions.
/// The class stores a queue of cards, and when a player lands on the square, the card is drawn and the effect is executed.
/// </summary>
public class CardSquare : Square
{
        private Queue<Card> deck;
        public CardSquare(string name, Queue<Card> deck) : base(name)
        {
            this.deck = deck;
        }
        
        /// <summary>
        /// Adds the card drawing effect to the player landing on the square.
        /// </summary>
        public override void playerLands()
        {
            if(deck.Count == 0) return; // Deck with at least one non GetOutOfJail card should not be empty
            Card card = deck.Dequeue();
            card.effect();
            if(card is not GetOutOfJail) deck.Enqueue(card);
        }
}