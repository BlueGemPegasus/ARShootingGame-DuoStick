using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // For Debug Issue.
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceArena : MonoBehaviour
{
    /* The main inclusion component in order for AR to detect
     * Do not delete/change (Unless you know what are you doing)
     */
    private ARRaycastManager _arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    // This is the Placement Indicator //
    public GameObject GhostArena;

    

    public GameObject Arena;
    private GameObject TheSpawnedArena;
   
    private Vector2 touchposition;

    private PlayerInput playerInput;

    private void Awake()
    {
        // Getting the Component
        _arRaycastManager = GetComponent<ARRaycastManager>();
        playerInput = new PlayerInput();

        // First Hide The Ghost Arena
        GhostArena.SetActive(false);
    }

    private void Update()
    {
        UpdatePlacementIndicator();
        UpdatePlacementPose();
        
        
        //if (_arRaycastManager.Raycast(touchposition, hits, TrackableType.PlaneWithinPolygon))
        //{
        //    var hitPose = hits[0].pose;
            
        //    if (TheSpawnedArena == null)
        //    {
        //        TheSpawnedArena = Instantiate(Arena, hitPose.position, hitPose.rotation);
        //    }
        //    else
        //    {
        //        TheSpawnedArena.transform.position = hitPose.position;
        //
        //}
            
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            GhostArena.SetActive(true);
            GhostArena.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            GhostArena.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        _arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = hits.Count > 0;
        /*
         * Debug Area
         * There is a problem that the "Ghost Arena" wouldn't move with the camera
         *
         *
         * After Debug
         * Hits.Count == 0, but the Ghost Arena did instantly set Active as soon as the game start
         */
        Text Debugger = GameObject.Find("Debugger").GetComponent<Text>();
        if (Debugger != null)
        {
            Debugger.text = hits.Count.ToString();
        }
        
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
