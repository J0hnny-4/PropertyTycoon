using System;
using System.Collections.Generic;

/// <summary>
/// A special variant of the card class that can be stored by the player for later use.
/// Allows the player to leave jail imediatly for free.
/// Extends the Card class to add functionality for returning the card to the deck.
/// </summary>
public class GetOutOfJail : Card
{
    private Queue<Card> homeDeck;

    public GetOutOfJail(string name, string description, Action effect, Queue<Card> homeDeck) : base(name, description, effect)
    {
        this.homeDeck = homeDeck;
    }
    
    public void returnToDeck()
    {
        homeDeck.Enqueue(this);
    }
}