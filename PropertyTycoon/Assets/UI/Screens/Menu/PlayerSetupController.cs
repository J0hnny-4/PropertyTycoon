using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSetupController
{
    private readonly List<PlayerData> _players;
    private readonly List<string> _defaultNames;
    private readonly Token[] _allTokens;
    private readonly HashSet<Token> _usedTokens;
    private readonly int _minPlayers;
    private readonly int _maxPlayers;
    public event Action OnPlayersChanged;
    
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
    }

    /// <summary>
    /// Add a new PlayerData to the current list, then returns it (to be used by UI).
    /// </summary>
    /// <returns>Newly created player.</returns>
    /// <exception cref="InvalidOperationException">Thrown if maximum number of player has already been reached.
    /// </exception>
    public PlayerData AddPlayer()
    {
        if (!CanAddPlayer)
        {
            throw new InvalidOperationException("Cannot add new player, maximum reached.");
        }
        var token = GetNextAvailableToken();
        var randomIndex = Random.Range(0, _defaultNames.Count);
        var player = new PlayerData(_defaultNames[randomIndex], token);
        _defaultNames.RemoveAt(randomIndex);
        _players.Add(player);
        OnPlayersChanged?.Invoke();
        return player;
    }

    /// <summary>
    /// Remove PlayerData from the current list.
    /// </summary>
    /// <param name="player">Player to be removed.</param>
    /// <exception cref="InvalidOperationException">Thrown if currently at minimum required number of players.
    /// </exception>
    public void RemovePlayer(PlayerData player)
    {
        if (!CanRemovePlayer)
        {
            throw new InvalidOperationException("Cannot remove player, current at minimum required.");
        }
        _usedTokens.Remove(player.Token);
        _defaultNames.Add(player.Name);
        _players.Remove(player);
        OnPlayersChanged?.Invoke();
    }

    /// <summary>
    /// Gets next available token from the list.
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
    /// Gets the current list of players as a readonly list.
    /// </summary>
    /// <returns>The list of players.</returns>
    public IReadOnlyList<PlayerData> GetPlayers() => _players.AsReadOnly();
    
    /// <summary>
    /// Returns the current number of players.
    /// </summary>
    public int PlayersCount => _players.Count;
}
