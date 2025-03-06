using System;
using System.Collections.Generic;
using Data;

namespace BackEnd
{
    /// <summary>
    /// A class representing a player in the game.
    /// Stores data via the PlayerData class.
    /// Provide functionality for taking turns, moving, and interacting with the board.
    /// This is an abstract class, and should be extended to create a specific player type, human or AI.
    /// </summary>
    public abstract class Player
    {
        protected PlayerData Data { get; }

        protected int Money
        {
            get => Data.Money;
            set => Data.Money = value;
        }

        protected int Position
        {
            get => Data.Position;
            set => Data.Position = value;
        }

        protected Tuple<int, int> LastRoll
        {
            get => Data.LastRoll;
            set => Data.LastRoll = value;
        }

        protected int DoublesRolled
        {
            get => Data.DoublesRolled;
            set => Data.DoublesRolled = value;
        }

        protected int TurnsLeftInJail
        {
            get => Data.TurnsLeftInJail;
            set => Data.TurnsLeftInJail = value;
        }

        protected List<GetOutOfJail> GetOutOfJailCards
        {
            get => Data.GetOutOfJailCards;
            set => Data.GetOutOfJailCards = value;
        }

        protected HashSet<int> Properties => Data.Properties;

        protected Player(PlayerData data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Generates two dice (d6) rolls and return the sum.
        /// The last dice roll is stored in lastRoll.
        /// If doubles are rolled, doublesRolled is incremented.
        /// </summary>
        /// <returns> The sum of two d6 dice rolls </returns>
        protected int RollDice()
        {
            var rand = new Random();
            var die1 = rand.Next(1, 7);
            var die2 = rand.Next(1, 7);
            LastRoll = new Tuple<int, int>(die1, die2);
            if (die1 == die2)
                DoublesRolled++;
            else
                DoublesRolled = 0;

            return die1 + die2;
        }

        /// <summary>
        /// Performs the actions required when a player is in jail.
        /// </summary>
        /// <returns></returns>
        protected void HandleJAil()
        {
            //TODO give options to leave jail
            --TurnsLeftInJail;
        }

        /// <summary>
        /// Adds a property to the player's list of owned properties.
        /// Used for purchases or trades.
        /// The int is the index of the property in GameState.board.
        /// </summary>
        /// <param name="property">The index of the property the player gained</param>
        public void AddProperty(int property)
        {
            Properties.Add(property);
        }

        /// <summary>
        /// Removes a property from the player's list of owned properties.
        /// Used for trades.
        /// The int is the index of the property in GameState.board.
        /// </summary>
        /// <param name="property">The index of the property the player lost</param>
        public void RemoveProperty(int property)
        {
            Properties.Remove(property);
        }

        /// <summary>
        /// Used when a player gains money for any reason.
        /// </summary>
        /// <param name="amount">The amount of money to add to their total.</param>
        public void AddMoney(int amount)
        {
            //TODO some nice animation
            Money += amount;
        }

        /// <summary>
        /// Removes an amount of money from the player's total.
        /// If they do not have enough money, the bankrupt process starts.
        /// </summary>
        /// <param name="amount">Amount of money needed to be paid</param>
        /// <returns></returns>
        public int PayMoney(int amount)
        {
            //TODO some nice animation, Possibly start bankrupt process here?
            if (Money < amount)
            {
                var ret = Money;
                Money = 0;
                return ret;
            }

            Money -= amount;
            return amount;
        }

        /// <summary>
        /// Sends the player to jail.
        /// Sets their position to the jail square and sets the number of turns left in jail to the appropriate number.
        /// </summary>
        public void GoToJail()
        {
            //TODO Just a placeholder for now, remove magic numbers.
            Position = 10;
            TurnsLeftInJail = 3;
        }

        /// <summary>
        /// Moves the player around the board based on a dice roll.
        /// Takes any actions required when landing on a square.
        /// </summary>
        /// <returns></returns>
        protected bool Move()
        {
            var roll = RollDice();
            if (DoublesRolled == 3) //TODO magic number
            {
                GoToJail();
                return false;
            }

            //TODO Separate method or two for actually setting player position may be in order
            Position += roll;
            if (Position >= GameState.BoardSize)
            {
                Position -= GameState.BoardSize;
                AddMoney(200); //TODO magic number
            }

            return DoublesRolled > 0;
        }

        /// <summary>
        /// Performs the actions required for a player's turn.
        /// Rolling the dice up to 3 times, moving around the board, and taking any actions required when landing on a square.
        /// </summary>
        //TODO: This functionality may need to be moved to a gamecontroller class
        public void TakeTurn()
        {
            if (TurnsLeftInJail > 0)
            {
                HandleJAil();
                return;
            }

            while (TurnsLeftInJail == 0 && Move()) ;
        }
    }
}