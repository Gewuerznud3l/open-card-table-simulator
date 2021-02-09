using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCamera : MonoBehaviour
{
    public GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(Camera, new Vector3(0, 25, -55), Quaternion.Euler(40, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("MainCamera").Length == 0) 
        {
            print("camera spawning");
            Instantiate(Camera, new Vector3(0, 25, -55), Quaternion.Euler(40, 0, 0)); 
        }
    }
}
