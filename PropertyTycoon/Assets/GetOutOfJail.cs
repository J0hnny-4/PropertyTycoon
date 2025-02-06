using System;
using System.Collections.Generic;

public class GetOutOfJail : Card
{
    private Queue<Card> homeDeck;

    public GetOutOfJail(string name, Func<Player> effect, Queue<Card> homeDeck) : base(name, effect)
    {
        this.homeDeck = homeDeck;
    }
}