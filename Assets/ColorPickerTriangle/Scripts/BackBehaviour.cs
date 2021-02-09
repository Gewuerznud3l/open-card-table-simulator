using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void OnMouseDown()
    {
        GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.parent.parent.parent.GetComponent<UpdatePlayers>().DestroyCP();
        GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.parent.parent.GetComponent<CameraMovement>().enabled = true;
    }
}
