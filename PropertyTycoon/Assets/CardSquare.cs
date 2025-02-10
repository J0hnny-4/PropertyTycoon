using System.Collections.Generic;

public class CardSquare : Square
{
        private Queue<Card> deck;
        public CardSquare(string name, Queue<Card> deck) : base(name)
        {
            this.deck = deck;
        }
        public override void playerLands()
        {
            if(deck.Count == 0) return; // Deck with at least one non GetOutOfJail card should not be empty
            Card card = deck.Dequeue();
            card.effect();
            if(card is not GetOutOfJail) deck.Enqueue(card);
        }
}