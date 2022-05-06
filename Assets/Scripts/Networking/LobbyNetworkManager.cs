using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _enteredRoomName;
    private bool ARSupported = true;

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Couldnt create room: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined room: " + PhotonNetwork.CurrentRoom);
        //Check for AR Compatibility and open relevant scene
        if (ARSupported)
        {
            SceneManager.LoadScene("Scenes/AR Viewer Scene");
        }
        else
        {
            SceneManager.LoadScene("Scenes/3D Viewer Scene");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room: " + message);
    }

    IEnumerator Start() {
        PhotonNetwork.ConnectUsingSettings();
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }
    
        if (ARSession.state == ARSessionState.Unsupported)
        {
            // Start some fallback experience for unsupported devices
            ARSupported = false;
        }
        else
        {
            // Start the AR session
            ARSupported = true;
        }
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
