using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

//This should go on the spawned annotation prefab, itll contain the code for hiding/showing (creating) the annotation UI, and updating the annotation info
//Ex: onselected method to toggle ui when inputHandler detects that this annotation has been clicked
public class AnnotationController : MonoBehaviourPun
{
    [SerializeField]
    private string _annotation_title;

    [SerializeField]
    private string _annotation_description;

    private ViewerNetworkManager _viewerManager;
    
    [SerializeField] private GameObject _annotationUI;
    [SerializeField] private Text descriptionField;
    [SerializeField] private Text titleField;
    
    //Controls in which direction away from the annotation point will the UI be shown
    [SerializeField] private bool _up = true;
    [SerializeField] private bool _left = false;
    [SerializeField] private bool _right = true;

    public float UIdistanceFromAnnotation = 2;
    public string AnnotationTitle
    {
        get => _annotation_title;
        set
        {
            _annotation_title = value;
            titleField.text = _annotation_title;
        }
    }

    public string AnnotationDescription
    {
        get => _annotation_description;
        set
        {
            _annotation_description = value;
            descriptionField.text = _annotation_description;
           
        }
    }

    [SerializeField]
    private bool isShown = true;
    
    // Preparation
    void OnEnable()
    {
        _viewerManager = (ViewerNetworkManager)FindObjectOfType(typeof(ViewerNetworkManager));
        _annotationUI = this.transform.Find("Canvas").gameObject;
        _annotationUI.GetComponent<Canvas>().worldCamera = _viewerManager.sceneCam;
        _annotationUI.transform.localPosition =
            new Vector3(_left ? -1 : (1 * (_right ? 1 : 0)), (_up ? 1 : 0), 0) * UIdistanceFromAnnotation;
        descriptionField.text = _annotation_description;
        titleField.text = _annotation_title;
    }

    // Cleanup
    void OnDisable()
    {
        
    }

    public void ToggleAnnotation()
    {
        isShown = !isShown;
        if (isShown)
        {
            showAnnotationUI();
        }

        else
        {
            hideAnnotationUI();
        }
    }

    private void Update()
    {
        //NOTE THAT ANNOTATIONS SHOULD BE IN WORLD SPACE ONLY FOR AR VIEWING, AND HENCE ONLY BE ROTATED IN THE AR VIEWER. FOR 3D VIEWING, A SCREEN SPACE STRATEGY MAY BE BETTER.
        _annotationUI.transform.LookAt(_viewerManager.sceneCam.transform);
        
    }

    private void hideAnnotationUI()
    {
        _annotationUI.SetActive(false);
        Debug.Log("hiding UI");
    }

    private void showAnnotationUI()
    {
        //This should instantiate the UI 
        _annotationUI.SetActive(true);
        Debug.Log("Title: " + _annotation_title + ", Description: " + _annotation_description);
    }
}
