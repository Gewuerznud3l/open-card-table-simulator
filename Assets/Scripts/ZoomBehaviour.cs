using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomBehaviour : MonoBehaviour
{
    private Image img;
    private Camera cam;
    private RectTransform rt, canvas;
    private List<string> zoomable = new List<string> { "Card", "Can" };
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        rt = GetComponent<RectTransform>();
        canvas = transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("IgnoreRaycast").Length > 0) { img.color = Color.clear; return; }
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (zoomable.Contains(hit.collider.gameObject.tag))
            {
                Vector2 pos = Input.mousePosition;
                pos.x += rt.rect.width / 2 + 10;
                pos.y += rt.rect.height / 2 + 10;
                if (pos.x + rt.rect.width / 2 > canvas.rect.width)
                {
                    pos.x = canvas.rect.width - rt.rect.width / 2;
                }
                if (pos.y + rt.rect.height / 2 > canvas.rect.height)
                {
                    pos.y = canvas.rect.height - rt.rect.height / 2;
                }
                transform.position = pos;
                GameObject card = hit.collider.gameObject;
                img.sprite = card.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                if (img.sprite != null)
                {
                    img.color = Color.white;
                }
                else
                {
                    img.color = Color.clear;
                }
            }
            else
            {
                img.color = Color.clear;
            }
        }
        else
        {
            img.color = Color.clear;
        }
    }
}
