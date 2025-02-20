using System;
using System.Collections.Generic;

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