using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEditor;

public class ObjectsSanti : MonoBehaviour
{
    private InputMaster controls;
    private GameObject playerL;
    private GameObject playerR;
    private GameObject cloneL;
    private GameObject cloneR;
    private GameObject player;
    private RaycastHit hit;
    private Rigidbody objectRightRb;
    private Rigidbody objectLeftRb;
    private Transform objectRightT;
    private Transform objectLeftT;
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

    public bool puzzleCreated = false;
    public bool puzzleActive = false;
    public bool noteCreated = false;

    public void Awake()
    {
        controls = new InputMaster();
        playerL = GameObject.Find("LeftObject");
        playerR = GameObject.Find("RightObject");
        player = GameObject.Find("Santi");
        playerCamera = player.transform.GetChild(0).GetComponent<Transform>();
    }

    public void Update()
    {
        Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine);
        PuzzleManager();
        NoteManager();
        Grab();
        Drop();
    }

    public void PuzzleManager()
    {
        if(hit.transform != null && hit.transform.tag == "Puzzle")
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
        if(hit.transform != null && hit.transform.tag == "Note")
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

    public void Grab()
    {
        if(hit.transform != null && (hit.transform.tag == "Object" || hit.transform.tag == "Bengal"))
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
    

    public void Drop()
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

    public IEnumerator LeftGrab()
    {
        objectGrabbedL = false;
        hit.transform.position = objectLeftCamera.position;
        hit.rigidbody.isKinematic = true;
        hit.transform.parent = objectLeftCamera;
        objectScaleDataL = hit.transform.GetComponent<ObjectsData>().ObjectScale;
        hit.transform.localScale = new Vector3(objectScaleDataL, objectScaleDataL, objectScaleDataL);
        hit.transform.localPosition = new Vector3(0, 0, 0);
        hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        objectOriginalScaleL = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        objectLeftRb = hit.rigidbody;
        objectLeftT = hit.transform;

        LeftGrabTwo();
        yield return new WaitForSeconds(0.5f);
        grabObjL = true;
        objectGrabbedL = true;
    }

    public void LeftGrabTwo()
    {   
        int layerIgnoreRaycast = LayerMask.NameToLayer("PlayerSanti");

        GameObject child = playerL.transform.GetChild(0).gameObject;
        child.layer = layerIgnoreRaycast;
        cloneL = (GameObject)Instantiate(child, objectLeftHand.position, Quaternion.identity);
        cloneL.transform.parent = objectLeftHand;
        layerIgnoreRaycast = LayerMask.NameToLayer("PlayerJose");
        child.layer = layerIgnoreRaycast;
    }

    public IEnumerator RightGrab()
    {
        objectGrabbedR = false;
        hit.transform.position = objectRightCamera.position;
        hit.rigidbody.isKinematic = true;
        hit.transform.parent = objectRightCamera;
        objectScaleDataR = hit.transform.GetComponent<ObjectsData>().ObjectScale;
        hit.transform.localScale = new Vector3(objectScaleDataR, objectScaleDataR, objectScaleDataR);
        hit.transform.localPosition = new Vector3(0, 0, 0);
        hit.transform.localRotation = Quaternion.Euler(-25f, -60f, 45f);
        objectOriginalScaleR = hit.transform.GetComponent<ObjectsData>().ObjectOriginalScale;
        objectRightRb = hit.rigidbody;
        objectRightT = hit.transform;

        RightGrabTwo();
        yield return new WaitForSeconds(0.5f);
        grabObjR = true;
        objectGrabbedR = true;
    }

    public void RightGrabTwo()
    {
        int layerIgnoreRaycast = LayerMask.NameToLayer("PlayerSanti");

        GameObject child = playerR.transform.GetChild(0).gameObject;
        child.layer = layerIgnoreRaycast;
        cloneR = (GameObject) Instantiate(child, objectRightHand.position, Quaternion.identity);
        cloneR.transform.parent = objectRightHand;
        layerIgnoreRaycast = LayerMask.NameToLayer("PlayerJose");
        child.layer = layerIgnoreRaycast;
    }

    public IEnumerator LeftDrop()
    {
        GameObject child = playerL.transform.GetChild(0).gameObject;
        child.layer = 0;
        throwCheckL = false;
        objectLeftT.transform.localScale = new Vector3(objectOriginalScaleL, objectOriginalScaleL, objectOriginalScaleL);
        Vector3 camerDirection = playerCamera.transform.forward;
        objectLeftT.transform.parent = null;
        objectLeftRb.isKinematic = false;
        objectLeftRb.AddForce(camerDirection * throwForce);
        Destroy(cloneL);
        yield return new WaitForSeconds(0.5f);
        grabObjL = false;
        throwCheckL = true;
    }

    public IEnumerator RightDrop()
    {
        GameObject child = playerR.transform.GetChild(0).gameObject;
        child.layer = 0;
        throwCheckR = false;
        objectRightT.transform.localScale = new Vector3(objectOriginalScaleR, objectOriginalScaleR, objectOriginalScaleR);
        Vector3 camerDirection = playerCamera.transform.forward;
        objectRightT.transform.parent = null;
        objectRightRb.isKinematic = false;
        objectRightRb.AddForce(camerDirection * 0);
        Destroy(cloneR);
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