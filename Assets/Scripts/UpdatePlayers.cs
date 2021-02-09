using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class UpdatePlayers : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetName))]
    public string playerName;
    [SyncVar(hook = nameof(SetColor))]
    public Color playerColor;
    public int playercount;
    private Camera cam;
    public GameObject ColorPickedPrefab;
    private ColorPickerTriangle CP;
    private bool isPaint = false;
    private GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(10).GetChild(0).GetComponent<Text>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
        if (!isLocalPlayer) { return; }
        playercount = 1;
        CmdSetName(PlayerPrefs.GetString("name"));
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; } //all players
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (isPaint && hit.collider.gameObject.name != "pickerCollider")
                {
                    DestroyCP();
                }
                if (hit.collider.gameObject.name == "ChangeColor")
                {
                    transform.GetChild(2).GetComponent<CameraMovement>().Reset();
                    transform.GetChild(2).GetComponent<CameraMovement>().enabled = false;
                    go = (GameObject)Instantiate(ColorPickedPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    Vector3 pos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 3f));
                    go.transform.position = pos;
                    go.transform.rotation = Quaternion.LookRotation( - cam.gameObject.transform.forward);
                    CP = go.GetComponent<ColorPickerTriangle>();
                    CP.SetNewColor(transform.GetChild(0).GetComponent<MeshRenderer>().material.color);
                    isPaint = true;
                }
            }
        }
        if (isPaint)
        {
            CmdSetColor(CP.TheColor);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DestroyCP();
            }
        }

        if (!isServer) { return; } //server only
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != playercount)
        {
            playercount = players.Length;
            int basepos = -50;
            if (playercount > 4)
            {
                basepos = (int)(-75 * playercount / 6.283f);
                print(basepos / -50);
            }
            for (int i = 0; i < playercount; i++)
            {
                Quaternion rot = Quaternion.Euler(0, i * 360 / playercount, 0);
                Vector3 pos = new Vector3(0, 0, basepos);
                pos = rot * pos;
                CmdUpdateTransform(players[i].transform, pos, rot, -(float)basepos / 50);
                GetComponent<LevelManager>().SetLevel(0, 1);
            }
        }
    }

    public void DestroyCP()
    {
        if (go != null)
        {
            Destroy(go);
            isPaint = false;
        }
    }

    void SetName(string oldName, string newName)
    {
        if (!isLocalPlayer) { transform.GetChild(10).GetChild(0).GetComponent<Text>().text = newName; }
        GetComponent<LevelManager>().SetLevel(0, 1);
    }

    void SetColor(Color oldColor, Color newColor)
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = newColor;
        transform.GetChild(1).GetComponent<MeshRenderer>().material.color = newColor;
        transform.GetChild(10).GetChild(0).GetComponent<Text>().color = newColor;
    }

    [Command]
    void CmdUpdateTransform(Transform player, Vector3 pos, Quaternion rot, float x)
    {
        player.position = pos;
        player.rotation = rot;
        player.GetChild(2).localScale = new Vector3(x, x, x);
        player.GetChild(2).position = new Vector3(0, 0, 0);
        RpcUpdateTransform(player, pos, rot, x);
    }

    [ClientRpc]
    void RpcUpdateTransform(Transform player, Vector3 pos, Quaternion rot, float x)
    {
        player.position = pos;
        player.rotation = rot;
        player.GetChild(2).localScale = new Vector3(x, x, x);
        player.GetChild(2).position = new Vector3(0, 0, 0);
    }

    [Command]
    void CmdSetName(string newName)
    {
        playerName = newName;
    }


    [Command]
    void CmdSetColor(Color newColor)
    {
        playerColor = newColor;
    }
}
