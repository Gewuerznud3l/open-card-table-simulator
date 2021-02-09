using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CanManager : NetworkBehaviour
{
    public GameObject Card;
    private Camera cam;
    public List<int> cardIds;
    public List<Sprite> sprites;
    private List<string> buttons = new List<string> { "before", "after", "middle", "back" };
    private int deckId, selected;
    private GameObject can;
    private Transform before, after, back, cardSelection, cardPos;
    private bool afterReset;
    Ray lastRay;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) { return; }
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        selected = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }
        if (!afterReset) { afterReset = true; return; }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Can" && can == null &&
                    hit.collider.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color == Color.white)
                {
                    //print("clicked");
                    Vector3 raypos = hit.point;
                    raypos.y += 0.1f;
                    lastRay = new Ray(raypos, new Vector3(0, -1, 0));
                    can = hit.collider.gameObject;
                    cardSelection = can.transform.GetChild(1);
                    cardIds = can.transform.parent.GetComponent<CanBehaviour>().cardIds;
                    deckId = can.transform.parent.GetComponent<CanBehaviour>().deckId;
                    sprites = can.transform.parent.GetComponent<CanBehaviour>().sprites;
                    Vector3 pos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                    cardSelection.position = pos;
                    cardSelection.rotation = Quaternion.LookRotation(cam.gameObject.transform.forward);
                    cam.gameObject.transform.parent.parent.GetComponent<CameraMovement>().Reset();
                    cam.gameObject.transform.parent.parent.GetComponent<CameraMovement>().enabled = false;
                    GameObject.Find("Zoom").GetComponent<Image>().enabled = false;
                    /*cam.gameObject.transform.position = can.transform.GetChild(1).GetChild(0).position;
                    cam.gameObject.transform.rotation = can.transform.GetChild(1).GetChild(0).rotation;*/
                    cardPos = cardSelection.GetChild(3);
                    cardPos.GetComponent<Collider>().enabled = true;
                    before = cardSelection.GetChild(1);
                    before.GetComponent<Collider>().enabled = true;
                    after = cardSelection.GetChild(2);
                    after.GetComponent<Collider>().enabled = true;
                    back = cardSelection.GetChild(4);
                    back.GetComponent<Collider>().enabled = true;
                    back.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                    if (cardIds.Count > 0)
                    {
                        //CmdSpawnCard(lastRay, cardSelection.GetChild(3).position, cardSelection.GetChild(3).rotation, cardIds[sprites.Count - 1]);
                        cardPos.GetComponent<SpriteRenderer>().sprite = sprites[sprites.Count - 1];
                        cardPos.GetComponent<SpriteRenderer>().color = Color.white;
                        if (cardIds.Count > 1)
                        {
                            before.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[sprites.Count - 2];
                            before.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        selected = sprites.Count - 1;
                    }
                }
                switch (hit.collider.gameObject.name)
                {
                    case "before": { beforeClick(); break; }
                    case "after": { afterClick(); break; }
                    case "back": { backClick(); break; }
                    case "CardPos": { cardClick(); break; }
                }
                //print(hit.collider.gameObject.name);
            }
        }
        if (selected > -1)
        {
            //print(selected);
            if (can != null)
            { 
                if (cardSelection.GetChild(3).childCount > 0 && cardSelection.GetChild(3).GetChild(0).GetComponent<CardBehaviour>().faceup == false)
                {
                    //print("flipping because not faceup");
                    //print(can.transform.GetChild(4).GetChild(0).gameObject.name);
                    cardSelection.GetChild(3).GetChild(0).GetComponent<CardBehaviour>().Flip();
                }
            }
        }
    }

    public void GotCard()
    {
        CmdRemoveCard(lastRay, selected);
        Reset();
    }

    void Reset()
    {
        print("reset");
        selected = -1;
        after.GetComponent<Collider>().enabled = false;
        before.GetComponent<Collider>().enabled = false;
        back.GetComponent<Collider>().enabled = false;
        cardPos.GetComponent<Collider>().enabled = false;
        after.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
        before.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
        back.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
        cardPos.GetComponent<SpriteRenderer>().color = Color.clear;
        after.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        before.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        cardPos.GetComponent<SpriteRenderer>().sprite = null;
        can = null;
        afterReset = false;
        cam.gameObject.transform.parent.parent.GetComponent<CameraMovement>().enabled = true;
        GameObject.Find("Zoom").GetComponent<Image>().enabled = true;
    }

    void cardClick()
    {
        Vector3 groundPos = transform.GetChild(1).position;
        groundPos.y += 1;
        Ray groundRay = new Ray(groundPos, new Vector3(0, -1, 0));
        CmdSpawnCard(groundRay, cardIds[selected], deckId);
        CmdRemoveCard(lastRay, selected);
        Reset();
    }

    void beforeClick()
    {
        if (selected > 0)
        {
            //CmdDestroyCard(lastRay);
            after.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[selected];
            after.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            selected--;
            //CmdSpawnCard(lastRay, cardSelection.GetChild(3).position, cardSelection.GetChild(3).rotation, cardIds[selected]);
            cardPos.GetComponent<SpriteRenderer>().sprite = sprites[selected];
            cardPos.GetComponent<SpriteRenderer>().color = Color.white;
            if (selected > 0)
            {
                before.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[selected - 1];
                before.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                before.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
    }

    void afterClick()
    {
        if (selected < sprites.Count - 1)
        {
            //CmdDestroyCard(lastRay);
            before.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[selected];
            before.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            selected++;
            cardPos.GetComponent<SpriteRenderer>().sprite = sprites[selected];
            cardPos.GetComponent<SpriteRenderer>().color = Color.white;
            //CmdSpawnCard(lastRay, cardSelection.GetChild(3).position, cardSelection.GetChild(3).rotation, cardIds[selected]);
            if (selected < sprites.Count - 1)
            {
                after.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[selected + 1];
                after.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                after.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
    }

    void backClick()
    {
        //CmdDestroyCard(lastRay);
        Reset();
    }

    void SpawnCard(int cardId, int deckId, Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform parent = hit.collider.gameObject.transform;
            Vector3 pos = hit.point;
            pos.y += 0.1f;
            GameObject card = GameObject.Instantiate(Card, pos, parent.rotation);
            card.GetComponent<CardBehaviour>().cardId = cardId;
            card.GetComponent<CardBehaviour>().deckId = deckId;
            card.transform.parent = parent;
        }
    }

    [Command]
    void CmdSpawnCard(Ray ray, int cardId, int deckId)
    {
        SpawnCard(cardId, deckId, ray);
        RpcSpawnCard(ray, cardId, deckId);
    }

    [ClientRpc]
    void RpcSpawnCard(Ray ray, int cardId, int deckId)
    {
        if (isServer) { return; }
        SpawnCard(cardId, deckId, ray);
    }

    [Command]
    void CmdDestroyCard(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Can")
        {
            Destroy(hit.collider.transform.GetChild(1).GetChild(3).GetChild(0).gameObject);
            RpcDestroyCard(ray);
        }
    }

    [ClientRpc]
    void RpcDestroyCard(Ray ray)
    {
        if (isServer) { return; }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Destroy(hit.collider.transform.GetChild(1).GetChild(3).GetChild(0).gameObject);
        }
    }

    [Command]
    void CmdRemoveCard(Ray ray, int selected)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.transform.parent.GetComponent<CanBehaviour>().RemoveCard(selected);
            RpcRemoveCard(ray, selected);
        }
    }

    [ClientRpc]
    void RpcRemoveCard(Ray ray, int selected)
    {
        if (isServer) { return; }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.transform.parent.GetComponent<CanBehaviour>().RemoveCard(selected);
        }
    }
}
