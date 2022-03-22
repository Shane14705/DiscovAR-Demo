using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNetworkManager : MonoBehaviour
{
    [SerializeField] private InputField _enteredRoomName;
    private RoomOptions _roomOptions;

    private void OnEnable()
    {
        _roomOptions.MaxPlayers = 0;
        //Room will last up to a minute after everyone has left
        _roomOptions.EmptyRoomTtl = 60000;
    }

    public void TryJoinRoom()
    {
        if (_enteredRoomName.text != null)
        {
            PhotonNetwork.JoinOrCreateRoom(_enteredRoomName.text, _roomOptions, TypedLobby.Default);
            throw new NotImplementedException();
        }
    }
}
