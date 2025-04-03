using System;
using System.Collections.Generic;

namespace BackEnd
{
    /// <summary>
    /// A special variant of the card class that can be stored by the player for later use.
    /// Allows the player to leave jail imediatly for free.
    /// Extends the Card class to add functionality for returning the card to the deck.
    /// </summary>
    public class GetOutOfJail : Card
    {
        private Queue<Card> _homeDeck;

        public GetOutOfJail(string name, string description, string effect, Queue<Card> homeDeck) : base(name, description,
            effect)
        {
            this._homeDeck = homeDeck;
        }

        public void ReturnToDeck()
        {
            _homeDeck.Enqueue(this);
        }
    }
}