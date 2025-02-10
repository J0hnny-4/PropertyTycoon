using System;

public class Card
{
    public string name { get; }
    public string description { get; }
    public Action effect { get; }

    public Card(string name, string description, Action effect)
    {
        this.name = name;
        this.description = description;
        this.effect = effect;
    }
}