using System;

/// <summary>
/// A class representing a card in the game, potluck, hard knocks etc.
/// Functionally of the card is defined by a lambda function.
/// </summary>
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