using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public override void OnStopClient()
    {
        base.OnStopClient();
        SceneManager.LoadScene("StartScene");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        SceneManager.LoadScene("StartScene");
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        SceneManager.LoadScene("StartScene");
    }
}
