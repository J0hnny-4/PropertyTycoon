
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using BackEnd;
using Data;

namespace UI.Board
{

    public class BoardTileController : MonoBehaviour
    {
        private List<SquareData> _data;
        public Transform mainPlane;
        public List<GameObject> _planes;  
        private string _Path;
        
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _data = GameState.Board; 
            _Path = "Assets/Resources/Images/GameMode/TileImages/";
            mainPlane = mainPlane.GetComponent<Transform>();
            _planes = CreateArray();
            print("DATA: " + _data.Count);
            

            for (int i = 0; i < _planes.Count; i += 10)
            { 
                CreateCornerTiles(i, _data[i].Name);
            }

            for (int i = 0; i < _planes.Count; i++)
            {
                if (i % 10 == 0 || i ==0 )
                {
                    continue;
                }
                else
                {
                    UpdateTiles(i);
                }
            }

            var playercontroller = gameObject.GetComponent<PlayerController>();
            playercontroller.Initialize();
        }

        private void UpdateTiles(int idx)
        {

            if (_data[idx] is PropertyData tile)
            {
                
                GameObject child = _planes[idx].transform.GetChild(0).gameObject;
                Renderer rend = child.GetComponent<Renderer>();
                rend.material.color = tile.Colour.UnityColour;
                child.SetActive(true);
            }
            

        }
        

        private void CreateCornerTiles(int idx, string cornerName)
        {
            print(idx + cornerName);
            GameObject corner = _planes[idx];
            Renderer rend = corner.GetComponent<Renderer>();
            rend.material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(_Path + cornerName + ".png");

        }

        private List<GameObject> CreateArray()
        {
            List<GameObject> tiles = new List<GameObject>();
            for (int i = 0; i < 40; i++)
            {
                tiles.Add(GameObject.Find(i.ToString()));
            }
            print(tiles.Count);
            return tiles;
        }
        
    }
}