
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
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
        private List<GameObject> _planes;
        private string _Path;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _data = GameState.Board; 
            _Path = "Assets/Resources/Images/GameMode/TileImages/";
            mainPlane = mainPlane.GetComponent<Transform>();
            _planes = CreateArray();

            for (int i = 0; i < _planes.Count; i += 10)
            {
                CreateCornerTiles(i, _data[i].Name);
            }

            for (int i = 0; i < _planes.Count; i++)
            {
                if (i % 10 == 0)
                {
                    continue;
                }
                else
                {
                    UpdateTiles(i);
                }
            }
            
        }

        private void UpdateTiles(int idx)
        {

            if (_data[idx] is PropertyData tile)
            {
                Renderer rend = _planes[idx].GetComponent<Renderer>();
                rend.material.color = tile.Colour;
            }
            

        }
        

        private void CreateCornerTiles(int idx, string cornerName)
        {
            GameObject corner = _planes[idx];
            Renderer rend = corner.GetComponent<Renderer>();
            rend.material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(_Path + cornerName + ".png");

        }

        private List<GameObject> CreateArray()
        {
            for (int i = 0; i < 40; i++)
            {
                _planes.Add(GameObject.Find(i.ToString()));
            }
            print(_planes.Count);
            return _planes;
        }
        
    }
}