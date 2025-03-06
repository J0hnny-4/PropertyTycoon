using System;
using System.Collections.Generic;

/// <summary>
/// A class representing a player in the game.
/// Stores data including name, piece, money, properties, and position.
/// Provide functionality for taking turns, moving, and interacting with the board.
/// This is an abstract class, and should be extended to create a specific player type, human or AI.
/// </summary>

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
    
    /// <summary>
    /// Used when a player gains money for any reason.
    /// </summary>
    /// <param name="amount">The amount of money to add to their total.</param>
    public void addMoney(int amount)
    {
        //TODO some nice animation
        money += amount;
    }
    
    /// <summary>
    /// Adds a property to the player's list of owned properties.
    /// Used for purchases or trades.
    /// </summary>
    /// <param name="property">The property the plater gained</param>
    public void addProperty(Ownable property) { properties.Add(property); }
    
    /// <summary>
    /// Removes an amount of money from the player's total.
    /// If they do not have enough money, the bankrupt process starts.
    /// </summary>
    /// <param name="amount">Amount of money needed to be paid</param>
    /// <returns></returns>
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

    /// <summary>
    /// Sends the player to jail.
    /// Sets their position to the jail square and sets the number of turns left in jail to the appropriate number.
    /// </summary>
    public void goToJail()
    {
        //TODO Just a placeholder for now, remove magic numbers.
        position = 10;
        turnsLeftInJail = 3;
    }
    
    /// <summary>
    /// Moves the player around the board based on a dice roll.
    /// Takes any actions required when landing on a square.
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// Performs the actions required for a player's turn.
    /// Rolling the dice up to 3 times, moving around the board, and taking any actions required when landing on a square.
    /// </summary>
    //TODO: This functionality may need to be moved to a gamecontroller class
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