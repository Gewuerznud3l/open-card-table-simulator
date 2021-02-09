using UnityEngine;
using System.Collections;

public class example : MonoBehaviour {

    public GameObject ColorPickedPrefab;
    private ColorPickerTriangle CP;
    private bool isPaint = false;
    private GameObject go;
    private Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (isPaint)
        {
            mat.color = CP.TheColor;
        }
        if (Input.GetKeyDown("x"))
        {
            if (isPaint)
            {
                StopPaint();
            }
            else
            {
                StartPaint();
            }
        }
    }

    private void StartPaint()
    {
        Transform pos = transform.parent.GetChild(5);
        go = (GameObject)Instantiate(ColorPickedPrefab, pos.position, pos.rotation);
        go.transform.localScale = Vector3.one * 1.3f;
        go.transform.LookAt(Camera.main.transform);
        CP = go.GetComponent<ColorPickerTriangle>();
        CP.SetNewColor(mat.color);
        isPaint = true;
    }

    private void StopPaint()
    {
        Destroy(go);
        isPaint = false;
    }
}
