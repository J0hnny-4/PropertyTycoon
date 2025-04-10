
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
        public List<GameObject> planes;
        private string _path;
        private string _stationPath;
        private string _utilPath;
        private string _potPath;
        private string _knocksPath;
        private string _taxPath;


        ///<summary>
        /// Runs on start and adds all tile textures on 
        /// to the tiles 
        /// </summary> 
        void Start()
        {
            _data = GameState.Board;
            _path = "Images/GameMode/TileImages/";
            _stationPath = "Images/Icons/Trains";
            _utilPath = "Images/Icons/Util";
            _potPath = "Images/Icons/Pot";
            _knocksPath = "Images/Icons/Knocks";
            _taxPath = "Images/Icons/Tax";
            mainPlane = mainPlane.GetComponent<Transform>();
            planes = CreateArray();
            print("DATA: " + _data.Count);


            for (int i = 0; i < planes.Count; i += 10)
            {
                CreateCornerTiles(i, _data[i].Name);
            }

            for (int i = 0; i < planes.Count; i++)
            {
                if (i % 10 == 0 || i == 0)
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
        ///<summary>
        ///gets the index which it is passed and sets the childs
        ///tile to the colour which is passed by the parser 
        ///</summary> 
        private void UpdateTiles(int idx)
        {

            if (_data[idx] is PropertyData tile)
            {

                GameObject child = planes[idx].transform.GetChild(0).gameObject;
                Renderer rend = child.GetComponent<Renderer>();
                rend.material.color = tile.Colour.UnityColour;
                child.SetActive(true);
            }
            else if (_data[idx] is StationData)
            {
                GameObject station = planes[idx];
                Renderer rend = station.GetComponent<Renderer>();
                rend.material.mainTexture = Resources.Load<Texture>(_stationPath);
            }
            else if (_data[idx] is UtilityData)
            {
                GameObject utility = planes[idx];
                Renderer rend = utility.GetComponent<Renderer>();
                rend.material.mainTexture = Resources.Load<Texture>(_utilPath);
            }
            else if (_data[idx] is CardSquareData card)
            {
                if (card.Name == "Pot Luck")
                {
                    GameObject pot = planes[idx];
                    Renderer rend = pot.GetComponent<Renderer>();
                    rend.material.mainTexture = Resources.Load<Texture>(_potPath);
                }
                else if (card.Name == "Opportunity Knocks")
                {
                    GameObject knocks = planes[idx];
                    Renderer rend = knocks.GetComponent<Renderer>();
                    rend.material.mainTexture = Resources.Load<Texture>(_knocksPath);
                }
            }
            else if (_data[idx] is TaxData)
            {
                GameObject tax = planes[idx];
                Renderer rend = tax.GetComponent<Renderer>();
                rend.material.mainTexture = Resources.Load<Texture>(_taxPath);
            }
        }

        ///<summary>
        ///Gets the index passed to it and the name associated with it 
        ///and finds the texture based on the name 
        ///</summary> 
        private void CreateCornerTiles(int idx, string cornerName)
        {
            print(idx + cornerName);
            GameObject corner = planes[idx];
            Renderer rend = corner.GetComponent<Renderer>();
            rend.material.mainTexture = Resources.Load<Texture>(_path + cornerName);


        }

        ///<summary>
        ///Creates a array of the tiles on the board and retuns it 
        ///</summary>
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
