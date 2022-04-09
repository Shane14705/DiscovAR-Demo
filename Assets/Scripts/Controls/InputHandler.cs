using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
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
    private bool _isRotating = false;
    
    //NOTE: IF YOU WANT TO CHANGE SENSITIVITY, IT MUST BE DONE ON EACH MODEL PREFAB AS EACH PREFAB HAS ITS OWN INPUT HANDLER
    public float rotSensitivity = 1f;

    private void OnEnable()
    {
        _inputComponent = this.GetComponent<PlayerInput>();
        _selectAction = _inputComponent.actions["Select"];
        _mainCam = Camera.main;
        //Bind events here
        _selectAction.performed += DisambiguateSelectionInput;
        _selectAction.canceled += OnInputReleased;
        _viewerManager = (ViewerNetworkManager)FindObjectOfType(typeof(ViewerNetworkManager));
        _viewerManager.CurrentModel = this.transform.parent.gameObject;
        
        FindObjectOfType<ViewerUIHandles>().animateCloseMenu();
        
        
        //TODO: REMEMBER THAT THIS LINE SHOULD ONLY RUN IF WE ARE IN 3D: AR VIEWER PLACES THE MODEL BASED ON OTHER CRITERIA
        
        //ALSO NOTE: ALL MOVEMENT/MANIPULATION SHOULD HAPPEN TO THE PARENT'S TRANSFORM (THIS ALLOWS FOR FIXING THE PIVOT POINTS TO THE CENTER OF THE MODEL
        this.transform.parent.DOMove(_mainCam.transform.position + _viewOffsetfromCam, 3);
    }

    private void OnInputReleased(InputAction.CallbackContext ctx)
    {
        //If we didnt rotate, then we should place an annotation when we lift our finger
        if (!_isRotating)
        {
            OnSelectAnnotate(ctx);
        }
        _isRotating = false;
    }

    //TODO: IDEA: INSTANTIATE ANNOTATIONS AS THEIR SPHERE COLLIDERS ON THEIR POSITION RELATIVE TO THE MESH ORIGIN, AND MAKE THEM ALL START HIDDEN. THEY EXIST FOR EVERYONE, BUT ARE ONLY SHOWN/HIDDEN FOR EACH INDIVIDUAL USER
    //DONT NETWORK THE SHOW/HIDE OF THE ANNOTATION, BUT DO NETWORK THE TEXT--just only allow the player that created it to actually edit the text
    private void OnSelectAnnotate(InputAction.CallbackContext ctx)
    {
        Vector2 clickPos = ((Pointer)(ctx.control.device)).position.ReadValue();
        //TODO: IMPLEMENT ANNOTATION PLACEMENT FUNCTION
        Debug.Log("Input handler is trying to create an annotation at " + clickPos);
        RaycastHit selectPos = new RaycastHit(); 
        //Check all layers
        if (Physics.Raycast(_mainCam.ScreenPointToRay(clickPos), out selectPos, 1000f, ~0))
        {
            //If we hit something, check if its a model or an annotation
            if (selectPos.collider.gameObject.CompareTag("Model"))
            {
                Debug.Log("Hit model collider, add an annotation!");
                //TODO: CREATE UI FOR CREATION OF ANNOTATION, AND THEN SAID UI MUST HOOK BACK INTO ANNOTATION CREATION ROUTINE -- ALSO POSSIBLY ALLOW FOR ADJUSTMENT OF ANNOTATION SIZE DEPENDING ON THE MODEL BEING ANNOTATED
                //For now, however:
                GameObject newAnnot = PhotonNetwork.Instantiate("Annotation", selectPos.point, Quaternion.identity);
                //Dont forget to parent it to the model so it moves when model is manipulated!
                newAnnot.transform.parent = _viewerManager.CurrentModel.transform;
            }
            else if (selectPos.collider.gameObject.CompareTag("Annotation"))
            {
                selectPos.collider.gameObject.GetComponent<AnnotationController>().ToggleAnnotation();
                Debug.Log("Hit an annotation collider, display the annotation!");
            }

        }

        else
        {
            Debug.Log("Didnt hit anything LOL");
        }
        
        
    }
    
    private void OnRotateActionPerformed(InputAction.CallbackContext ctx, Vector2 delta)
    {
        //With much help from this video it kind of makes sense to me now (I am very bad at vector math lol) https://youtu.be/kplusZYqBok
        
        Vector3 mouseDelta = new Vector3(delta.x, delta.y, 0) * rotSensitivity;
        //If dot is positive, then we are rightside up
        if (Vector3.Dot(this.transform.parent.up, Vector3.up) >= 0)
        {
            this.transform.parent.Rotate(this.transform.parent.up, -Vector3.Dot(mouseDelta, _mainCam.transform.right), Space.World);
        }
        
        //If dot is negative, then we are upside down and need to adjust our rotation angle accordingly
        else
        {
            this.transform.parent.Rotate(this.transform.parent.up, Vector3.Dot(mouseDelta, _mainCam.transform.right), Space.World);
        }
        
        //Finally, do up/down rotation (rotate around the right axis, depending on how different our mouse delta is from our up axis)?
        this.transform.parent.Rotate(_mainCam.transform.right, Vector3.Dot(mouseDelta, _mainCam.transform.up), Space.World);
    }

    //This function is called when the user lifts their finger/releases the mouse button
    private void DisambiguateSelectionInput(InputAction.CallbackContext ctx)
    {
        Vector2 delta = ((Pointer)ctx.control.device).delta.ReadValue();
        if (!_isRotating)
        {
            if (delta.magnitude > _inputDragDeadzone)
            {
                _isRotating = true; 
                OnRotateActionPerformed(ctx, delta);
            }
        }
        else
        {
            OnRotateActionPerformed(ctx, delta);
        }

    }

    
    //TODO: WE NEED TO UNLOAD ANNOTATIONS HERE SO THEY DO NOT KEEP SHOWING AFTER THE MODEL IS DESTROYED BY PHOTON
    private void OnDisable()
    {
        //Unbind events here to prevent memory leaks
        _selectAction.performed -= DisambiguateSelectionInput;
        _selectAction.canceled -= OnInputReleased;
        _viewerManager.CurrentModel = null;
    }
}
