using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using UI.Board;
using UI.Game;

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

        public PlayerData Data { get; }

        public string Name
        {
            get => Data.Name;
        }

        public int Money
        {
            get => Data.Money;
            protected set { Data.Money = value; }
        }

        public int Position
        {
            get => Data.Position;
            protected set => Data.Position = value;
        }

        public Tuple<int, int> LastRoll
        {
            get => Data.LastRoll;
            protected set => Data.LastRoll = value;
        }

        public int DoublesRolled
        {
            get => Data.DoublesRolled;
            protected set => Data.DoublesRolled = value;
        }

        public int TurnsLeftInJail
        {
            get => Data.TurnsLeftInJail;
            protected set { Data.TurnsLeftInJail = value; Data.TriggerOnUpdateEvent(); }
        }

        public bool IsBankrupt => Data.IsBankrupt;

        public List<GetOutOfJail> GetOutOfJailCards
        {
            get => Data.GetOutOfJailCards;
            protected set => Data.GetOutOfJailCards = value;
        }

        public HashSet<int> Properties => Data.Properties;

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
        public int RollDice()
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
        /// <returns>True if player has left jail, false otherwise</returns>
        public void HandleJAil()
        {
            if (DoublesRolled > 0)
            {
                TurnsLeftInJail = 0;
                DoublesRolled = 0;
            }
            if (TurnsLeftInJail > 0)
            {
                TurnsLeftInJail -= 1;
            }

        }

        /// <summary>
        /// Moves the player around the board based on a dice roll.
        /// Takes any actions required when landing on a square.
        /// </summary>
        /// <returns></returns>
        public async Task Move()
        {
            if (DoublesRolled == 3) //TODO magic number
            {
                await Data.GoToJail();
                return;
            }

            var roll = LastRoll.Item1 + LastRoll.Item2;

            //TODO Separate method or two for actually setting player position may be in order
            if (Position + roll >= GameState.BoardSize)
            {
                Position = (Position + roll) % GameState.BoardSize;
                Data.AddMoney(200); //TODO magic number
            }
            else
            {
                Position += roll;
            }
        }

        /// <summary>
        /// Performs the actions required for a player's turn.
        /// Rolling the dice up to 3 times, moving around the board, and taking any actions required when landing on a square.
        /// </summary>
        //TODO: This functionality may need to be moved to a gamecontroller class
        public void TakeTurn()
        {
            // while (RollDice() > 0 && HandleJAil() && Move()) ;
        }
    }
}
