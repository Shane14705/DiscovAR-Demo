using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ViewerNetworkManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private GameObject _currentModel = null;

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
    }

    [PunRPC]
    public void LoadModel(int ModelID, PhotonMessageInfo info) 
    {
        
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
}
