using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ObjectsSanti : MonoBehaviourPun
{
    private Animator santiAnimator;
    private InputMaster controls;
    private GameObject playerL;
    private GameObject playerR;
    private GameObject cloneL;
    private GameObject cloneR;
    private GameObject player;
    private GameObject pascualita;
    private GameObject nurse;
    private GameObject nina;
    private GameObject LightBox;
    private GameObject puertaPrinicipal; 
    private RaycastHit hit;
    private Rigidbody objectRightRb;
    private Rigidbody objectLeftRb;
    public Transform objectRightT;
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
    private Cure activeStation = null;

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

    public GameObject DropRightUI;
    [SerializeField]
    private GameObject InteractUI;
    [SerializeField]
    private GameObject HealingUI;

    //MasterKeys
    [SerializeField]
    private GameObject Key1UI;
    [SerializeField]
    private GameObject Key2UI;
    [SerializeField]
    private GameObject Key3UI;

    //ObjectsNurse
    [SerializeField]
    private GameObject Object1UI;
    [SerializeField]
    private GameObject Object2UI;
    [SerializeField]
    private GameObject Object3UI;

    [SerializeField]
    private GameObject Timer;

    private PhotonView PV;
    private TextMeshProUGUI textMeshProText;
    private bool LockPickCheck = false;
    public bool puzzleCreated = false;
    public bool puzzleActive = false;
    public bool noteCreated = false;
    public string objectNameString;



        // Arreglar

    public int keylevel = 1;

    

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
        PV = GetComponent<PhotonView>();
        pascualita = GameObject.Find("Pascualita");
        nurse = GameObject.Find("nurse");
        nina = GameObject.Find("Nina");
        LightBox = GameObject.Find("LightBox");
        AIControl aicontrolP = pascualita.GetComponent<AIControl>();
        NurseAI aicontrolN = nurse.GetComponent<NurseAI>();
        NinaAI aicontrolNi = nina.GetComponent<NinaAI>();
        aicontrolP.santiActivation();
        aicontrolN.SantiActivation();
        aicontrolNi.SantiActivation();
        puertaPrinicipal = GameObject.Find("PuertaPrincipal");
        santiAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine);
        Activation();
        if(hit.transform != null)
        {
            PuzzleManager();
            NoteManager();
            if (objectNameString == "KeyMaster1" || objectNameString == "KeyMaster2" || objectNameString == "KeyMaster3")
            {
                GameManager.Instance.Keys.Play();
                StartCoroutine(RightDrop());
                hit.transform.GetComponent<PhotonView>().RPC("MasterKeysChange", RpcTarget.All, hit.transform.name);
                objectNameString = "";
            }
            if (objectNameString == "Object1" || objectNameString == "Object2" || objectNameString == "Object3")
            {
                StartCoroutine(RightDrop());
                hit.transform.GetComponent<PhotonView>().RPC("ObjectsNurseChange", RpcTarget.All, hit.transform.name);
                objectNameString = "";
            }
            else
            {
                Grab();
            }
            
        }

        //Object UI
        if (hit.transform != null && hit.transform.tag == "Object" && !grabObjR)
        {
            ObjectRightUI.SetActive(true);
            DropRightUI.SetActive(false);
        }
        else
        {
            ObjectRightUI.SetActive(false);
        }

        //Drop UI
        if (grabObjR)
        {
            DropRightUI.SetActive(true);
            ObjectRightUI.SetActive(false);
        }
        else
        {
            DropRightUI.SetActive(false);
        }

        //Interact UI
        if (hit.transform != null && hit.transform.tag == "Puzzle")
        {
            InteractUI.SetActive(true);
            DropRightUI.SetActive(false);
        }
        else if (hit.transform != null && hit.transform.tag == "Button")
        {
            InteractUI.SetActive(true);
            DropRightUI.SetActive(false);
        }
        else if (hit.transform != null && hit.transform.tag == "Box")
        {
            InteractUI.SetActive(true);
            DropRightUI.SetActive(false);
        }
        else if (hit.transform != null && hit.transform.tag == "Health")
        {
            InteractUI.SetActive(true);
            DropRightUI.SetActive(false);
        }
        else if (hit.transform != null && hit.transform.tag == "FinalDoor")
        {
            InteractUI.SetActive(true);
            DropRightUI.SetActive(false);
        }
        else if (hit.transform != null && hit.transform.tag == "Note")
        {
            InteractUI.SetActive(true);
            DropRightUI.SetActive(false);
        }
        else
        {
            InteractUI.SetActive(false);
        }

        //MasterKeysUI
        if (GameManager.Instance.Key1)
        {
            Key1UI.SetActive(true);
        }
        if (GameManager.Instance.Key2)
        {
            Key2UI.SetActive(true);
        }
        if (GameManager.Instance.Key3)
        {
            Key3UI.SetActive(true);
        }
        //ObjectsNurseUI
        if (GameManager.Instance.Object1)
        {
            Object1UI.SetActive(true);
        }
        if (GameManager.Instance.Object2)
        {
            Object2UI.SetActive(true);
        }
        if (GameManager.Instance.Object3)
        {
            Object3UI.SetActive(true);
        }

        //Timer UI
        if (this.GetComponent<Injection>().isPlayerInjected)
        {
            Timer.SetActive(true);
            textMeshProText = Timer.GetComponent<TextMeshProUGUI>();
            textMeshProText.text = ((int)this.GetComponent<Injection>().downTime - (int)this.GetComponent<Injection>().currentTime).ToString();
        }
        else
        {
            Timer.SetActive(false);
            textMeshProText = Timer.GetComponent<TextMeshProUGUI>();
            textMeshProText.text = ((int)this.GetComponent<Injection>().currentTime).ToString();
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
        
        if (hit.transform != null && hit.transform.tag == "Box" && !GameManager.Instance.audioH)
        {
            Electricity box = hit.transform.GetComponent<Electricity>();
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.0f;
            
            if (isInteractPressed)
            {
                LightBox.GetComponent<PhotonView>().RPC("Activation", RpcTarget.All, true);
            }
        }

        if (hit.transform != null && hit.transform.tag == "Health")
        {
            Cure station = hit.transform.GetComponent<Cure>();
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.0f;

            if (station != null)
            {
                if (isInteractPressed)
                {
                    HealingUI.SetActive(true);
                    station.updateCure(true, this.gameObject);
                    activeStation = station;
                    activated = true;
                }
                else if (activeStation != null)
                {
                    HealingUI.SetActive(false);
                    station.updateCure(false, this.gameObject);
                    activeStation = null;
                    activated = false;
                }
            }
        }
        else if (activeStation != null && activated)
        {
            HealingUI.SetActive(false);
            activeStation.updateCure(false, this.gameObject);
            activated = false;
        }

        if (hit.transform != null && hit.transform.tag == "FinalDoor")
        {
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.1f;
            if (isInteractPressed && GameManager.Instance.Key1 && GameManager.Instance.Key2 && GameManager.Instance.Key3)
            {
                Debug.Log("3 KEYS");
                puertaPrinicipal.GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
                puertaPrinicipal.GetComponent<Door>().doorState = true;
                puertaPrinicipal.GetComponent<Door>().OpenDoor();
            }
        }
    }

    public void PuzzleManager()
    {
        if(hit.transform.tag == "Puzzle")
        {
            Puzzle puzzle = hit.transform.GetComponent<Puzzle>();
            string objectName = hit.collider.gameObject.name;
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.2f;
            //if (objectNameString != "Key" && puzzle.puzzle.name != null)
            //{
            //    if (puzzle.puzzle.name == "LockPick")
            //    {
            //        Debug.Log("You need a key");
            //        //UI You need a key
            //    }
            //}
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

    [PunRPC]
    void UpdateBengalThrowAnimation(bool isBengalThrow)
    {
        if (santiAnimator != null)
        {
            santiAnimator.SetBool("IsBengalThrow", isBengalThrow);
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
                PV.RPC("UpdateBengalThrowAnimation", RpcTarget.All, true);
                PV.RPC("UpdateBengalThrowAnimation", RpcTarget.All, false);
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
        objectNameString = hit.transform.GetComponent<ObjectsData>().ObjectName;
        
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



    public IEnumerator RightDrop()
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
        grabObjR = false;
        throwCheckR = true;
        yield return new WaitForSeconds(0.5f);
    }

    [PunRPC]
    public void SyncKeyLevel(int _keylevel)
    {
        keylevel = _keylevel;
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