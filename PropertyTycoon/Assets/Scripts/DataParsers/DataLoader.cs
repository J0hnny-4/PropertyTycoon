using BackEnd;
using UnityEngine;

namespace DataParsers
{
    public class DataLoader: MonoBehaviour
    {
        private void Awake()
        {
            GameState.NewGame(); // ensures data from old games is erased
            GameState.Board = BoardParser.TileCreator();
        }
    }
}