using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;

public class ObjectsSanti : MonoBehaviour
{
    private InputMaster controls;
    private GameObject playerL;
    private GameObject playerR;
    private GameObject cloneL;
    private GameObject cloneR;
    private GameObject player;
    private GameObject pascualita;
    private RaycastHit hit;
    private Rigidbody objectRightRb;
    private Rigidbody objectLeftRb;
    private Transform objectRightT;
    private Transform objectLeftT;
    [SerializeField]
    private Transform playerCamera;
    private float objectScaleDataR;
    private float objectOriginalScaleR;
    private float objectScaleDataL;
    private float objectOriginalScaleL;
    private bool grabObjR = false;
    private bool objectGrabbedR = true;
    private bool throwCheckR = true;
    private bool grabObjL = false;
    private bool objectGrabbedL = true;
    private bool throwCheckL = true;
    private bool activated = false;
    private Button activeButton = null;
    

    [SerializeField]
    private Transform objectRightCamera;
    [SerializeField]
    private Transform objectLeftCamera;
    [SerializeField]
    private Transform objectRightHand;
    [SerializeField]
    private Transform objectLeftHand;
    [SerializeField]
    private float rayLine;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private GameObject ObjectRightUI;

    [SerializeField]
    private GameObject DropRightUI;
    [SerializeField]
    private GameObject InteractUI;

    public bool puzzleCreated = false;
    public bool puzzleActive = false;
    public bool noteCreated = false;
    public string objectName;

    public void Awake()
    {
        controls = new InputMaster();
        playerL = GameObject.Find("LeftObject");
        playerR = GameObject.Find("RightObject");
        player = GameObject.Find("Santi");
        
        // playerCamera = player.transform.GetChild(0).GetComponent<Transform>();
    }

    public void Start()
    {
        pascualita = GameObject.Find("Pascualita");
        AIControl aicontrol = pascualita.GetComponent<AIControl>();
        aicontrol.santiActivation();
    }

    public void Update()
    {
        Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine);
        Activation();
        if(hit.transform != null)
        {
            PuzzleManager();
            NoteManager();
            Grab();
        }
        if (hit.transform != null && hit.transform.tag == "Object" && !grabObjR)
        {
            ObjectRightUI.SetActive(true);
        }
        else
        {
            ObjectRightUI.SetActive(false);
        }

        //Drop UI
        if (grabObjR && throwCheckR)
        {
            DropRightUI.SetActive(true);
        }
        else
        {
            DropRightUI.SetActive(false);
        }

        //Interact UI
        if (hit.transform != null && hit.transform.tag == "Puzzle")
        {
            InteractUI.SetActive(true);
        }
        else
        {
            InteractUI.SetActive(false);
        }
        if (puzzleActive)
        {
            InteractUI.SetActive(false);
        }
        Drop();
    }

    private void Activation()
    {
        if (hit.transform != null && hit.transform.tag == "Button")
        {
            Button button = hit.transform.GetComponent<Button>();
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.0f;

            if (button != null)
            {
                if (isInteractPressed)
                {
                    button.Activation(true);
                    activeButton = button;
                    activated = true;
                }
                else if (activeButton != null)
                {
                    activeButton.Activation(false);
                    activeButton = null;
                    activated = false;
                }
            }
        }
        else if (activeButton != null && activated)
        {
            activeButton.Activation(false);
            activated = false;
        }
    }

    public void PuzzleManager()
    {
        if(hit.transform.tag == "Puzzle")
        {
            Puzzle puzzle = hit.transform.GetComponent<Puzzle>();
            string objectName = hit.collider.gameObject.name;
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.2f;
            if (puzzle != null && isInteractPressed && !puzzleCreated && !puzzleActive)
            {
                puzzle.OpenPuzzle(false, false, objectName);
                puzzleCreated = true;
                puzzleActive = true;
            }
            else if(puzzle != null && isInteractPressed && puzzleCreated && !puzzleActive)
            {
                puzzle.OpenPuzzle(true, false, objectName);
                puzzleActive = true;
            }
            else if(puzzleCreated && puzzleActive)
            {
                bool isCancelPressed = controls.Player.Cancel.ReadValue<float>() > 0.2f;
                if(isCancelPressed && puzzleActive)
                {
                    puzzle.ClosePuzzle(true);
                    puzzleActive = false;
                }
            }
        }
    }

    public void NoteManager()
    {
        if(hit.transform.tag == "Note")
        {
            Note note = hit.transform.GetComponent<Note>();
            string objectName = hit.collider.gameObject.name;
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.2f;
            bool isCancelPressed = controls.Player.Cancel.ReadValue<float>() > 0.2f;
            if (note != null && isInteractPressed && !noteCreated)
            {
                note.OpenNote(false);
                noteCreated = true;
            }
            else if(isCancelPressed && noteCreated)
            {
                note.CloseNote(true);
                noteCreated = false;
            }
        }
    }

    private void Grab()
    {
        if(hit.transform.tag == "Object" || hit.transform.tag == "Bengal")
        {
            bool isRightPressed = controls.Player.RightItem.ReadValue<float>() > 0.1f;
            bool isLeftPressed = controls.Player.LeftItem.ReadValue<float>() > 0.1f;

            if(isRightPressed && !grabObjR && objectGrabbedR && hit.transform.tag == "Object")
            {
                StartCoroutine(RightGrab());
            }
            else if(isLeftPressed && !grabObjL && objectGrabbedL && hit.transform.tag == "Bengal")
            {
                StartCoroutine(LeftGrab());
            }
        }
    }
    

    private void Drop()
    {
        bool isRightPressed = controls.Player.RightThrow.ReadValue<float>() > 0.1f;
        bool isLeftPressed = controls.Player.LeftThrow.ReadValue<float>() > 0.1f;

        if(isRightPressed && grabObjR && throwCheckR)
        {
            StartCoroutine(RightDrop());
        }
        else if(isLeftPressed && grabObjL && throwCheckL)
        {
            StartCoroutine(LeftDrop());
        }
    }

    private IEnumerator LeftGrab()
    {
        objectGrabbedL = false;
        // hit.transform.position = objectLeftCamera.position;
        // hit.rigidbody.isKinematic = true;
        // hit.transform.parent = objectLeftCamera;
        // objectScaleDataL = hit.transform.GetComponent<ObjectsData>().ObjectScale;
        // hit.transform.localScale = new Vector3(objectScaleDataL, objectScaleDataL, objectScaleDataL);
        // hit.transform.localPosition = new Vector3(0, 0, 0);
        // hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        // objectOriginalScaleL = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;

        hit.transform.GetComponent<ObjectsData>().OnGrab(objectLeftHand);

        objectLeftRb = hit.rigidbody;
        objectLeftT = hit.transform;

        // LeftGrabTwo();
        yield return new WaitForSeconds(0.5f);
        grabObjL = true;
        objectGrabbedL = true;
    }

    private IEnumerator RightGrab()
    {
        objectGrabbedR = false;

        // hit.transform.position = objectRightCamera.position;
        // hit.rigidbody.isKinematic = true;
        // hit.transform.parent = objectRightCamera;

        // objectScaleDataR = hit.transform.GetComponent<ObjectsData>().ObjectScale;
        // hit.transform.localScale = new Vector3(objectScaleDataR, objectScaleDataR, objectScaleDataR);
        // hit.transform.localPosition = new Vector3(0, 0, 0);
        // hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        // objectOriginalScaleR = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        
        hit.transform.GetComponent<ObjectsData>().OnGrab(objectRightHand);
        
        objectRightRb = hit.rigidbody;
        objectRightT = hit.transform;
        ObjectRightUI.SetActive(false);
        objectName = hit.transform.GetComponent<ObjectsData>().ObjectName;
        
        // RightGrabTwo();
        yield return new WaitForSeconds(0.5f);
        grabObjR = true;
        objectGrabbedR = true;
    }

    private void LeftGrabTwo()
    {   
        int layerIgnoreRaycast = LayerMask.NameToLayer("PlayerSanti");

        // GameObject child = playerL.transform.GetChild(0).gameObject;
        // child.layer = layerIgnoreRaycast;
        // cloneL = (GameObject)Instantiate(child, objectLeftHand.position, Quaternion.identity);
        // cloneL.transform.parent = objectLeftHand;
        // layerIgnoreRaycast = LayerMask.NameToLayer("PlayerJose");
        // child.layer = layerIgnoreRaycast;

        GameObject grabbedObject = objectLeftT.gameObject;
        grabbedObject.layer = layerIgnoreRaycast;

        cloneL = (GameObject)Instantiate(grabbedObject, objectLeftCamera.position, Quaternion.identity);
        // remove photonview
        cloneL.GetComponent<PhotonView>().enabled = false;
        cloneL.GetComponent<PhotonTransformView>().enabled = false;
        cloneL.transform.parent = objectLeftCamera;

        objectScaleDataL = hit.transform.GetComponent<ObjectsData>().ObjectScale;
        cloneL.transform.localScale = new Vector3(objectScaleDataL, objectScaleDataL, objectScaleDataL);
        cloneL.transform.localPosition = new Vector3(0, 0, 0);
        cloneL.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        objectOriginalScaleL = cloneL.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
    }

    private void RightGrabTwo()
    {
        int layerIgnoreRaycast = LayerMask.NameToLayer("PlayerSanti");

        // GameObject child = playerR.transform.GetChild(0).gameObject;
        // child.layer = layerIgnoreRaycast;
        // cloneR = (GameObject) Instantiate(child, objectRightHand.position, Quaternion.identity);
        // cloneR.transform.parent = objectRightHand;
        // layerIgnoreRaycast = LayerMask.NameToLayer("PlayerJose");
        // child.layer = layerIgnoreRaycast;

        GameObject grabbedObject = objectRightT.gameObject;
        grabbedObject.layer = layerIgnoreRaycast;

        cloneR = (GameObject)Instantiate(grabbedObject, objectRightCamera.position, Quaternion.identity);
        cloneR.GetComponent<PhotonView>().enabled = false;
        cloneR.GetComponent<PhotonTransformView>().enabled = false;
        cloneR.transform.parent = objectRightCamera;

        objectScaleDataR = hit.transform.GetComponent<ObjectsData>().ObjectScale;
        cloneR.transform.localScale = new Vector3(objectScaleDataR, objectScaleDataR, objectScaleDataR);
        cloneR.transform.localPosition = new Vector3(0, 0, 0);
        cloneR.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        objectOriginalScaleR = cloneR.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        
    }

    

    private IEnumerator LeftDrop()
    {
        // GameObject child = playerL.transform.GetChild(0).gameObject;
        // child.layer = 0;
        // throwCheckL = false;
        // objectLeftT.transform.localScale = new Vector3(objectOriginalScaleL, objectOriginalScaleL, objectOriginalScaleL);
        // Vector3 camerDirection = playerCamera.transform.forward;
        // objectLeftT.transform.parent = null;
        // objectLeftRb.isKinematic = false;
        // objectLeftRb.AddForce(camerDirection * throwForce);
        // Destroy(cloneL);
        objectLeftT.GetComponent<ObjectsData>().OnRelease();
        yield return new WaitForSeconds(0.5f);
        grabObjL = false;
        throwCheckL = true;
    }

    private IEnumerator RightDrop()
    {
        // GameObject child = playerR.transform.GetChild(0).gameObject;
        // child.layer = 0;
        // throwCheckR = false;
        // objectRightT.transform.localScale = new Vector3(objectOriginalScaleR, objectOriginalScaleR, objectOriginalScaleR);
        // Vector3 camerDirection = playerCamera.transform.forward;
        // objectRightT.transform.parent = null;
        // objectRightRb.isKinematic = false;
        // objectRightRb.AddForce(camerDirection * 0);
        // Destroy(cloneR);
        objectRightT.GetComponent<ObjectsData>().OnRelease();
        yield return new WaitForSeconds(0.5f);
        grabObjR = false;
        throwCheckR = true;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}