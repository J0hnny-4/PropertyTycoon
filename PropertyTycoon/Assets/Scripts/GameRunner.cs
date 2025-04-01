using System.Collections.Generic;
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
    async void Start()
    {
        // initialises variables
        _players = new List<Player>();
        _board = new List<Square>();
        _controller = GameObject.FindFirstObjectByType<PlayerController>();

        foreach (var pd in GameState.Players)
        {
            if (pd.IsAi) _players.Add(new AiPlayer(pd));
            else _players.Add(new HumanPlayer(pd));
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
                default:
                    _board.Add(new Square(sd));
                    break;
            }
        }

        await GameLoop();

        Debug.Log("GAME OVER");
    }

    private bool GameOver()
    {
        int playersLeft = 0;
        foreach (var p in _players)
        {
            if (p.Money > 0) playersLeft++;
        }

        return playersLeft <= 1;
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
        Debug.Log("start game loop");
        while (!GameOver())
        {
            // get next available player
            do
            {
                _activePlayerIndex = (_activePlayerIndex + 1) % _players.Count;
            } while (_players[_activePlayerIndex].Money < 0);
            GameState.ActivePlayerIndex = _activePlayerIndex;
            var player = _players[_activePlayerIndex];



            // handle player already in jail
            if (player.TurnsLeftInJail > 0)
            {
                await DialogBoxFactory.PlayerInJailDialogBox(player.Name, player.Money).AsTask();

                // todo: not sure what this is?
                // if (player.HandleJAil())
                // {
                //     player.Move();
                //     _board[player.Position].PlayerLands();
                //     continue;
                // }
            }

            // main loop
            do
            {
                player.RollDice();
                Debug.Log($"{player.Name} rolled {player.LastRoll}");
                await DialogBoxFactory.DiceDialogBox(player.Name, player.LastRoll).AsTask();

                var startPos = _players[_activePlayerIndex].Position;
                player.Move();
                var endPos = _players[_activePlayerIndex].Position;
                Debug.Log(startPos + " " + endPos);
                StartCoroutine(_controller.MovePlayer(startPos, endPos));
                await PauseAndWait();

                await _board[player.Position].PlayerLands();

            } while (player.DoublesRolled > 0);

            // game stops until end-turn button is pressed
            await PauseAndWait();
        }
    }
}
