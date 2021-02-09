using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button start = GameObject.Find("Play").GetComponent<Button>();
        start.onClick.AddListener(OnStartClick);
        GameObject name = GameObject.Find("NameField");
        name.GetComponent<InputField>().onValueChanged.AddListener(delegate { OnNameChanged(); });
        if (PlayerPrefs.GetString("name") != null)
        {
            name.GetComponent<InputField>().text = PlayerPrefs.GetString("name", "not yet set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStartClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    void OnNameChanged()
    {
        PlayerPrefs.SetString("name", GameObject.Find("NameField").GetComponent<InputField>().text);
        print("set: " + GameObject.Find("NameField").GetComponent<InputField>().text);
        PlayerPrefs.Save();
        GameObject.Find("Play").GetComponent<Button>().interactable = true;
    }
}
