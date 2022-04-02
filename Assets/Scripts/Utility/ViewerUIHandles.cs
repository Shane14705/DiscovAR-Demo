using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class ViewerUIHandles : MonoBehaviour
{

    [SerializeField] private ViewerNetworkManager _networkManager;

    [SerializeField] private RectTransform _menuPanel;
    

    private bool menuOpen = true;

    public void OnModelOneSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }

        PhotonNetwork.InstantiateRoomObject("MoleculePivot", Vector3.zero, Quaternion.identity, 0);
    }

    public void menuToggleClick()
    {
        if (menuOpen)
        {
            animateCloseMenu();
        }
        else
        {
            animateOpenMenu();
        }
    }

    public void animateCloseMenu()
    {
        menuOpen = false;
        _menuPanel.DOAnchorPosY(-338f, 2);
    }

    public void animateOpenMenu()
    {
        menuOpen = true;
        _menuPanel.DOAnchorPosY(-175f, 2);
    }

    public void OnModelTwoSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        
        PhotonNetwork.InstantiateRoomObject("SaturnPivot", Vector3.zero, Quaternion.identity, 0);
    }    
    
    public void OnModelThreeSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        
        PhotonNetwork.InstantiateRoomObject("BrainPivot", Vector3.zero, Quaternion.identity, 0);
    }    
    
}
