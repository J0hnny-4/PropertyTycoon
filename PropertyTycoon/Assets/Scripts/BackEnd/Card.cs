using System;

/// <summary>
/// A class representing a card in the game, potluck, hard knocks etc.
/// Functionally of the card is defined by a lambda function.
/// </summary>
public class Card
{
    public string Name { get; }
    public string Description { get; }
    public Action Effect { get; }

    public Card(string name, string description, Action effect)
    {
        this.Name = name;
        this.Description = description;
        this.Effect = effect;
    }
}