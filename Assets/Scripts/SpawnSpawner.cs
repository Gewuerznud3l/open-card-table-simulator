using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class SpawnSpawner : NetworkBehaviour
{
    public GameObject Spawner;
    private GameObject BtnStart;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) { return; }
        BtnStart = GameObject.Find("StartGame");
        if (!isServer)
        {
            BtnStart.SetActive(false);
        }
        else
        {
            BtnStart.SetActive(true);
            BtnStart.GetComponent<Button>().onClick.AddListener(SpawnDecks);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnDecks()
    {
        //print("spawning");
        BtnStart.SetActive(false);
        int deckCount = GameObject.Find("Table").GetComponent<LoadCards>().Stacks.Count;
        for (int i = 0; i < deckCount; i++)
        {
            Vector3 pos = new Vector3(0, 0.2f, 10);
            Quaternion rot = Quaternion.Euler(0, i * 360 / deckCount, 0);
            pos = rot * pos;
            //GameObject spawner = Instantiate(Spawner, pos, rot);
            //NetworkServer.Spawn(spawner);
            CmdSpawnDeck(i, pos, rot);
        }
    }

    void SpawnDeck(int deckId, Vector3 pos, Quaternion rot)
    {
        GameObject spawner = Instantiate(Spawner, pos, rot);
        spawner.GetComponent<SpawnerBehaviour>().deckId = deckId;
    }

    [Command]
    void CmdSpawnDeck(int deckId, Vector3 pos, Quaternion rot)
    {
        GameObject spawner = Instantiate(Spawner, pos, rot);
        NetworkServer.Spawn(spawner);
        spawner.GetComponent<SpawnerBehaviour>().deckId = deckId;
        //SpawnDeck(deckId, pos, rot);
        //RpcSpawnDeck(deckId, pos, rot);
    }

    [ClientRpc]
    void RpcSpawnDeck(int deckId, Vector3 pos, Quaternion rot)
    {
        if (!isServer) { SpawnDeck(deckId, pos, rot); }
    }
     
}
