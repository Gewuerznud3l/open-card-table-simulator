using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AttachCamera : NetworkBehaviour
{
    private GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) { return; }
        Camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        Camera.transform.parent = transform.GetChild(2).GetChild(0);
        Camera.transform.position = transform.GetChild(2).GetChild(0).position;
        Camera.transform.rotation = transform.GetChild(2).GetChild(0).rotation;
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/
}
