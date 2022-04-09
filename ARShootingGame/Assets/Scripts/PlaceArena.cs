using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceArena : MonoBehaviour
{
    // Component //
    private ARRaycastManager _arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    // This is the Placement Indicator //
    public GameObject GhostArena;

    // The Arena to replace GhostArena, and start the game.
    public GameObject Arena;

    public GameObject _btnPlaceArena;
    public GameObject JoySticks;

    // If Arena is placed, this set to true, GhostArena will be hidden from sight, so is Plane Scanner
    bool ArenaPlaced = false;

    private void Awake()
    {
        // Getting the Component
        _arRaycastManager = GetComponent<ARRaycastManager>();
        _btnPlaceArena = GameObject.Find("Btn_PlaceArena");
        JoySticks = GameObject.Find("JoySticks");

        // First Hide The Ghost Arena
        GhostArena.SetActive(false);
        _btnPlaceArena.SetActive(false);
        JoySticks.SetActive(false);
    }

    private void Update()
    {
        if (!ArenaPlaced)
        {
            UpdatePlacementIndicator();
            UpdatePlacementPose();
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            GhostArena.SetActive(true);
            GhostArena.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            _btnPlaceArena.SetActive(true);
        }
        else
        {
            GhostArena.SetActive(false);
            _btnPlaceArena.SetActive(false);
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
        Instantiate(Arena, GhostArena.transform.position, GhostArena.transform.rotation);
        GhostArena.SetActive(false);
        ARPlaneManager _arPlaneManager = FindObjectOfType<ARPlaneManager>();
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        _arPlaneManager.enabled = !_arPlaneManager.enabled;
        _btnPlaceArena.SetActive(false);
        JoySticks.SetActive(true);
    }
}
