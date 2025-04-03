using System;
using System.Collections.Generic;
using Data;

namespace BackEnd
{
    /// <summary>
    /// The primary storage medium for 
    /// Stores the current state of the game, location of players, current properties etc.
    /// A singleton class, fields of the instance are accessible globally with static fields.
    /// </summary>
    public class GameState
    {
        private int _activePlayerIndex = 0;
        private List<PlayerData> _players = new();
        private List<SquareData> _board = new();
        private GameMode _gameMode;
        private int _freeParkingMoney = 0;
        public static event Action OnNewPlayerTurn;
        public static bool Paused { get; private set; } = false;

        /// <summary>
        /// Stores the single instance of GameState.
        /// This should autofill when any field is accessed.
        /// </summary>
        private static GameState Instance { get; set; } = new();

        public static PlayerData ActivePlayer => Instance._players[Instance._activePlayerIndex];

        public static int ActivePlayerIndex
        {
            get => Instance._activePlayerIndex;
            set
            {
                Instance._activePlayerIndex = value;
                OnNewPlayerTurn?.Invoke();
            }
        }

        public static int FreeParkingMoney { get => Instance._freeParkingMoney; set => Instance._freeParkingMoney += value; }

        public static GameMode GameMode { get => Instance._gameMode; set => Instance._gameMode = value; }

        public static List<PlayerData> Players { get => Instance._players; set => Instance._players = value; }
        
        

        /// <summary>
        /// Restest the free parking money to 0.
        /// Used buy the free parking square when a player lands on it.
        /// </summary>
        public static void FreeParkingReset() { Instance._freeParkingMoney = 0; }

        public static List<SquareData> Board { get { return Instance._board; } set { Instance._board = value; } }
        public static void AddSquare(SquareData square) { Instance._board.Add(square); }
        
        public static int BoardSize => Instance._board.Count;

        /// <summary>
        /// Resets the singleton instance to a new GameState object.
        /// </summary>
        public static void NewGame() { Instance = new GameState(); }
        
        public static void Pause() { Paused = true; }
        public static void Unpause() { Paused = false; }
    }
}