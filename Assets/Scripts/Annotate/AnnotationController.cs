using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//This should go on the spawned annotation prefab, itll contain the code for hiding/showing (creating) the annotation UI, and updating the annotation info
//Ex: onselected method to toggle ui when inputHandler detects that this annotation has been clicked
public class AnnotationController : MonoBehaviourPun
{
    public string annotation_title;

    public string annotation_description;

    [SerializeField]
    private bool isShown = false;
    
    // Preparation
    void OnEnable()
    {
        
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

    private void hideAnnotationUI()
    {
        Debug.Log("hiding UI");
    }

    private void showAnnotationUI()
    {
        //This should instantiate the UI 
        Debug.Log("Title: " + annotation_title + ", Description: " + annotation_description);
    }
}
