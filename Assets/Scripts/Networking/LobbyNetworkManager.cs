using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _enteredRoomName;
    

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Couldnt create room: " + message);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Successfully joined room: " + PhotonNetwork.CurrentRoom);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected!");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room: " + message);
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    
    
    public void TryJoinRoom()
    {
        if (_enteredRoomName.text != null)
        {
            RoomOptions _roomOptions = new RoomOptions();
            _roomOptions.MaxPlayers = 0;
            //Room will last up to a minute after everyone has left
            _roomOptions.EmptyRoomTtl = 60000;
            PhotonNetwork.JoinOrCreateRoom(_enteredRoomName.text, _roomOptions, TypedLobby.Default);
        }
    }
}
