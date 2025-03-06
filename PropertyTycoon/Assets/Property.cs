/// <summary>
/// The coloured properties on the board that can be bought and sold.
/// They are own-able, rent can be charged, and houses/hotels can be built.
/// Features are unlocked when a player owns all properties of the same colour.
/// </summary>
public class Property : Ownable
{
    public int[] rent { get; }
    public string colour { get; }
    public int houseCost { get; }
    public int houses { get; private set; } = 0;
    
    /// <summary>
    /// Uses the GameState to count the number of properties in the set.
    /// </summary>
    /// <returns>The number of properties that share a colour with this one.</returns>
    public int numberInSet
    {
        get
        {
            int count = 0;
            foreach (var i in GameState.board)
            {
                if (i is Property p && p.colour == colour) count++;
            }
            return count;
        }
    }

    public Property(string name, int cost, int[] rent, string color, int houseCost) : base(name, cost)
    {
        this.rent = rent;
        this.colour = color;
        this.houseCost = houseCost;
    }
    
    /// <summary>
    /// Calculates and charges rent based on the number of properties in the set and the number of houses on the property.
    /// </summary>
    protected override void chargeRent()
    {
        int rentDue = rent[houses];
        if(houses == 0 && ownerHasSet) rentDue *= 2;
        int money = GameState.currentPlayer.payMoney(rentDue);
        owner.addMoney(money);
    }
    
    /// <summary>
    /// Allows the player to buy a house/hotel on the property if they own all properties in the set.
    /// The difference between number of houses on a property should never be more than 1.
    /// Hotels are considered to be 5 houses.
    /// </summary>
    /// //TODO Do this properly
    public void buyHouse()
    {
        if (houses <= 4 
            && owner == GameState.currentPlayer
            && ownerHasSet)
        {
            GameState.currentPlayer.payMoney(houseCost); // 
            houses++;
        }
    }
    //TODO Do this properly too
    public void sellHouse()
    {
        if (houses > 0 && owner == GameState.currentPlayer)
        {
            GameState.currentPlayer.addMoney(houseCost / 2);
            houses--;
        }
    }

    public bool ownerHasSet
    {
        get
        {
            int count = 0;
            foreach (var i in owner.properties)
            {
                if (i is Property p && p.colour == colour) count++;
            }
            return count == numberInSet;
        }
    }
}