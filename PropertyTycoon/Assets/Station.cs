/// <summary>
/// Stations are an ownable square that charge rent based on the number of stations owned by the player.
/// </summary>
public class Station : Ownable
{
    private int rent { get; }
    public Station(string name, int cost, int rent) : base(name, cost)
    {
        this.rent = rent;
    }
    
    /// <summary>
    /// Counts the number of stations owned by the player and charges rent based on that number.
    /// </summary>
    protected override void chargeRent()
    {
        int noOfStationsOwned = 0;
        foreach(var i in owner.properties)
        {
            if(i is Station) noOfStationsOwned++;
        }
        int money = GameState.currentPlayer.payMoney(rent * noOfStationsOwned);
        owner.addMoney(money);
    }
}