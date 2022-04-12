using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using Unity.Netcode;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceArena : NetworkBehaviour
{
    // Component
    private ARRaycastManager _arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    ARPlaneManager _arPlaneManager;

    // This is the Placement Indicator
    public GameObject GhostArena;

    // The Arena to replace GhostArena, and start the game.
    public GameObject Arena;

    // UI Stuff
    public GameObject _btnPlaceArena;
    public GameObject JoySticks;

    // If Arena is placed, this set to true, GhostArena will be hidden from sight, so is Plane Scanner
    bool ArenaPlaced = false;

    // Networking Stuff
    private ulong clientId;
    public GameObject PlayerPrefab;
    bool Spawned = false;
    public bool GetRespond = false;

    private void Awake()
    {
        // Getting the Component
        _arRaycastManager = GetComponent<ARRaycastManager>();
        _btnPlaceArena = GameObject.Find("Btn_PlaceArena");
        JoySticks = GameObject.Find("JoySticks");
        _arPlaneManager = FindObjectOfType<ARPlaneManager>();

        // First Hide The Ghost Arena
        GhostArena.SetActive(false);
        _btnPlaceArena.GetComponent<Button>().interactable = false;
        JoySticks.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        clientId = NetworkManager.Singleton.LocalClientId;
    }

    private void Update()
    {
        if (GetRespond == true)
        {
            if (!ArenaPlaced)
            {
                if (IsHost)
                {
                    UpdatePlacementIndicator();
                    UpdatePlacementPose();
                }
                else if (!IsHost)
                {
                    _arPlaneManager.enabled = false ;
                    _btnPlaceArena.GetComponentInChildren<Text>().text = "Please Wait...";
                }
            }
            else if (ArenaPlaced && Spawned)
            {
                return;
            }
            else if (ArenaPlaced)
            {
                JoySticks.SetActive(true);
                _btnPlaceArena.SetActive(false);
                if (Spawned == false)
                InitiateGettingClientAndSpawning();
            }
        }

    }

    private void InitiateGettingClientAndSpawning()
    {
        if (IsHost)
        {
            Transform SpawnPoint = GameObject.Find("PlayerSpawn01").GetComponent<Transform>();
            GameObject HostPlayer = Instantiate(PlayerPrefab, SpawnPoint.position, Quaternion.identity);
            HostPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            Spawned = true;
        }
        else if (IsClient)
        {
            Transform SpawnPoint = GameObject.Find("PlayerSpawn02").GetComponent<Transform>();
            GameObject HostPlayer = Instantiate(PlayerPrefab, SpawnPoint.position, Quaternion.identity);
            HostPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            Spawned = true;
        }
        return;
       
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            GhostArena.SetActive(true);
            GhostArena.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            _btnPlaceArena.GetComponent<Button>().interactable = true;
        }
        else
        {
            GhostArena.SetActive(false);
            _btnPlaceArena.GetComponent<Button>().interactable = false;
        }
    }

    // This function is to Update the "Ghost Arena" position, inorder for player to place the real arena.
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        _arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = hits.Count > 0; 
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    // Placing the Arena only allow the Host to place
    // This require Networking.
    // After the Arena placed, only spawn both player
    // Or maybe, try to do : " Only the Arena placed, other player can join the lan-server etcs "
    public void PlacingArena()
    {
        ArenaPlaced = true;
        GameObject NewArena = Instantiate(Arena, GhostArena.transform.position, GhostArena.transform.rotation);
        NewArena.GetComponent<NetworkObject>().Spawn();
        GhostArena.SetActive(false);
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        _arPlaneManager.enabled = !_arPlaneManager.enabled;

        // After placing the Arena, begin getting ID, and spawning character on the Arena's Spawnpoint.
        
    }
}
