using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Respawn : NetworkBehaviour
{
    public int Life = 10;
    PlayerController Player;
    Transform SpawnPoint01;
    Transform SpawnPoint02;

    CharacterController cc;

    private void Start()
    {
        Player = GetComponent<PlayerController>();
        cc = GetComponent<CharacterController>();
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
                Destroy(this.gameObject);
            }
        }
    }
}
