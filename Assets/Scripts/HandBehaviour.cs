using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehaviour : MonoBehaviour
{
    private Transform Hand;
    // Start is called before the first frame update
    void Start()
    {
        Hand = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 1)
        {
            print("added card to hand");
            Transform card = transform.GetChild(1);
            card.parent = Hand;
            int index = card.GetComponent<CardBehaviour>().handIndex;
            if (index != 0) 
            {
                print("index set: " + index);
                card.SetSiblingIndex(index);
            }
            int cardCount = Hand.childCount;
            for (int i = 0; i < cardCount; i++)
            {
                card = Hand.GetChild(i);
                Vector3 pos = new Vector3(0, 0, 0);
                //print(pos);
                pos.x = (float)i - (float)cardCount * 1 / 2 + 1 / 2;
                pos.z += -0.025f * ((float)i - ((float)cardCount / 2)) * ((float)i - ((float)cardCount / 2)) + 1.0125f;
                pos.y += ((float)i) / 10;
                card.localPosition = pos;
                card.rotation = Hand.rotation;
                card.Rotate(0, 2.5f * i - cardCount * 1.25f + 1.25f, 0);
                card.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = i + 100;
                if (GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.parent.parent.parent != transform.parent
                    && card.GetComponent<CardBehaviour>().faceup)
                {
                    card.GetComponent<CardBehaviour>().Flip();
                }
                if (GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.parent.parent.parent == transform.parent
                    && (!card.GetComponent<CardBehaviour>().faceup))
                {
                    card.GetComponent<CardBehaviour>().Flip();
                }
            }
        }
    }
}
