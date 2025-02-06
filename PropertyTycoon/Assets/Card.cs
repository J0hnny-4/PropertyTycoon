using System;

public class Card
{
    public string name { get; }
    public Func<Player> effect { get; }

    public Card(string name, Func<Player> effect)
    {
        this.name = name;
        this.effect = effect;
    }
}