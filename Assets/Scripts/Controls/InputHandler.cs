using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    private PlayerInput _inputComponent;
    private InputAction _selectAction;
    private InputAction _rotateAction;
    private Camera _mainCam;
    private ViewerNetworkManager _viewerManager;
    [SerializeField] private Vector3 _viewOffsetfromCam = Vector3.zero;
    
    private void OnEnable()
    {
        _inputComponent = this.GetComponent<PlayerInput>();
        _selectAction = _inputComponent.actions["Select"];
        _rotateAction = _inputComponent.actions["Rotate"];
        _mainCam = Camera.main;
        //Bind events here
        _selectAction.performed += OnSelectionButtonTapped;
        _rotateAction.performed += OnRotateActionPerformed;
        _viewerManager = (ViewerNetworkManager)FindObjectOfType(typeof(ViewerNetworkManager));
        _viewerManager.CurrentModel = this.gameObject;
        
        FindObjectOfType<ViewerUIHandles>().animateCloseMenu();
        
        
        //TODO: REMEMBER THAT THIS LINE SHOULD ONLY RUN IF WE ARE IN 3D: AR VIEWER PLACES THE MODEL BASED ON OTHER CRITERIA
        this.transform.DOMove(_mainCam.transform.position + _viewOffsetfromCam, 3);
    }

    private void OnRotateActionPerformed(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }

    //This function is called when the user lifts their finger/releases the mouse button
    private void OnSelectionButtonTapped(InputAction.CallbackContext ctx)
    {
        Debug.Log("Selection made!");
        // RaycastHit selectPos = new RaycastHit(); 
        // //Check all layers
        // Physics.Raycast(_mainCam.ScreenPointToRay(ctx.ReadValue<Vector2>()), 1000f, ~0);
        //
        // //TODO: Add logic for placing an annotation and what not
    }

    private void OnDisable()
    {
        //Unbind events here to prevent memory leaks
        _selectAction.performed -= OnSelectionButtonTapped;
        _rotateAction.performed -= OnRotateActionPerformed;
        _viewerManager.CurrentModel = null;
    }
}
