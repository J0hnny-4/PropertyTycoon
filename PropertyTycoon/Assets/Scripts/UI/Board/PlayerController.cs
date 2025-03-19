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
        private string _path;
        private BoardTileController _tileController;
        private List<GameObject> _tileArray;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Initialize()
        {
            _tileController = gameObject.GetComponent<BoardTileController>();
            _path = "Assets/Resources/Tokens/3D models";
            _tileArray = _tileController._planes;
            _players = GameState.Players;
            
            for (int i = 0; i < _players.Count; i++)
            {
                print("Player count" + _players.Count + " tile " + _tileArray.Count);
                var player = _players[i];
                GameObject r1 = Instantiate(player.Token.sprite3D,new Vector3(1, 1, 1), Quaternion.identity);
                var spawnPoint = _tileArray[0].transform.Find("Spawn" + i);
                r1.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y+1f, spawnPoint.position.z );
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

