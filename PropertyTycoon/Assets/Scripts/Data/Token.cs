using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Token", menuName = "Scriptable Objects/Token")]
    public class Token : ScriptableObject
    {
        public GameObject sprite3D;
        public Texture2D icon;
        
    }
}