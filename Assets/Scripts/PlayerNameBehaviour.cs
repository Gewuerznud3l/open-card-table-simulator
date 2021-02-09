using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameBehaviour : MonoBehaviour
{
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-cam.transform.position);
    }
}
