public class Property : Ownable
{
    public int[] rent { get; }
    public string colour { get; }
    public int houseCost { get; }
    public int houses { get; private set; } = 0;
    
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
    
    protected override void chargeRent()
    {
        int rentDue = rent[houses];
        if(houses == 0 && ownerHasSet) rentDue *= 2;
        int money = GameState.currentPlayer.payMoney(rentDue);
        owner.addMoney(money);
    }
    
    //TODO Do this properly
    public void buyHouse()
    {
        if (houses < 4 
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