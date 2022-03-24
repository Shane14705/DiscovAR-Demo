using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    private PlayerInput _inputComponent;
    private InputAction _selectAction;
    private Camera _mainCam;
    private ViewerNetworkManager _viewerManager;
    
    private void OnEnable()
    {
        _inputComponent = this.GetComponent<PlayerInput>();
        _selectAction = _inputComponent.actions["Select"];
        _mainCam = Camera.main;
        //Bind events here
        _selectAction.canceled += OnSelectionButtonReleased;
        _viewerManager = (ViewerNetworkManager)FindObjectOfType(typeof(ViewerNetworkManager));
        _viewerManager.CurrentModel = this.gameObject;
    }

    //This function is called when the user lifts their finger/releases the mouse button
    private void OnSelectionButtonReleased(InputAction.CallbackContext ctx)
    {
        RaycastHit selectPos = new RaycastHit(); 
        //Check all layers
        Physics.Raycast(_mainCam.ScreenPointToRay(ctx.ReadValue<Vector2>()), 1000f, ~0);
        
        //TODO: Add logic for placing an annotation and what not
    }

    private void OnDisable()
    {
        //Unbind events here to prevent memory leaks
        _selectAction.canceled -= OnSelectionButtonReleased;

        _viewerManager.CurrentModel = null;
    }
}
