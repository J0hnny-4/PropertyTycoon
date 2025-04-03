using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Squares;
using Data;

public class GameRunner : MonoBehaviour
{
    private List<Player> _players;
    private List<Square> _board;
    private int _activePlayerIndex = -1;
    void Start()
    {
        foreach(var pd in GameState.Players)
        {
            if(pd.IsAi) _players.Add(new AiPlayer(pd));
            else _players.Add(new HumanPlayer(pd));
        }

        //TODO add load from xml
        
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
        
        GameLoop();

        Debug.Log("GAME OVER");
    }

    private bool GameOver()
    {
        int playersLeft = 0;
        foreach (var p in _players)
        {
            if (p.Money < 0) playersLeft++;
        }

        return playersLeft <= 1;
    }

    //TODO this may need to be a coroutine?
    private void GameLoop()
    {
        while (!GameOver())
        {
            do
            {
                _activePlayerIndex = (_activePlayerIndex + 1) % _players.Count;
            } while (_players[_activePlayerIndex].Money < 0);

            if (_players[_activePlayerIndex].TurnsLeftInJail > 0)
            {
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
                _players[_activePlayerIndex].Move();
                _board[_players[_activePlayerIndex].Position].PlayerLands();
            } while (_players[_activePlayerIndex].DoublesRolled > 0);
        }
    }
}
