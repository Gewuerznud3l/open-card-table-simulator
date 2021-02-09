using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnerManager : NetworkBehaviour
{
    public GameObject Card;
    private GameObject[] spawners;
    public List<List<int>> decks;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        decks = GameObject.Find("Table").GetComponent<LoadCards>().Decks;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }
        if (spawners != null && spawners.Length > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Deck")
                    {
                        GameObject deck = hit.collider.gameObject;
                        Vector3 groundPos = transform.GetChild(1).position;
                        groundPos.y += 1;
                        Ray groundRay = new Ray(groundPos, new Vector3(0, -1, 0));
                        int deckId = hit.collider.gameObject.transform.parent.GetComponent<SpawnerBehaviour>().deckId;
                        int index = Random.Range(0, decks[deckId].Count);
                        int cardId = decks[deckId][index];
                        CmdSpawnCard(cardId, deckId, groundRay);
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Deck")
                    {
                        GameObject deck = hit.collider.gameObject;
                        Vector3 groundPos = transform.GetChild(3).position;
                        groundPos.y += 1;
                        Ray groundRay = new Ray(groundPos, new Vector3(0, -1, 0));
                        int deckId = hit.collider.gameObject.transform.parent.GetComponent<SpawnerBehaviour>().deckId;
                        int index = Random.Range(0, decks[deckId].Count);
                        int cardId = decks[deckId][index];
                        CmdSpawnCard(cardId, deckId, groundRay);
                    }
                }
            }
        }
        else
        {
            spawners = GameObject.FindGameObjectsWithTag("Spawner");
            //print(spawners.Length);
        }
    }

    /*void SpawnCard(int cardId, int deckId, Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform parent = hit.collider.gameObject.transform;
            decks[deckId].Remove(cardId);
            Vector3 pos = hit.point;
            pos.y += 0.1f;
            GameObject card = GameObject.Instantiate(Card, pos, parent.rotation);
            card.GetComponent<CardBehaviour>().cardId = cardId;
            card.GetComponent<CardBehaviour>().deckId = deckId;
            card.transform.parent = parent;
        }
       
    }*/

    [Command]
    void CmdSpawnCard(int cardId, int deckId, Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform parent = hit.collider.gameObject.transform;
            decks[deckId].Remove(cardId);
            Vector3 pos = hit.point;
            pos.y += 0.1f;
            GameObject card = GameObject.Instantiate(Card, pos, parent.rotation);
            card.transform.parent = parent;
            NetworkServer.Spawn(card);
            card.GetComponent<CardBehaviour>().cardId = cardId;
            card.GetComponent<CardBehaviour>().deckId = deckId;
        }
        //SpawnCard(cardId, deckId, ray);
        //RpcSpawnCard(cardId, deckId, ray);
    }

    /*[ClientRpc]
    void RpcSpawnCard(int cardId, int deckId, Ray ray)
    {
        if (!isServer)
        {
            SpawnCard(cardId, deckId, ray);
        }
    }*/
}
