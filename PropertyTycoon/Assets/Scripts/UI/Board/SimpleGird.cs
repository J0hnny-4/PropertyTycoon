using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Board
{

    public class SimpleGird : MonoBehaviour
    {
        public GameObject goPlane;
        public GameObject gotojailPlane;
        public GameObject jailPlane;
        public GameObject freePlane;
        public Transform mainPlane;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            mainPlane = mainPlane.GetComponent<Transform>();
            var goTileScale = TileScale();
            var goTilePosition = TilePosition(goTileScale); 
            goPlane = Instantiate(goPlane,new Vector3(-goTilePosition,0.1f,-goTilePosition) , Quaternion.identity);
            jailPlane = Instantiate(jailPlane,new Vector3(-goTilePosition,0.1f,goTilePosition) , Quaternion.identity);   
            gotojailPlane = Instantiate(gotojailPlane,new Vector3(goTilePosition,0.1f,-goTilePosition) , Quaternion.identity);
            freePlane = Instantiate(freePlane,new Vector3(goTilePosition,0.1f,goTilePosition) , Quaternion.identity);
            goPlane.transform.localScale = new Vector3(goTileScale, 0.01f, goTileScale);
            jailPlane.transform.localScale = new Vector3(goTileScale, 0.01f, goTileScale);
            gotojailPlane.transform.localScale = new Vector3(goTileScale, 0.01f, goTileScale);
            freePlane.transform.localScale = new Vector3(goTileScale, 0.01f, goTileScale);
            goPlane.transform.rotation = Quaternion.Euler(0,270,0); 
            jailPlane.transform.rotation = Quaternion.Euler(0, 270, 0);
            gotojailPlane.transform.rotation = Quaternion.Euler(0, 180, 0);
            freePlane.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private float TileScale()
        {
            var test = mainPlane.transform.localScale;
            var scale = test.x/5;
            return scale;
        }

        private float TilePosition(float goTileScale)
        {
            var test = mainPlane.transform.localScale;
            print(test.x * ((test.x/goTileScale)-1));
            return test.x * ((test.x / goTileScale)-1);
        }
    }
}