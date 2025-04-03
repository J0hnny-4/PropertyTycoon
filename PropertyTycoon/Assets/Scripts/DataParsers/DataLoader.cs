using BackEnd;
using UnityEngine;

namespace DataParsers
{
    public class DataLoader: MonoBehaviour
    {
        private void Awake()
        {
            GameState.Board = BoardParser.TileCreator();
        }
    }
}