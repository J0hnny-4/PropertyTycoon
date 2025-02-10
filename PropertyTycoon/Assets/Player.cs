using System;
using System.Collections.Generic;

public abstract class Player
{
    private string name { get; }
    private string piece { get; }
    private int money { get; set; } = 1500; //TODO magic number

    private Square currentSquare => GameState.board[position];

    private int position = 0;
    public HashSet<Ownable> properties { get; } = new HashSet<Ownable>();
    public Tuple<int, int> lastRoll = new Tuple<int, int>(0, 0);
    private int doublesRolled = 0;
    private int turnsLeftInJail = 0;
    private List<GetOutOfJail> getOutOfJailCards = new List<GetOutOfJail>();

    protected Player(string name, string piece)
    {
        this.name = name;
        this.piece = piece;
    }

    /// <summary>
    /// Generates two dice (d6) rolls and return the sum.
    /// The last dice roll is stored in lastRoll.
    /// If doubles are rolled, doublesRolled is incremented.
    /// </summary>
    /// <returns> The sum of two d6 dice rolls </returns>
    private int rollDice()
    {
        Random rand = new Random();
        int die1 = rand.Next(1, 7);
        int die2 = rand.Next(1, 7);
        lastRoll = new Tuple<int, int>(die1, die2);
        if (die1 == die2)
        {
            doublesRolled++;
        }
        else
        {
            doublesRolled = 0;
        }

        return die1 + die2;
    }
    /// <summary>
    /// Performs the actions required when a player is in jail.
    /// </summary>
    /// <returns></returns>
    private void handleJAil()
    {
        //TODO give options to leave jail
        --turnsLeftInJail;
    }

    public void addMoney(int amount)
    {
        //TODO some nice animation
        money += amount;
    }
    
    public void addProperty(Ownable property) { properties.Add(property); }
    
    public int payMoney(int amount)
    {
        //TODO some nice animation, Possibly start bankrupt process here?
        if (money < amount)
        { 
            int ret = money;
            money = 0;
            return ret;
        }
        money -= amount;
        return amount;
    }

    public void goToJail()
    {
        //TODO Just a placeholder for now, remove magic numbers.
        position = 10;
        turnsLeftInJail = 3;
    }

    private bool move()
    {
        int roll = rollDice();
        if (doublesRolled == 3) //TODO magic number
        {
            goToJail();
            return false;
        }
        
        //TODO Separate method or two for actually setting player position may be in order
        position += roll;
        if (position >= GameState.boardSize)
        {
            position -= GameState.boardSize;
            addMoney(200); //TODO magic number
        }

        this.currentSquare.playerLands();
        return doublesRolled > 0;
    }

    public void takeTurn()
    {
        if (turnsLeftInJail > 0)
        {
            handleJAil();
            return;
        }

        while (turnsLeftInJail == 0 && move()) ;
    }
}