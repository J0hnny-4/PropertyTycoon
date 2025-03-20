using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using NUnit.Framework.Internal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Controllers
{
    public class PlayerSetupController
    {
        private readonly List<PlayerData> _players;
        private readonly List<string> _defaultNames;
        private readonly Token[] _allTokens;
        private readonly HashSet<Token> _usedTokens;
        private readonly int _minPlayers;
        private readonly int _maxPlayers;
        public event Action<PlayerData> OnPlayerRemoved;
        public event Action<PlayerData> OnPlayerAdded;
        
        
    
        public bool CanAddPlayer => _players.Count < _maxPlayers;
        public bool CanRemovePlayer => _players.Count > _minPlayers;
        public bool CanSwitchToken => _usedTokens.Count < _allTokens.Length;

        public PlayerSetupController(int minPlayers, int maxPlayers, List<string> defaultNames)
        {
            _players = new List<PlayerData>();
            _usedTokens = new HashSet<Token>();
            _allTokens = Resources.LoadAll<Token>("Tokens");
            _minPlayers = minPlayers;
            _maxPlayers = maxPlayers;
            _defaultNames = defaultNames;
            
            // error checking: can only be tested at runtime, as the list of names can only be accessed by the instance
            // present in the scene.
            if (defaultNames.Count < minPlayers)
            {
                throw new ArgumentException("List of names shorter than min number of players.");
            }
        }

        /// <summary>
        /// Initialise players to the minimum number allowed.
        /// </summary>
        public void InitialisePlayers()
        {
            for (var i = 0; i < _minPlayers; i++) { AddPlayer(); }
        }

        /// <summary>
        /// Add a new PlayerData to the current list, then returns it (to be used by UI).
        /// </summary>
        /// <returns>Newly created player.</returns>
        /// <exception cref="InvalidOperationException">Thrown if maximum number of player has already been reached.
        /// </exception>
        public void AddPlayer()
        {
            if (!CanAddPlayer) { throw new InvalidOperationException("Cannot add new player, maximum reached."); }
            
            var randomIndex = Random.Range(0, _defaultNames.Count);
            var player = new PlayerData(_defaultNames[randomIndex], GetNextAvailableToken());
            _defaultNames.RemoveAt(randomIndex);
            _players.Add(player);
            OnPlayerAdded?.Invoke(player);
        }

        /// <summary>
        /// Remove PlayerData from the current list.
        /// </summary>
        /// <param name="player">Player to be removed.</param>
        /// <exception cref="InvalidOperationException">Thrown if currently at minimum required number of players.
        /// </exception>
        public void RemovePlayer(PlayerData player)
        {
            if (!CanRemovePlayer) { throw new InvalidOperationException("Cannot remove player, minimum reached."); }
            
            _usedTokens.Remove(player.Token);
            _defaultNames.Add(player.Name);
            _players.Remove(player);
            OnPlayerRemoved?.Invoke(player);
        }

        /// <summary>
        /// Gets next available token, skipping already in use tokens.
        /// </summary>
        /// <returns>Next available token in the list</returns>
        /// <exception cref="InvalidOperationException">Thrown if no more tokens are available.</exception>
        public Token GetNextAvailableToken(Token prevToken = null, bool forward = true)
        {
            if (!CanSwitchToken) { throw new InvalidOperationException("No more tokens available."); }
            
            // get starting index
            var index = prevToken == null ? 0 : Array.IndexOf(_allTokens, prevToken);
            var increment = forward ? +1 : -1;
            Token newToken;
            
            // cycle through the list to find an unused token
            do {
                index = (index + increment + _allTokens.Length) % _allTokens.Length;
                newToken = _allTokens[index];
            } while (_usedTokens.Contains(newToken));
            
            // update used set
            _usedTokens.Remove(prevToken);
            _usedTokens.Add(newToken);
            return newToken;
        }
    
        /// <summary>
        /// Gets the current list.
        /// </summary>
        /// <returns>The list of players.</returns>
        public List<PlayerData> GetPlayers() => _players;
        
        /// <summary>
        /// Get the current list of tokens.
        /// </summary>
        /// <returns>The list of all tokens, used and available.</returns>
        public Token[] GetAllTokens() => _allTokens;

        public Token[] GetUsedTokens() => _usedTokens.ToArray();
    
        /// <summary>
        /// Returns the current number of players.
        /// </summary>
        public int PlayersCount => _players.Count;
        
    }
}
