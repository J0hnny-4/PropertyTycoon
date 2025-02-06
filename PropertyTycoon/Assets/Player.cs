using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class Player
{
    public string name { get; }
    public string piece { get; }
    public int money { get; private set; } = 0;
    
    public Square currentSquare
    {
        get
        {
            //TODO get square from GameState
            return null;
        }
    }
    
    private int position = 0;
    private HashSet<Ownable> properties = new HashSet<Ownable>();
    private Tuple<int, int> lastRoll = new Tuple<int, int>(0, 0);
    private int doublesRolled = 0;
    private int turnsLeftInJail = 0;
    private List<GetOutOfJail> getOutOfJailCards = new List<GetOutOfJail>();

    protected Player(string name, string piece)
    {
        this.name = name;
        this.piece = piece;
    }

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
    
    private bool handleJAil()
    {
        //TODO give options to leave jail
        if (turnsLeftInJail > 0)
        {
            turnsLeftInJail--;
            return true;
        }
        return false;
    }
    
    public void addMoney(int amount)
    {
        //TODO some nice animation
        money += amount;
    }
    
    public void goToJail()
    {
        //TODO just a placeholder for now
        position = 10;
        turnsLeftInJail = 3;
    }

    public void move()
    {
        if (handleJAil())
        {
            return;
        }

        int roll = rollDice();
        if(doublesRolled == 3)
        {
            goToJail();
            return;
        }

        position += roll;
        if (position >= 40)
        {
            position -= 40;
            addMoney(200);
        }
        this.currentSquare.playerLands(this);
    }
}