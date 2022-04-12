using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Respawn : NetworkBehaviour
{
    public int Life = 10;
    CharacterController cc;
    MenuScripts Canvas;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Canvas = GameObject.Find("Canvas").GetComponent<MenuScripts>();
    }

    public void Respawning()
    {
        if (IsLocalPlayer)
        {
            if (Life > 1)
            {
                if (IsHost)
                {
                    cc.enabled = false;
                    Transform SpawnPoint = GameObject.Find("PlayerSpawn01").GetComponent<Transform>();
                    //GameObject HostPlayer = Instantiate(Player.gameObject, SpawnPoint.position, Quaternion.identity);
                    //HostPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(Player.clientId);
                    transform.position = SpawnPoint.position;
                    cc.enabled = true;
                }
                else if (IsClient)
                {
                    cc.enabled = false;
                    Transform SpawnPoint = GameObject.Find("PlayerSpawn02").GetComponent<Transform>();
                    //GameObject HostPlayer = Instantiate(Player.gameObject, SpawnPoint.position, Quaternion.identity);
                    //HostPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(Player.clientId);
                    transform.position = SpawnPoint.position;
                    cc.enabled = true;
                }
            }
            else if (Life == 0)
            {
                Canvas.Lost.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
