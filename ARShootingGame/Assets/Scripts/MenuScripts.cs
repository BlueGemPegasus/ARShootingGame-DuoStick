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

    public PlaceArena ArenaScript;

    public GameObject Lost;
    public GameObject Won;

    private void Awake()
    {
        Won = GameObject.Find("Panel_Won!");
        Lost = GameObject.Find("Panel_Lose!");
        Lost.SetActive(false);
        Won.SetActive(false);
    }
    private void Start()
    {
        ArenaScript = GameObject.Find("AR Session Origin").GetComponent<PlaceArena>();

        PlayerName = PlayerPrefs.GetString("PlayerName");
        if (PlayerName != null || PlayerName != "")
        {
            Input_Name.text = PlayerName;
        }

    }

    public void Host()
    {
        if (Input_Name.text != null || Input_Name.text != " ")
        {
            PlayerPrefs.SetString("PlayerName", Input_Name.ToString());
            NetworkManager.Singleton.StartHost();
            menuPanel.SetActive(false);
            ArenaScript.GetRespond = true;
        }
        else
        {
            Input_PlaceHolder.text = "Please input a name before you try to host a server!";
        } 
    }

    public void Join()
    {
        if (Input_Name.text != null || Input_Name.text != " " )
        {
            PlayerPrefs.SetString("PlayerName", Input_Name.ToString());
            NetworkManager.Singleton.StartClient();
            menuPanel.SetActive(false);
            ArenaScript.GetRespond = true;
        }
        else
        {
            Input_PlaceHolder.text = "Please input a name before you try to join a server!";
        }
    }


}
