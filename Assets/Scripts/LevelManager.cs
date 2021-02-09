using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LevelManager : NetworkBehaviour
{
    private Camera cam;
    [SyncVar(hook = nameof(SetLevel))]
    public int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) { return; }
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        SetLevel(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "LevelUp")
                {
                    CmdSetLevel(level + 1);
                }
                if (hit.collider.gameObject.name == "LevelDown")
                {
                    CmdSetLevel(level - 1);
                }
            }
        }
    }

    public void SetLevel(int oldLevel, int newLevel)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        string text = "Leaderboard: \n";
        for (int i = 0; i < players.Length; i++)
        {
            text += players[i].GetComponent<UpdatePlayers>().playerName + ": " + players[i].GetComponent<LevelManager>().level.ToString() + "\n";
            GameObject.Find("LevelCounter").GetComponent<Text>().text = text;
        }
    }

    [Command]
    void CmdSetLevel(int newLevel)
    {
        level = newLevel;
    }
}
