using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DiceManager : NetworkBehaviour
{
    private GameObject dice;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) { return; }
        dice = transform.GetChild(6).gameObject;
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == dice)
                {
                    int lastrand = -1;
                    for (int i = 0; i < Random.Range(10, 20); i++)
                    {
                        int rand = Random.Range(0, 6);
                        if (!(6 - rand == lastrand))
                        {
                            CmdAddRotation(ray, rand);
                            lastrand = rand;
                        }
                    }
                }
            }
        }
    }

    [Command]
    void CmdAddRotation(Ray ray, int rot)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.gameObject.GetComponent<DiceBehaviour>().Rotate(rot);
            RpcAddRotation(ray, rot);
        }
    }

    [ClientRpc]
    void RpcAddRotation(Ray ray, int rot)
    {
        if (isServer) { return; }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.gameObject.GetComponent<DiceBehaviour>().Rotate(rot);
        }
    }
}
