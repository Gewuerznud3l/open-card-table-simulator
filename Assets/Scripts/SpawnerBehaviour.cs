using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnerBehaviour : NetworkBehaviour
{
    public GameObject Card;
    [SyncVar(hook=nameof(SetDeckId))]
    public int deckId = -1;
    //public bool set = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*if (deckId != -1 && !set)
        {
            
            set = true;
        }*/
    }

    void SetDeckId(int oldId, int newId)
    {
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = GameObject.Find("Table").GetComponent<LoadCards>().Backsides[newId];
        print("set new id: " + newId);
    }
}
