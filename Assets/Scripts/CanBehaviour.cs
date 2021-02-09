using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CanBehaviour : NetworkBehaviour
{
    private GameObject Can;
    public List<int> cardIds;
    public List<Sprite> sprites;
    [SyncVar]
    public int deckId;
    public GameObject Card;
    Transform before, after;
    // Start is called before the first frame update
    void Start()
    {
        Can = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Can.transform.childCount > 2)
        {
            if (Can.transform.GetChild(2).GetComponent<CardBehaviour>().deckId == deckId)
            {
                Transform card = Can.transform.GetChild(2);
                cardIds.Add(card.gameObject.GetComponent<CardBehaviour>().cardId);
                sprites.Add(card.gameObject.GetComponent<CardBehaviour>().front);
                Can.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[sprites.Count - 1];
                Can.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                Destroy(card.gameObject);
            }
            else
            {
                Can.transform.GetChild(2).parent = null;
            }
        } 
    }

    public void SpawnCard(Vector3 pos, Quaternion rot, int cardId)
    {
        print("spawning card");
        if (pos == new Vector3(0, 0, 0))
        {
            pos = Can.transform.position;
            pos.y += 2;
        }
        GameObject card = GameObject.Instantiate(Card, pos, rot);
        card.GetComponent<CardBehaviour>().cardId = cardId;
        card.GetComponent<CardBehaviour>().deckId = deckId;
        card.transform.parent = Can.transform.GetChild(1).GetChild(3);
        card.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 100;
    }

    public void RemoveCard(int selected)
    {
        cardIds.RemoveAt(selected);
        sprites.RemoveAt(selected);
        if (sprites.Count > 0)
        {
            Can.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[sprites.Count - 1];
        }
        else
        {
           Can.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
           Can.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }
}
