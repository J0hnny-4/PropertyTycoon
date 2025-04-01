using BackEnd;
using UnityEngine.Events;

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
        public int Money { get; set; } = 1500; //TODO magic number
        public int Position { get; set; }
        public HashSet<int> Properties = new(); //Indices of properties in GameState.board
        public Tuple<int, int> LastRoll { get; set; } = new(0, 0);
        public int DoublesRolled { get; set; } = 0;
        public int TurnsLeftInJail { get; set; } = 0;

        public List<GetOutOfJail> GetOutOfJailCards { get; set; } =
            new(); //TODO maybe move to player class, replace with int

        public bool IsAi { get; set; } = false;

        public event Action OnUpdate;

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

        public void AddMoney(int amount)
        {
            Money += amount;
            OnUpdate?.Invoke();
        }

        public void TakeMoney(int amount)
        {
            Money -= amount;
            OnUpdate?.Invoke();
        }

        public void TriggerOnUpdateEvent() => OnUpdate?.Invoke();
    }
}
