using UnityEngine;


public class MainCamera : MonoBehaviour
{
    
    private Camera _mainCamera;

    private MeshFilter _plane;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _plane = GameObject.Find("MainPlane").GetComponent<MeshFilter>();
        _mainCamera.transform.position = new Vector3(0, 250, 0);
        _mainCamera.transform.LookAt(_plane.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //hello 
    }
}
