using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BackEnd;
using BackEnd.Squares;
using Data;
using UI.Game;
using System.Threading.Tasks;
using UI.Board;

public class GameRunner : MonoBehaviour
{
    private List<Player> _players;
    private List<Square> _board;
    private PlayerController _controller;
    private int _activePlayerIndex = -1;
    private bool GameOver => _players.Count(p => !p.IsBankrupt) <= 1;

    private async void Start()
    {
        // initialises variables
        _players = new List<Player>();
        _board = new List<Square>();
        _controller = FindFirstObjectByType<PlayerController>();

        foreach (var pd in GameState.Players)
        {
            if (pd.IsAi) _players.Add(new AiPlayer(pd));
            else _players.Add(new HumanPlayer(pd));
            pd.OnBankrupted += HandlePlayerBankruptcy;
        }

        foreach (var sd in GameState.Board)
        {
            switch (sd)
            {
                case UtilityData u:
                    _board.Add(new Utility(u));
                    break;
                case PropertyData p:
                    _board.Add(new Property(p));
                    break;
                case StationData s:
                    _board.Add(new Station(s));
                    break;
                case TaxData t:
                    _board.Add(new Tax(t));
                    break;
                default:
                    switch (sd.Name)
                    {
                        case "Go to jail":
                            _board.Add(new GoToJail(sd));
                            break;
                        case "Free Parking":
                            _board.Add(new FreeParking(sd));
                            break;
                        default:
                            _board.Add(new Square(sd));
                            break;
                    }
                    break;
            }
        }

        await GameLoop();

        Player winner = null;
        foreach (var player in _players.Where(player => !player.IsBankrupt)) { winner = player; }
        Debug.Log($"{winner.Name} has won!");
    }


    private static async Task PauseAndWait()
    {
        GameState.Pause();
        while (GameState.Paused)
        {
            await Task.Yield();
        }
    }

    private async Task GameLoop()
    {
        while (!GameOver)
        {
            // get next available player
            do
            {
                _activePlayerIndex = (_activePlayerIndex + 1) % _players.Count;
            } while (_players[_activePlayerIndex].IsBankrupt);
            GameState.ActivePlayerIndex = _activePlayerIndex;
            var player = _players[_activePlayerIndex];

            // handle player already in jail
            if (player.TurnsLeftInJail > 0)
            {
                await DialogBoxFactory.PlayerInJailDialogBox(player.Name, player.TurnsLeftInJail).AsTask();
                player.HandleJAil();
                continue;
            }

            // main loop
            do
            {
                player.RollDice();
                Debug.Log($"{player.Name} rolled {player.LastRoll}");
                await DialogBoxFactory.DiceDialogBox(player.Name, player.LastRoll).AsTask();
                
                if (player.DoublesRolled == Cons.DoublesToJail)
                {
                    await player.Data.GoToJail();
                    break;
                }

                var startPos = _players[_activePlayerIndex].Position;
                await player.Move();
                var endPos = _players[_activePlayerIndex].Position;
                Debug.Log(startPos + " " + endPos);
                StartCoroutine(_controller.MovePlayer(startPos, endPos));
                await PauseAndWait();

                await _board[player.Position].PlayerLands();

            } while (player.DoublesRolled > 0 && !player.IsBankrupt && player.TurnsLeftInJail == 0);

            if (player.IsBankrupt)
            {
                await DialogBoxFactory.BankruptcyDialogBox(player.Name).AsTask();
            }
            else
            {
                GameState.TriggerOnActionPhase();
                await PauseAndWait(); // game stops until end-turn button is pressed
            }
        }
    }

    /// <summary>
    /// Resets properties owned by the bankrupt player.
    /// </summary>
    /// <param name="player">Player going bankrupt.</param>
    private void HandlePlayerBankruptcy(PlayerData player)
    {
        foreach (var tileNo in player.Properties)
        {
            var property = (Ownable)_board[tileNo];
            property.Reset();
        }
    }
}
