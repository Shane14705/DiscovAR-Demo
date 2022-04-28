using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class ViewerNetworkManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private GameObject _currentModel = null;

    [SerializeField] private bool _arScene = false;
    [SerializeField] private ARRaycastManager _arRaycastManager;

    
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

        if (_arScene)
        {
            
            Debug.Log("Tap a surface to pick your object spawn location!");
        }
        
    }
    
    public override void OnLeftRoom()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
