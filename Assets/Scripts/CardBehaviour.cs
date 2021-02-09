using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mirror;

public class CardBehaviour : NetworkBehaviour
{
    [SyncVar(hook=nameof(SetCardId))]
    public int cardId;
    [SyncVar(hook=nameof(SetDeckId))]
    public int deckId;
    [SyncVar(hook=nameof(SetFlipped))]
    public bool faceup;
    private SpriteRenderer spriteRenderer;
    public Sprite front, back;
    private List<string> placeable = new List<string> { "CardPlaceable", "Card", "Spawner", "Can", "Deck", "Hand" };
    private List<string> attachable = new List<string> { "Hand", "Can" };
    private ParticleSystem particle;
    [SyncVar(hook=nameof(SetSideways))]
    private bool sideways;
    private IMG2Sprite i2s;
    public int handIndex;
    // Start is called before the first frame update
    void Start()
    {
        handIndex = -1;
        //print("front is null");
        i2s = new IMG2Sprite();
        //GameObject.Find("Table").GetComponent<LoadCards>().Decks[deckId].Remove(cardId);
        sideways = false;
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Collider>().enabled == false)
        {
            Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ParticleSystem.MainModule settings = particle.main;
                if (placeable.Contains(hit.collider.gameObject.tag))
                {
                    settings.startColor = new ParticleSystem.MinMaxGradient(Color.green);
                }
                else
                {
                    settings.startColor = new ParticleSystem.MinMaxGradient(Color.red);
                }
                if (hit.collider.gameObject.tag == "Card")
                {
                    spriteRenderer.sortingOrder = hit.collider.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
                else
                {
                    spriteRenderer.sortingOrder = 99;
                }
            }
        }
    }

    public void UpdatePosition(Vector3 pos, Quaternion rot)
    {
        /*transform.position = pos;
        transform.rotation = rot;*/
    }

    public void StartDrag()
    {
        handIndex = -1;
        if (transform.parent != null)
        {
            //print(transform.parent.name);
            //print(faceup);
            if (transform.parent.name == "Hand" && (faceup))
            {
                Flip();
            }
            if (transform.parent.name == "CardPos")
            {
                //print(transform.parent.parent.parent.gameObject.name);
                
            }
            transform.parent = null;
        }
        transform.rotation = Quaternion.identity;
        GetComponent<Collider>().enabled = false;
        particle.Play();
        spriteRenderer.sortingOrder = 99;
    }

    public void StopDrag()
    {
        print("stop drag");
        GetComponent<Collider>().enabled = true;
        float lasthit = -1;

        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (attachable.Contains(hit.collider.gameObject.tag))
            {
                //print("no parent");
                //print(hit.collider.gameObject.name);
                particle.Stop();
                transform.parent = hit.collider.gameObject.transform;
                return;
            }
            //print(hit.collider.gameObject.transform.parent.tag);
            if (hit.collider.gameObject.transform.parent != null && hit.collider.gameObject.transform.parent.name == "Hand")
            {
                print("card is attaching itself to hand");
                particle.Stop();
                transform.parent = hit.collider.gameObject.transform.parent.parent;
                handIndex = hit.collider.gameObject.transform.GetSiblingIndex() + 1;
                //print(transform.GetSiblingIndex());
                return;
            }
        }
        print("nix parent");
        List<Vector3> startPositions = new List<Vector3> { new Vector3(1.5f, -0.01f, 2.5f), new Vector3(-1.5f, -0.01f, 2.5f),
            new Vector3(-1.5f, -0.01f, -2.5f), new Vector3(1.5f, -0.01f, -2.5f), new Vector3(0, -0.01f, 2.5f),  new Vector3(0, -0.01f, -2.5f),
            new Vector3(1.5f, -0.01f, 0), new Vector3(-1.5f, -0.01f, 0) };
        Quaternion hitRot = Quaternion.identity;
        foreach (Vector3 start in startPositions)
        {
            ray = new Ray(transform.position + start, new Vector3(0, -1, 0));
            if (Physics.Raycast(ray, out hit))
            {
                //print(hit.collider.gameObject.tag);
                if (placeable.Contains(hit.collider.gameObject.tag) && lasthit < hit.point.y)
                {
                    lasthit = hit.point.y;
                    if (hit.collider.gameObject.tag != "Card")
                    {
                        hitRot = hit.collider.gameObject.transform.rotation;
                        transform.parent = hit.collider.gameObject.transform;
                    }
                    else
                    {
                        if (hit.collider.gameObject.transform.parent != null)
                        {
                            transform.parent = hit.collider.gameObject.transform.parent;
                            hitRot = hit.collider.gameObject.transform.parent.rotation;
                        }
                    }
                }
            }
        }
        if (lasthit != -1)
        {
            Vector3 pos = transform.position;
            pos.y = lasthit + 0.1f;
            transform.position = pos;
            transform.rotation = hitRot * Quaternion.Euler(0, sideways ? -90 : 0, 0);
            particle.Stop();
            spriteRenderer.sortingOrder = (int)(pos.y * 10);
        }
    }

    public void Rotate()
    {
        if (transform.parent == null || transform.parent.tag == "CardPlaceable")
        {
            sideways = !sideways;
            if (transform.parent != null)
            {
                Transform parent = transform.parent;
                transform.parent = null;
                transform.Rotate(0, sideways ? -90 : 90, 0);
                transform.parent = parent;
            }
            else
            {
                transform.Rotate(0, sideways ? -90 : 90, 0);
            }
            
        }
    }

    public void Flip()
    {
        faceup = !faceup;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = faceup ? front : back;
    }

    public void UpdateRotation(Quaternion rot)
    {
        //transform.rotation = rot * Quaternion.Euler(0, sideways ? -90 : 0, 0);
    }

    void SetCardId(int oldId, int newId)
    {
        print("setting card id: " + newId);
        i2s = new IMG2Sprite();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        string path = GameObject.Find("Table").GetComponent<LoadCards>().Cards[newId];
        front = i2s.LoadNewSprite(path);
        spriteRenderer.sprite = faceup ? front : back;
    }

    void SetDeckId(int oldId, int newId)
    {
        print("setting deck id: " + newId);
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        back = GameObject.Find("Table").GetComponent<LoadCards>().Backsides[newId];
        spriteRenderer.sprite = faceup ? front : back;
    }

    void SetFlipped(bool oldFlip, bool newFlip)
    {
        spriteRenderer.sprite = newFlip ? front : back;
    }

    void SetSideways(bool oldSideways, bool newSideways)
    {
        Transform parent = transform.parent;
        transform.parent = null;
        transform.Rotate(0, newSideways ? -90 : 90, 0);
        transform.parent = parent;
    }
}
