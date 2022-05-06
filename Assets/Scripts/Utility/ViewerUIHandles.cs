using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewerUIHandles : MonoBehaviour
{

    [SerializeField] private ViewerNetworkManager _networkManager;

    [SerializeField] private RectTransform _menuPanel;

    public InputHandler currentHandler = null;

    [SerializeField] private InputField _titleInput;
    [SerializeField] private InputField _descriptionInput;
    
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
        //TODO: Clients other than host cannot make annotations, as they have no "currentHandler" since the currentHandler is set when the model object is instantiated on the network by the host. Only the client which created the model can annotate it. Correct behavior?
        if (currentHandler != null)
        {
            
            currentHandler.photonView.RPC("InstantiateAnnotationRPC", RpcTarget.AllBuffered,
                _annotationDialogue.GetComponent<PositionStorageComponent>().newAnnotLocation, _titleInput.text, _descriptionInput.text);
        }
        
    }

    public void CancelAnnotationClick()
    {
        _annotationDialogue.SetActive(false);
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

    public void OnExitClick()
    {
        _networkManager.leaveRoom();
        SceneManager.LoadScene("Starting Scene");
    }
}
