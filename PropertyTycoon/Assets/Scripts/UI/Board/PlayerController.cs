using UnityEngine;
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


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Initialize()
        {
            _tileController = gameObject.GetComponent<BoardTileController>();
            _tileArray = _tileController.planes;
            _players = GameState.Players;
            _playersObjects = new List<GameObject>();

            for (int i = 0; i < _players.Count; i++)
            {
                print("Player count" + _players.Count + " tile " + _tileArray.Count);
                var player = _players[i];
                GameObject r1 = Instantiate(player.Token.sprite3D, new Vector3(1, 1, 1), Quaternion.identity);
                var spawnPoint = _tileArray[0].transform.Find("Spawn" + i);
                r1.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 1f, spawnPoint.position.z);
                Debug.Log(r1);
                _playersObjects.Add(r1);
            }
        }

        public void MovePlayer(int startPos, int EndPos)
        {
            var current_player = GameState.ActivePlayerIndex;
            var diff = EndPos - startPos;

            for (int i = 1; i <= diff; i++)
            {
                GameObject tile = _tileArray[startPos + i];
                _playersObjects[current_player].transform.position = tile.transform.position;
                StartCoroutine(Coroutines());

            }

        }

        IEnumerator<WaitForSeconds> Coroutines()
        {
            yield return new WaitForSeconds(2);
        }
    }
}

