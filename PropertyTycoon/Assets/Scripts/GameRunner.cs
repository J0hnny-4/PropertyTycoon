using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BackEnd;
using BackEnd.Squares;
using Data;
using UI.Game;
using System.Threading.Tasks;
using UI.Board;
using UnityEditor;

public class GameRunner : MonoBehaviour
{
    private List<Player> _players;
    private List<Square> _board;
    private PlayerController _controller;
    private int _activePlayerIndex = -1;
    private bool _timeout = false;
    private void OnTimeUp() => _timeout = true;
    private bool OnePlayerLeft => _players.Count(p => !p.IsBankrupt) <= 1;

    private async void Start()
    {
        // initialises variables
        _players = new List<Player>();
        _board = new List<Square>();
        _controller = FindFirstObjectByType<PlayerController>();
        
        // Dictionary of decks for CardSquares
        var decks = CardGenerator.GenerateCards();

        // setup listener to ABRIDGED timer (if present)
        FindFirstObjectByType<AbridgeTimer>().OnTimeUp += OnTimeUp;

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
                case CardSquareData csd:
                    _board.Add(new CardSquare(csd, decks[csd.Name]));
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

        var winner = _players.OrderByDescending(p => p.Money).First();
        Debug.Log($"{winner.Name} has won!");
        GameState.TriggerGameOver(winner.Data);
    }


    private static async Task PauseAndWait()
    {
        GameState.Pause();
        while (GameState.Paused)
        {
            await Task.Yield();
        }
    }


    private bool GameOver()
    {
        // if next player's index is less than the current one, it means we have done a full loop and all players have
        // taken an equal amount of turns
        var nextPlayerIndex = GetNextPlayerIndex();
        return OnePlayerLeft || (_timeout && nextPlayerIndex < _activePlayerIndex);
    }

    private int GetNextPlayerIndex()
    {
        var nextPlayerIndex = _activePlayerIndex;
        do
        {
            nextPlayerIndex = (nextPlayerIndex + 1) % _players.Count;
        } while (_players[nextPlayerIndex].IsBankrupt);
        return nextPlayerIndex;
    }


    private async Task GameLoop()
    {
        while (!GameOver())
        {
            // get next available player
            _activePlayerIndex = GetNextPlayerIndex();
            GameState.ActivePlayerIndex = _activePlayerIndex;
            var player = _players[_activePlayerIndex];

            // handle player already in jail
            if (player.TurnsLeftInJail > 0)
            {
                if (player is HumanPlayer)
                {
                    await DialogBoxFactory.PlayerInJailDialogBox(player.Name, player.TurnsLeftInJail).AsTask();
                    player.HandleJail();
                    continue;
                }
                else
                {
                    await DialogBoxFactory.AIDialogBox("Ai in Jail", "Ai has " + player.TurnsLeftInJail + " left in Jail!").AsTask();
                    player.HandleJail();
                    continue;
                }
            }

            // main loop
            do
            {
                player.RollDice();
                Debug.Log($"{player.Name} rolled {player.LastRoll}");
                if (player is HumanPlayer)
                {
                    await DialogBoxFactory.DiceDialogBox(player.Name, player.LastRoll).AsTask();
                }
                else
                {
                    await DialogBoxFactory.AIDialogBox("AI Rolled", "Ai rolled: " + player.LastRoll).AsTask();
                }

                if (player.DoublesRolled == Cons.DoublesToJail)
                {
                    await DialogBoxFactory.AIDialogBox(
                        "Doubles rolled",
                        "You rolled too many doubles, you've been sent to jail.").AsTask();
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
                if (player is HumanPlayer)
                {
                    await DialogBoxFactory.BankruptcyDialogBox(player.Name).AsTask();
                }
                else
                {
                    await DialogBoxFactory.AIDialogBox(
                            "Bankruptcy",
                            $"{player.Name} has lost all of their money, they file for bankruptcy and leave the game!")
                            .AsTask();
                }
                    
            }
            else if (player is HumanPlayer)
            {
                GameState.TriggerOnActionPhase();
                await PauseAndWait();// game stops until end-turn button is pressed
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
