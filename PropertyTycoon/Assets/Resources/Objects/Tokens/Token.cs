using UI.Screens.Menu.Components.PlayerPanel;
using UnityEngine;

namespace Objects.Tokens
{
    [CreateAssetMenu(fileName = "Token", menuName = "Scriptable Objects/Token")]
    public class Token : ScriptableObject
    {
        public Sprite sprite3D;
        public Texture2D icon;
        public PlayerPanel Owner; // todo: update this to be a Player object
        public bool HasOwner => Owner != null;
    }
}
