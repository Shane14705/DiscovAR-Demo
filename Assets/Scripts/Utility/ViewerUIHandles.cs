using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class ViewerUIHandles : MonoBehaviour
{
    [SerializeField] private GameObject _modelOne;

    [SerializeField] private GameObject _modelTwo;

    [SerializeField] private GameObject _modelThree;

    [SerializeField] private ViewerNetworkManager _networkManager;

    [SerializeField] private RectTransform _menuPanel;

    [SerializeField] private RectTransform _menuButton;

    private bool menuOpen = true;

    public void OnModelOneSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }

        PhotonNetwork.InstantiateRoomObject("MoleculeModel", Vector3.zero, Quaternion.identity, 0);
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

    private void animateCloseMenu()
    {
        menuOpen = false;
        _menuPanel.DOAnchorPosY(-338f, 2);
        //_menuButton.DOScaleY(-0.5f, 1);
    }

    private void animateOpenMenu()
    {
        menuOpen = true;
        //_menuButton.DOScaleY(0.5f, 1);
        _menuPanel.DOAnchorPosY(-175f, 2);
    }

    public void OnModelTwoSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        throw new NotImplementedException();
    }    
    
    public void OnModelThreeSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        throw new NotImplementedException();
    }    

}
