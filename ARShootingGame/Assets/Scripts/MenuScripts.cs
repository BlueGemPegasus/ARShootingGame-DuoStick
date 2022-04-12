using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class MenuScripts : MonoBehaviour
{
    public GameObject menuPanel;
    public Text Input_Name;
    public Text Input_PlaceHolder;
    [SerializeField] string PlayerName;

    private void Start()
    {
        PlayerName = PlayerPrefs.GetString("PlayerName");
        if (PlayerName != null || PlayerName != "")
        {
            Input_Name.text = PlayerName;
        }

    }

    public void Host()
    {
        if (Input_Name != null)
        {
            PlayerPrefs.SetString("PlayerName", Input_Name.ToString());
            NetworkManager.Singleton.StartHost();
            menuPanel.SetActive(false);
        }
        else
        {
            Input_PlaceHolder.text = "Please input a name before you try to host a server!";
        } 
    }

    public void Join()
    {
        if (Input_Name != null)
        {
            PlayerPrefs.SetString("PlayerName", Input_Name.ToString());
            NetworkManager.Singleton.StartClient();
            menuPanel.SetActive(false);
        }
        else
        {
            Input_PlaceHolder.text = "Please input a name before you try to join a server!";
        }
    }


}
