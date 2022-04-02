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
    private Camera _mainCam;
    private ViewerNetworkManager _viewerManager;
    [SerializeField] private Vector3 _viewOffsetfromCam = Vector3.zero;
    [SerializeField] private float _inputDragDeadzone = 0f;
    
    /*TODO: NOTE THAT WE MUST NO LONGER ALLOW ANNOTATIONS TO BE MADE ON THE GIVEN INPUT ONCE A ROTATION HAS BEGUN
     * OTHERWISE WHEN THE USER LIFTS THEIR FINGER AFTER STOPPING THE ROTATION, THEIR DELTA WOULD BE IN THE DEADZONE AGAIN AND AN ANNOTATION WOULD BE TRIGGERED
     */

    private void OnEnable()
    {
        _inputComponent = this.GetComponent<PlayerInput>();
        _selectAction = _inputComponent.actions["Select"];
        _mainCam = Camera.main;
        //Bind events here
        _selectAction.performed += OnSelectionButtonTapped;
        _viewerManager = (ViewerNetworkManager)FindObjectOfType(typeof(ViewerNetworkManager));
        _viewerManager.CurrentModel = this.gameObject;
        
        FindObjectOfType<ViewerUIHandles>().animateCloseMenu();
        
        
        //TODO: REMEMBER THAT THIS LINE SHOULD ONLY RUN IF WE ARE IN 3D: AR VIEWER PLACES THE MODEL BASED ON OTHER CRITERIA
        this.transform.DOMove(_mainCam.transform.position + _viewOffsetfromCam, 3);
    }

    //TODO: Need to figure out input action setup for this--perhaps we check the delta on tap, and if its below a certain amount we assume its a tap, otherwise its a drag
    //The above has the issue of not being able to wait until release to trigger the action, as drag should happen as it is performed while annotate should happen on release
    //Therefore, We need a way to disambiguate which action is being intended *as fast as possible*. Perhaps by getting rid of one possibility after a certain amount of time (like with the "tap duration" for selecting)
    private void OnRotateActionPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("rotating");
        //With much help from this video it kind of makes sense to me now (I am very bad at vector math lol) https://youtu.be/kplusZYqBok
        
        Vector3 mouseDelta = new Vector3(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y, 0);
        //If dot is positive, then we are rightside up
        if (Vector3.Dot(this.transform.up, Vector3.up) >= 0)
        {
            this.transform.Rotate(this.transform.up, -Vector3.Dot(mouseDelta, _mainCam.transform.right), Space.World);
        }
        
        //If dot is negative, then we are upside down and need to adjust our rotation angle accordingly
        else
        {
            this.transform.Rotate(this.transform.up, Vector3.Dot(mouseDelta, _mainCam.transform.right), Space.World);
        }
        
        //Finally, do up/down rotation (rotate around the right axis, depending on how different our mouse delta is from our up axis)?
        this.transform.Rotate(_mainCam.transform.right, Vector3.Dot(mouseDelta, _mainCam.transform.up), Space.World);
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
        _viewerManager.CurrentModel = null;
    }
}
