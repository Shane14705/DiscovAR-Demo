using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ViewerUIHandles : MonoBehaviour
{
    [SerializeField] private GameObject _modelOne;

    [SerializeField] private GameObject _modelTwo;

    [SerializeField] private GameObject _modelThree;

    [SerializeField] private ViewerNetworkManager _networkManager;

    public void OnModelOneSelected()
    {
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        PhotonNetwork.InstantiateRoomObject("MoleculeModel", Vector3.zero, Quaternion.identity, 0);
    }    
    
    public void OnModelTwoSelected()
    {
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        throw new NotImplementedException();
    }    
    
    public void OnModelThreeSelected()
    {
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        throw new NotImplementedException();
    }    

}
