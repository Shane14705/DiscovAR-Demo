using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ViewerNetworkManager : MonoBehaviourPunCallbacks
{
    
    private void Start()
    {
        //We need to make sure that we are connected to the Photon Room, if we aren't then we need to handle it somehow...
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("ERROR: Made it into viewer scene without being connected to a room!");
        }
        throw new NotImplementedException();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
