using System.Collections.Generic;
using UnityEngine;
using Data;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
        int x = 0;
        while (x < 40)
        {
            Debug.Log(Parser.TileCreator());
            x++;
        }
        
        Debug.Log("End");
        
    }
}
