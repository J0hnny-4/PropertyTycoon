/// <summary>
/// Extension of the Square class that can be owned by a player.
/// Provides the interface for charging rent, buy must be implemented by the subclass.
/// </summary>
public abstract class Ownable : Square
{
    private int cost { get; }
    private bool mortgaged { get; set; } = false;
    protected Player owner { get; set; } = null;

    
    protected Ownable(string name, int cost) : base(name)
    {
        this.cost = cost;
    }
    
    /// <summary>
    /// Gives the player the option to buy the property, otherwise it is auctioned. 
    /// </summary>
    private void buy()
    {
        //TODO menu options
        if(true)
        {
            GameState.currentPlayer.payMoney(cost);
            owner = GameState.currentPlayer;
            GameState.currentPlayer.addProperty(this);
        }
        else auction();
    }
    
    /// <summary>
    /// Auctions off the property to all the players.
    /// </summary>
    private void auction()
    {
        //TODO Auction should return the player who won the auction
        Player winner = GameState.currentPlayer;
        owner = winner;
        winner.addProperty(this);
    }
    
    /// <summary>
    /// Adds the functionality to purchase the property or charge rent on landing
    /// </summary>
    public override void playerLands()
    {
        if (owner == null)
        {
            buy();
        }
        else if (owner != GameState.currentPlayer && !mortgaged)
        {
            chargeRent();
        }
    }

    /// <summary>
    /// Charge the landing player rent for landing on the property.
    /// Must be implemented by the subclass.
    /// </summary>
    protected abstract void chargeRent();
    
    /// <summary>
    /// Sets the property to mortgaged and gives the owner half the cost of the property.
    /// Rent cannot be charged on mortgaged properties.
    /// </summary>
    public void mortgage()
    {
        mortgaged = true;
        owner.addMoney(cost / 2);
    }
}