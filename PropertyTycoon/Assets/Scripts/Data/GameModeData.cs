using UnityEngine;

namespace Data
{
    /// <summary>
    /// Define available game modes.
    /// </summary>
    public enum GameMode
    {
        Classic,
        Abridged
    }
    
    /// <summary>
    /// Scriptable object to store data specific to game modes.
    /// </summary>
    [CreateAssetMenu(fileName = "GameModeData", menuName = "Game/Game Mode Data")]
    public class GameModeData : ScriptableObject
    {
        public GameMode gameMode;
        public Texture2D image;
        public string description;
    }
}