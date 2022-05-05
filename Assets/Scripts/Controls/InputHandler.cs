using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviourPun
{
    private PlayerInput _inputComponent;
    private InputAction _selectAction;
    private Camera _mainCam;
    private ViewerNetworkManager _viewerManager;
    [SerializeField] private Vector3 _viewOffsetfromCam = Vector3.zero;
    [SerializeField] private float _inputDragDeadzone = 0f;
    private bool _isRotating = false;
    private bool _isMine;
    
    private GameObject _annotationDialogue;
    
    //Really should split some of this into another "per model" script, but I can do that later lol
    //this will let us keep track of this user's annotations so we can destroy them when the model is unloaded
    private List<GameObject> myAnnotations = new List<GameObject>();
    
    //NOTE: IF YOU WANT TO CHANGE SENSITIVITY, IT MUST BE DONE ON EACH MODEL PREFAB AS EACH PREFAB HAS ITS OWN INPUT HANDLER
    public float rotSensitivity = 1f;

    private void OnEnable()
    {
        _inputComponent = this.GetComponent<PlayerInput>();
        _selectAction = _inputComponent.actions["Select"];
        
        //Bind events here
        _selectAction.performed += DisambiguateSelectionInput;
        _selectAction.canceled += OnInputReleased;
        _viewerManager = (ViewerNetworkManager)FindObjectOfType(typeof(ViewerNetworkManager));
        _mainCam = _viewerManager.sceneCam;
        _viewerManager.CurrentModel = this.transform.parent.gameObject;
        _annotationDialogue = GameObject.Find("Canvas").transform.Find("AnnotationDialogue").gameObject;
        _isMine = this.photonView.IsMine;
        FindObjectOfType<ViewerUIHandles>().animateCloseMenu();


        //Check whether we should placed based on AR Spawn pos or in a certain predefined pos in 3D Viewer
        if (_viewerManager._arScene && _viewerManager.sceneReady)
        {
            this.transform.parent.position = _viewerManager.spawnPos.transform.position;
        }
        else
        {
            //ALSO NOTE: ALL MOVEMENT/MANIPULATION SHOULD HAPPEN TO THE PARENT'S TRANSFORM (THIS ALLOWS FOR FIXING THE PIVOT POINTS TO THE CENTER OF THE MODEL
            this.transform.parent.DOMove(_mainCam.transform.position + _viewOffsetfromCam, 3);
        }
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
    
    //DONT NETWORK THE SHOW/HIDE OF THE ANNOTATION, BUT DO NETWORK THE TEXT--just only allow the player that created it to actually edit the text
    private void OnSelectAnnotate(InputAction.CallbackContext ctx)
    {
        Vector2 clickPos = ((Pointer)(ctx.control.device)).position.ReadValue();
        Debug.Log("Input handler is trying to create an annotation at " + clickPos);
        RaycastHit selectPos = new RaycastHit(); 
        //Check model layer (layer 3)
        if (Physics.Raycast(_mainCam.ScreenPointToRay(clickPos), out selectPos, 1000f, 1 << 3))
        {
            //If we hit something, check if its a model or an annotation
            if (selectPos.collider.gameObject.CompareTag("Model") && _isMine)
            {
                //If we own the object, add an annotation to it!
                if (_annotationDialogue == null) Debug.Log("how");
                _annotationDialogue.SetActive(true);
                //Store local space position on networked storage component so we can spawn it in the correct world space pos on other clients
                _annotationDialogue.GetComponent<PositionStorageComponent>().newAnnotLocation = this.transform.parent.InverseTransformPoint(selectPos.point);
            }
            else if (selectPos.collider.gameObject.CompareTag("Annotation"))
            {
                //If we hit an annotation, toggle its view (later on we may want to give the option to edit it as well)
                selectPos.collider.gameObject.GetComponent<AnnotationController>().ToggleAnnotation();
            }

        }

        else
        {
            Debug.Log("Didnt hit anything LOL");
        }
        
        
    }

    
    [PunRPC]
    private GameObject InstantiateAnnotationRPC(Vector3 position, string title, string description)
    {
        GameObject newAnnot = Instantiate(Resources.Load<GameObject>("Annotation"), this.transform.parent.TransformPoint(position), Quaternion.identity);
        //newAnnot.transform.localPosition = position;   
        Debug.LogError("Position given to RPC: " + position.ToString());
        Debug.LogError("new local pos: " + newAnnot.transform.localPosition);
        Debug.LogError("new world pos: " + newAnnot.transform.position);
        //Dont forget to parent it to the model so it moves when model is manipulated!
        newAnnot.transform.parent = _viewerManager.CurrentModel.transform;
        AnnotationController controller = newAnnot.GetComponent<AnnotationController>();
        controller.AnnotationTitle = title;
        controller.AnnotationDescription = description;
        myAnnotations.Add(newAnnot);
        return newAnnot;
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

    
    private void OnDisable()
    {
        //Destroy annotations so they dont carry over to new model
        foreach (GameObject annot in myAnnotations)
        {
            Destroy(annot);
        } 
        
        //Unbind events here to prevent memory leaks
        _selectAction.performed -= DisambiguateSelectionInput;
        _selectAction.canceled -= OnInputReleased;
        _viewerManager.CurrentModel = null;
    }
}
