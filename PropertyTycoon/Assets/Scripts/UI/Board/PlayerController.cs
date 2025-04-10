using UnityEngine;
using System;
using System.Collections.Generic;
using BackEnd;
using Data;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Board
{
    public class PlayerController : MonoBehaviour
    {
        private List<PlayerData> _players;
        private BoardTileController _tileController;
        private List<GameObject> _tileArray;
        private List<GameObject> _playersObjects;


        ///<summary
        ///When the game starts it initiates all player objects onto 
        ///the board and adds them to an array 
        ///</summary>
        public void Initialize()
        {
            _tileController = gameObject.GetComponent<BoardTileController>();
            _tileArray = _tileController.planes;
            _players = GameState.Players;
            _playersObjects = new List<GameObject>();

            for (int i = 0; i < _players.Count; i++)
            {
                var player = _players[i];
                player.OnGoToJail += TeleportPlayer;
                player.OnBankrupted += PlayerBankrupt;
                GameObject r1 = Instantiate(player.Token.sprite3D, new Vector3(1, 1, 1), Quaternion.identity);
                var spawnPoint = _tileArray[0].transform.Find("Spawn" + i);
                r1.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 1f, spawnPoint.position.z);
                _playersObjects.Add(r1);
            }
        }

        ///<summary>
        ///gits the players start position and end position and
        ///updates the players position to all the tiles inbetween
        ///the start and end.
        ///</summary> 
        public IEnumerator<WaitForSeconds> MovePlayer(int startPos, int EndPos)
        {
            var current_player = GameState.ActivePlayerIndex;
            var diff = Math.Abs(EndPos - startPos);

            for (int i = 1; i <= diff; i++)
            {
                if (startPos + i == 40)
                {
                    diff = EndPos;
                    i = 0;
                    startPos = 0;
                    GameObject tile = _tileArray[0];
                    _playersObjects[current_player].transform.position = tile.transform.position;
                    yield return new WaitForSeconds(Cons.PlayerWait);

                }
                else
                {
                    GameObject tile = _tileArray[startPos + i];
                    _playersObjects[current_player].transform.position = tile.transform.position;
                    yield return new WaitForSeconds(Cons.PlayerWait);

                }

            }
            GameState.Unpause();

        }
        ///<summary>
        ///Moves the player from any position on the board to their new 
        ///position on the board.
        ///</summary> 
        public void TeleportPlayer()
        {
            GameObject tile = _tileArray[GameState.ActivePlayer.Position];
            _playersObjects[GameState.ActivePlayerIndex].transform.position = tile.transform.position;
        }
        ///<summary>
        ///When a player is bankrupt their player object 
        ///is destroyed.
        ///</summary> 
        public void PlayerBankrupt(PlayerData play)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (play == _players[i])
                {
                    Destroy(_playersObjects[i]);
                }
            }

        }

    }
}

