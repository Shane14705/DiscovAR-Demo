using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public class ViewerUIHandles : MonoBehaviour
{

    [SerializeField] private ViewerNetworkManager _networkManager;

    [SerializeField] private RectTransform _menuPanel;

    public InputHandler currentHandler = null;

    [SerializeField]
    private GameObject _annotationDialogue;
    
    private bool menuOpen = true;

    public void OnModelOneSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }

        currentHandler = PhotonNetwork.InstantiateRoomObject("MoleculePivot", Vector3.zero, Quaternion.identity, 0).GetComponentInChildren<InputHandler>();
    }

    public void SubmitAnnotationClick()
    {
        _annotationDialogue.SetActive(false);

        if (currentHandler != null)
        {
            //TODO: FIGURE OUT WAY TO GET OUR PARAMETERS (ANNOTATION LOCATION, TITLE, AND DESCRIPTION FROM THE INPUT HANDLER TO OUR ANNOTATION DIALOGUE OR THIS FUNCTION, SO WE CAN CALL THE RPC)
            //currentHandler.photonView.RPC("InstantiateAnnotationRPC");
        }
        throw new NotImplementedException();
    }

    public void CancelAnnotationClick()
    {
        _annotationDialogue.SetActive(false);
        throw new NotImplementedException();
    }
    
    //TODO: This function should be triggered from button on annotation display UI, deletes the annotation which was being displayed. Will need a way to identify the annotation (either an ID or by title if title doesnt change) so that we can tell the input handler to find and delete the annotation from within the model's list of current annotations. Will also need to send an RPC for this most likely.
    public void DeleteAnnotation()
    {
        throw new NotImplementedException();
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
        
        currentHandler = PhotonNetwork.InstantiateRoomObject("SaturnPivot", Vector3.zero, Quaternion.identity, 0).GetComponentInChildren<InputHandler>();
    }    
    
    public void OnModelThreeSelected()
    {
        animateCloseMenu();
        if (_networkManager.CurrentModel != null)
        {
            PhotonNetwork.Destroy(_networkManager.CurrentModel);
        }
        
        currentHandler = PhotonNetwork.InstantiateRoomObject("BrainPivot", Vector3.zero, Quaternion.identity, 0).GetComponentInChildren<InputHandler>();
    }    
    
}
