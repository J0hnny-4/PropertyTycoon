using Data;

/// <summary>
/// Stations are an ownable square that charge rent based on the number of stations owned by the player.
/// </summary>
public class Station : Ownable
{
    private int Rent { get; }

    public Station(string name, int cost, int rent) : base(name, cost)
    {
        this.Rent = rent;
    }

    /// <summary>
    /// Counts the number of stations owned by the player and charges rent based on that number.
    /// </summary>
    protected override void ChargeRent()
    {
        var noOfStationsOwned = 0;
        foreach (var i in Owner.properties)
            if (i is Station)
                noOfStationsOwned++;
        int money = GameState.ActivePlayer.payMoney(Rent * noOfStationsOwned);
        Owner.addMoney(money);
    }
}