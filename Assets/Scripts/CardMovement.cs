using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardMovement : NetworkBehaviour
{
    private List<string> dragable;
    public GameObject draggedCard, colliderObject;
    public Camera cam;
    public bool dragging;
    // Start is called before the first frame update
    void Start()
    {
        dragable = new List<string> { "Table", "CardPlaceable", "Card", "Spawner", "Can", "Deck", "Hand" };
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        dragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            float dist = Vector3.Distance(hit.point, new Vector3(0, 0, 0));
            Vector3 camPos = cam.gameObject.transform.position;
            camPos.y = 0;
            float maxDist = Vector3.Distance(camPos, new Vector3(0, 0, 0));
            if (dist < maxDist)
            {
                CmdUpdatePosition(ray);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Card")
                {
                    dragging = true;
                    if (hit.collider.transform.parent != null && hit.collider.transform.parent.name == "CardPos")
                    {
                        //print("got card");
                        gameObject.GetComponent<CanManager>().GotCard();
                    }
                    CmdStartDrag(ray);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                dragging = false;
                CmdStopDrag();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            CmdFlip(ray);
        }
        if (Input.GetKeyDown("r"))
        {
            CmdRotate(ray);
        }
        if (dragging)
        {
            Quaternion playerRot = transform.GetChild(2).rotation;
            CmdRotateToPlayer(playerRot);
        }
    }

    [Command]
    void CmdStartDrag(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Card")
            {
                draggedCard = hit.collider.gameObject;
                draggedCard.GetComponent<CardBehaviour>().StartDrag();
                //print("start drag");
                RpcStartDrag(ray);
            }
        }
    }

    [ClientRpc]
    void RpcStartDrag(Ray ray)
    {
        if (isServer) { return; }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Card")
            {
                draggedCard = hit.collider.gameObject;
                draggedCard.GetComponent<CardBehaviour>().StartDrag();
                //print("start drag");
            }
        }
    }

    [Command]
    void CmdStopDrag()
    {
        if (draggedCard != null)
        {
            draggedCard.GetComponent<CardBehaviour>().StopDrag();
            draggedCard = null;
            //print("stop drag");
            RpcStopDrag();
        }
    }

    [ClientRpc]
    void RpcStopDrag()
    {
        if (isServer) { return; }
        if (draggedCard != null)
        {
            draggedCard.GetComponent<CardBehaviour>().StopDrag();
            draggedCard = null;
            //print("stop drag");
        }
    }

    [Command]
    void CmdUpdatePosition(Ray ray)
    {
        if (draggedCard != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                colliderObject = hit.collider.gameObject;
                //print(hit.collider.gameObject.tag);
                if (dragable.Contains(colliderObject.tag))
                {
                    Vector3 pos = hit.point;
                    pos.y += 2;
                    Quaternion rot = draggedCard.transform.rotation;
                    //print(draggedCard.name + " " + pos.ToString());
                    draggedCard.transform.position = pos;
                    //draggedCard.GetComponent<CardBehaviour>().UpdatePosition(pos, rot);
                    //RpcUpdatePosition(pos, rot);
                }
            }
        }
    }

    [ClientRpc]
    void RpcUpdatePosition(Vector3 pos, Quaternion rot)
    {
        if (isServer) { return; }
        if (draggedCard != null)
        {
            draggedCard.GetComponent<CardBehaviour>().UpdatePosition(pos, rot);
            //print("position update");
        }
    }

    [Command]
    void CmdRotate(Ray ray)
    {
        if (draggedCard != null && draggedCard.transform.parent == null)
        {
            draggedCard.GetComponent<CardBehaviour>().Rotate();
            print("rotate");
            //RpcRotate(ray);
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                colliderObject = hit.collider.gameObject;
                if (colliderObject.tag == "Card" && colliderObject.transform.parent.tag == "CardPlaceable")
                {
                    colliderObject.GetComponent<CardBehaviour>().Rotate();
                    print("rotate");
                    //RpcRotate(ray);
                }
                else
                {
                    print(colliderObject.transform.parent.tag);
                }
            }
            else
            {
                print("no hit");
            }
        }
    }

    [ClientRpc]
    void RpcRotate(Ray ray)
    {
        if (isServer) { return; }
        if (draggedCard != null)
        {
            draggedCard.GetComponent<CardBehaviour>().Rotate();
            //print("rotate");
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                colliderObject = hit.collider.gameObject;
                if (colliderObject.tag == "Card")
                {
                    colliderObject.GetComponent<CardBehaviour>().Rotate();
                    //print("rotate");
                }
            }
        }
    }

    [Command]
    void CmdFlip(Ray ray)
    {
        if (draggedCard != null && draggedCard.transform.parent == null)
        {
            draggedCard.GetComponent<CardBehaviour>().Flip();
            //print("flip");
            //RpcFlip(ray);
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                colliderObject = hit.collider.gameObject;
                if (colliderObject.tag == "Card" && colliderObject.transform.parent.tag == "CardPlaceable")
                {
                    colliderObject.GetComponent<CardBehaviour>().Flip();
                    //print("flip");
                    //RpcFlip(ray);
                }
            }
        }
    }

    [ClientRpc]
    void RpcFlip(Ray ray)
    {
        if (isServer) { return; }
        if (draggedCard != null)
        {
            draggedCard.GetComponent<CardBehaviour>().Flip();
            //print("flip");
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                colliderObject = hit.collider.gameObject;
                if (colliderObject.tag == "Card")
                {
                    colliderObject.GetComponent<CardBehaviour>().Flip();
                    //print("flip");
                }
            }
        }
    }

    [Command]
    void CmdRotateToPlayer(Quaternion rot)
    {
        if (draggedCard != null)
        {
            draggedCard.transform.rotation = rot;
            //draggedCard.GetComponent<CardBehaviour>().UpdateRotation(rot);
            //RpcRotateToPlayer(rot);
        }
    }

    [ClientRpc]
    void RpcRotateToPlayer(Quaternion rot)
    {
        if (draggedCard != null)
        {
            draggedCard.GetComponent<CardBehaviour>().UpdateRotation(rot);
        }
    }
}
