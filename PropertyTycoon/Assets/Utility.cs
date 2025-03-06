using System;

/// <summary>
/// And ownable square that cheges rent based on the last dice roll and the number of utilities owned by the player.
/// </summary>
public class Utility : Ownable
{
    private int[] rent { get; }

    public Utility(string name, int cost, int[] rent) : base(name, cost) 
    {
        this.rent = rent;
    }

    protected override void chargeRent()
    {
        int noOfUtilitiesOwned = 0;
        foreach (var i in owner.properties)
        {
            if (i is Station) noOfUtilitiesOwned++;
        }
        noOfUtilitiesOwned = Math.Min(noOfUtilitiesOwned, rent.Length) - 1;
        int rentOwed = rent[noOfUtilitiesOwned] * (GameState.currentPlayer.lastRoll.Item1 + GameState.currentPlayer.lastRoll.Item2);
        int money = GameState.currentPlayer.payMoney(rentOwed);
        owner.addMoney(money);
    }
}