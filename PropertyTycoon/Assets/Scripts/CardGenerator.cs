using NUnit.Framework;
using System.Collections.Generic;
using BackEnd;
using Data;

public static class CardGenerator
{
    public static Dictionary<string, Queue<Card>> GenerateCards(string path = null) 
    {
        var decks = new Dictionary<string, Queue<Card>>();
        decks["Pot Luck"] = new Queue<Card>();
        decks["Opportunity Knocks"] = new Queue<Card>();
        
        decks["Pot Luck"].Enqueue(new Card("Pot Luck", "You inherit £200", "PayPlayer", 200));
        decks["Pot Luck"].Enqueue(new Card("Pot Luck", "Go to jail.", "GoToJail"));
        
        decks["Opportunity Knocks"].Enqueue(new Card("Opportunity Knocks", "Bank pays you divided of £50", "PayPlayer", 50));
        decks["Opportunity Knocks"].Enqueue(new Card("Opportunity Knocks", "Go to jail.", "GoToJail"));

        foreach (var deck in decks)
        {
            ShuffleQueue(deck.Value);
        }
        
        return decks;
    }
    
    /// <summary>
    /// Shuffles a queue in place using the Fisher-Yates algorithm.
    /// </summary>
    /// <param name="queue">The Queue to be shuffled in place</param>
    private static void ShuffleQueue<T>(Queue<T> queue)
    {
        var list = new List<T>(queue);
        var random = new System.Random();
        for(int i = list.Count; --i > 0;)
        {
            int j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        queue.Clear();
        foreach (var item in list)
        {
            queue.Enqueue(item);
        }
    }
}