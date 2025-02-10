public abstract class Ownable : Square
{
    private int cost { get; }
    private bool mortgaged { get; set; } = false;
    protected Player owner { get; set; } = null;

    
    protected Ownable(string name, int cost) : base(name)
    {
        this.cost = cost;
    }
    
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
    private void auction()
    {
        //TODO Auction should return the player who won the auction
        Player winner = GameState.currentPlayer;
        owner = winner;
        winner.addProperty(this);
    }
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

    protected abstract void chargeRent();
    public void mortgage()
    {
        mortgaged = true;
        owner.addMoney(cost / 2);
    }
}