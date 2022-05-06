using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;


public class ViewerNetworkManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private GameObject _currentModel = null;

    [SerializeField] public bool _arScene = false;
    [SerializeField] private ARRaycastManager _arRaycastManager;
    [SerializeField] public Camera sceneCam;
    private List<ARRaycastHit> ar_hits = new List<ARRaycastHit>();
    public GameObject spawnPos;
    public bool sceneReady = false;

    public ARRaycastManager ARRaycastManager
    {
        get
        {
            if (_arScene) return _arRaycastManager;
            else
            {
                Debug.Log("ERROR: trying to get ARRaycastManager in non-ar scene!");
                return _arRaycastManager;
            }
        }
        set => _arRaycastManager = value;
    }

    public GameObject CurrentModel
    {
        get => _currentModel;
        set => _currentModel = value;
    }

    private void Start()
    {
        _photonView = PhotonView.Get(this);
        //We need to make sure that we are connected to the Photon Room, if we aren't then we need to handle it somehow...
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("ERROR: Made it into viewer scene without being connected to a room!");
        }

        if (_arScene && ARRaycastManager)
        {
            spawnPos = (GameObject) Resources.Load("SpawnPos", typeof(GameObject));
            spawnPos = GameObject.Instantiate(spawnPos, Vector3.zero, Quaternion.identity);
            
            Debug.Log("Tap a surface to pick your object spawn location!");
            return;
        }

        sceneReady = true;

    }

    private void Update()
    {
        if (!sceneReady)
        {
            //Allow the user to pick where they want the models to spawn at
            if (_arRaycastManager.Raycast(Touchscreen.current.primaryTouch.position.ReadValue(), ar_hits))
            {
                spawnPos.transform.position = ar_hits[0].pose.position;
                sceneReady = true;
            }
        }
    }

    public override void OnLeftRoom()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered room!");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
