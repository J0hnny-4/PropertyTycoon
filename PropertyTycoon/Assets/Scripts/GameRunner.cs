using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnityEngine;
using BackEnd;
using BackEnd.Squares;
using Data;
using UI.Game;
using UnityEngine.Rendering.VirtualTexturing;

public class GameRunner : MonoBehaviour
{
    private List<Player> _players;
    private List<Square> _board;
    private int _activePlayerIndex = -1;
    async void Start()
    {
        // initialises variables
        _players = new List<Player>();
        _board = new List<Square>();
        
        foreach(var pd in GameState.Players)
        {
            if(pd.IsAi) _players.Add(new AiPlayer(pd));
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

    protected async Task GameLoop()
    {
        while (!GameOver())
        {
            do
            {
                _activePlayerIndex = (_activePlayerIndex + 1) % _players.Count;
            } while (_players[_activePlayerIndex].Money < 0);

            if (_players[_activePlayerIndex].TurnsLeftInJail > 0)
            {
                GameState.Pause();
                await DialogBoxFactory.PlayerInJailDialogBox(_players[_activePlayerIndex].Name, _players[_activePlayerIndex].Money).AsTask();
                GameState.Unpause();
                // todo: not sure what this is?
                while(GameState.Paused) await Task.Yield();
                if (_players[_activePlayerIndex].HandleJAil())
                {
                    
                    _players[_activePlayerIndex].Move();
                    _board[_players[_activePlayerIndex].Position].PlayerLands();
                    continue;
                }
            }
            
            do
            {
                _players[_activePlayerIndex].RollDice();
                Debug.Log($"{_players[_activePlayerIndex].Name} rolled {_players[_activePlayerIndex].LastRoll}");
                await DialogBoxFactory.DiceDialogBox(_players[_activePlayerIndex].Name, _players[_activePlayerIndex].LastRoll).AsTask();
                Debug.Log("done");
                // todo wait for player moving animation?
                // GameState.Pause();
                // while(GameState.Paused) await Task.Yield();
                _players[_activePlayerIndex].Move();
                _board[_players[_activePlayerIndex].Position].PlayerLands();
            } while (_players[_activePlayerIndex].DoublesRolled > 0);
            
            GameState.Pause();
            //TODO add end turn ui here, unpause there.
            GameState.Unpause();
            while(GameState.Paused) await Task.Yield();
        }
    }
}
