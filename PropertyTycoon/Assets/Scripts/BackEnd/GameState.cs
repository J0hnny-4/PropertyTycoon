using System.Collections.Generic;
using Data;
using UnityEngine;

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
        private int _freeParkingMoney = 0;

        /// <summary>
        /// Stores the single instance of GameState.
        /// This should autofill when any field is accessed.
        /// </summary>
        private static GameState Instance { get; set; } = new();

        public static PlayerData ActivePlayer => Instance._players[Instance._activePlayerIndex];
        public static int ActivePlayerIndex => Instance._activePlayerIndex;

        public static int FreeParkingMoney { get => Instance._freeParkingMoney; set => Instance._freeParkingMoney += value; }

        /// <summary>
        /// Sets up the board and players for a new game.
        /// Currently, creates a placeholder board and two placeholder players.
        /// </summary>
        private GameState()
        {
            //TODO replace placholder board with actual squares

            for (var i = 0; i < 40; i++)
                if (i == 20) _board.Add(new SquareData("Free Parking"));
                else if (i == 30) _board.Add(new SquareData("Go To Jail"));
                else _board.Add(new SquareData("Square " + i));
            _players.Add(new PlayerData("Player 1", ScriptableObject.CreateInstance<Token>()));
            _players.Add(new PlayerData("Player 2", ScriptableObject.CreateInstance<Token>()));
        }

        /// <summary>
        /// Restest the free parking money to 0.
        /// Used buy the free parking square when a player lands on it.
        /// </summary>
        public static void FreeParkingReset() { Instance._freeParkingMoney = 0; }

        public static List<SquareData> Board => Instance._board;
        public static int BoardSize => Instance._board.Count;

        /// <summary>
        /// Adds a player to the game.
        /// Does not affect other players already added
        /// </summary>
        /// <param name="player">The player object to add to the lsit of players</param>
        public static void NewPlayer(PlayerData player) { Instance._players.Add(player); }

        /// <summary>
        /// Resets the singleton instance to a new GameState object.
        /// </summary>
        public static void NewGame() { Instance = new GameState(); }
    }
}