using BackEnd;

namespace Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class to store the data of a player in the game.
    /// Functionality is defined by the Player class.
    /// </summary>
    public class PlayerData
    {
        public string Name { get; set; }
        public Token Token { get; set; }
        public int Money { get; set; } = Cons.StartingMoney; //TODO magic number
        public int Position { get; set; }
        public HashSet<int> Properties = new(); //Indices of properties in GameState.board
        public Tuple<int, int> LastRoll { get; set; } = new(0, 0);
        public int DoublesRolled { get; set; } = 0;
        public int TurnsLeftInJail { get; set; } = 0;

        public List<GetOutOfJail> GetOutOfJailCards { get; set; } =
            new(); //TODO maybe move to player class, replace with int

        public bool IsAi { get; set; } = false;
        public bool IsBankrupt => Money <= 0;

        public event Action OnStateUpdated;
        public event Action OnOwnedPropertiesUpdated;
        public event Action<PlayerData> OnBankrupted;
        public event Action OnTeleported;
        
        public void TriggerOnUpdateEvent() => OnStateUpdated?.Invoke();
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Players username.</param>
        /// <param name="token">Enum for the piece they are controlling.</param>
        /// <param name="isAi">Bool for denoting a human or bot.</param>
        public PlayerData(string name, Token token, bool isAi = false)
        {
            this.Name = name;
            this.Token = token;
            this.IsAi = isAi;
        }

        /// <summary>
        /// Used when a player gains money for any reason.
        /// </summary>
        /// <param name="amount">The amount of money to add to their total.</param>
        public void AddMoney(int amount)
        {
            Money += amount;
            OnStateUpdated?.Invoke();
        }
        
        /// <summary>
        /// Removes an amount of money from the player's total.
        /// If they do not have enough money, the bankrupt process starts.
        /// </summary>
        /// <param name="amount">Amount of money needed to be paid</param>
        /// <returns></returns>
        public int TakeMoney(int amount)
        {
            var amountPaid = Math.Min(amount, Money);
            Money -= amountPaid;
            OnStateUpdated?.Invoke();
            if (IsBankrupt) { OnBankrupted?.Invoke(this); }
            return amountPaid;
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
            OnOwnedPropertiesUpdated?.Invoke();
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
            OnOwnedPropertiesUpdated?.Invoke();
        }
    }
}
