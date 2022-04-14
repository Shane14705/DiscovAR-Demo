using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//This should go on the spawned annotation prefab, itll contain the code for hiding/showing (creating) the annotation UI, and updating the annotation info
//Ex: onselected method to toggle ui when inputHandler detects that this annotation has been clicked
public class AnnotationController : MonoBehaviourPun
{
    [SerializeField]
    private string _annotation_title;

    [SerializeField]
    private string _annotation_description;

    [SerializeField] private GameObject _annotationUI;
    
    
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
            //TODO: UPDATE THE TEXT COMPONENT IN THE ANNOTATION UI WHEN A NEW ANNOTATION TITLE IS SET
        }
    }

    public string AnnotationDescription
    {
        get => _annotation_description;
        set
        {
            _annotation_description = value;
            //TODO: UPDATE THE TEXT COMPONENT IN THE ANNOTATION UI WHEN A NEW ANNOTATION DESCRIPTION IS SET
        }
    }

    [SerializeField]
    private bool isShown = false;
    
    // Preparation
    void OnEnable()
    {
        this.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        _annotationUI = this.GetComponentInChildren<Canvas>().gameObject;
        _annotationUI.transform.localPosition =
            new Vector3(_left ? -1 : (1 * (_right ? 1 : 0)), (_up ? 1 : 0), 0) * UIdistanceFromAnnotation;
        
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
        if (isShown)
        {
            _annotationUI.transform.LookAt(Camera.main.transform);
        }
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
