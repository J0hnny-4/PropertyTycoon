using UnityEngine;
using UnityEngine.UIElements;


namespace UI.Board
{
    public class Test : MonoBehaviour
    {
        public GameObject _tile;
        public GameObject _tileChild;

        void Start()
        {
            var rend = _tileChild.GetComponent<Renderer>();
            rend.material.color = Color.red;
        }

        void Update()
        {
        
        }
    
    }
}

