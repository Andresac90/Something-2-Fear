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
    private GameObject pascualita;
    private GameObject nurse;
    private GameObject nina;
    private GameObject LightBox;
    private GameObject ChangeObjects;
    private RaycastHit hit;
    public Transform objectRightT;

    [SerializeField]
    private Transform playerCamera;
    private bool grabObjR = false;
    private bool objectGrabbedR = true;
    private bool activated = false;
    private Button activeButton = null;
    private Cure activeStation = null;

    [SerializeField]
    private Transform objectRightCamera;
    [SerializeField]
    private Transform objectRightHand;
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
    private GameObject InjectionUI;

    [SerializeField]
    private GameObject Timer;

    private PhotonView PV;
    private TextMeshProUGUI textMeshProText;
    private bool isInjectionSpawned = false;
    public bool puzzleCreated = false;
    public bool puzzleActive = false;
    public bool noteCreated = false;
    public string objectNameString;
    public int keylevel = 1;

    

    public void Awake()
    {
        controls = new InputMaster();
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
        ChangeObjects = GameObject.Find("ChangeObjects");
        santiAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (!PV.IsMine) return;

        Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine);
        Activation();
        if(hit.transform != null)
        {
            PuzzleManager();
            NoteManager();
            Grab();
            switch (objectNameString)
            {
                case "KeyMaster1":
                    GameManager.Instance.Keys.Play();
                    StartCoroutine(RightDrop());
                    objectNameString = "";
                    hit.transform.GetComponent<PhotonView>().RPC("MasterKeysChange", RpcTarget.All, hit.transform.name);
                    
                    break;
                case "KeyMaster2":
                    GameManager.Instance.Keys.Play();
                    StartCoroutine(RightDrop());
                    objectNameString = "";
                    hit.transform.GetComponent<PhotonView>().RPC("MasterKeysChange", RpcTarget.All, hit.transform.name);
                    
                    break;
                case "KeyMaster3":
                    GameManager.Instance.Keys.Play();
                    StartCoroutine(RightDrop());
                    objectNameString = "";
                    hit.transform.GetComponent<PhotonView>().RPC("MasterKeysChange", RpcTarget.All, hit.transform.name);
                    
                    break;
                case "Object1":
                    StartCoroutine(RightDrop());
                    objectNameString = "";
                    hit.transform.GetComponent<PhotonView>().RPC("ObjectsNurseChange", RpcTarget.All, hit.transform.name);

                    break;
                case "Object2":
                    StartCoroutine(RightDrop());
                    objectNameString = "";
                    hit.transform.GetComponent<PhotonView>().RPC("ObjectsNurseChange", RpcTarget.All, hit.transform.name);
                    
                    break;
                case "Object3":
                    StartCoroutine(RightDrop());
                    objectNameString = "";
                    hit.transform.GetComponent<PhotonView>().RPC("ObjectsNurseChange", RpcTarget.All, hit.transform.name);
                    
                    break;
                case "Injection":
                    StartCoroutine(RightDrop());
                    objectNameString = "";
                    hit.transform.GetComponent<PhotonView>().RPC("InjectionChange", RpcTarget.All, hit.transform.name);
                    
                    break;
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
        else if (hit.transform != null && hit.transform.tag == "Table")
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
        if (GameManager.Instance.Object1 && !GameManager.Instance.Injection && !GameManager.Instance.InjectionSpawn)
        {
            Object1UI.SetActive(true);
        }
        else
        {
            Object1UI.SetActive(false);
        }
        if (GameManager.Instance.Object2 && !GameManager.Instance.Injection && !GameManager.Instance.InjectionSpawn)
        {
            Object2UI.SetActive(true);
        }
        else
        {
            Object2UI.SetActive(false);
        }
        if (GameManager.Instance.Object3 && !GameManager.Instance.Injection && !GameManager.Instance.InjectionSpawn)
        {
            Object3UI.SetActive(true);
        }
        else
        {
            Object3UI.SetActive(false);
        }

        //Injection UI
        if (hit.transform != null && hit.transform.tag == "Table" && GameManager.Instance.Object1 && GameManager.Instance.Object2 && GameManager.Instance.Object3)
        {
            bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0.0f;
            if (isInteractPressed && !isInjectionSpawned)
            {
                //Spawnear Injection
                hit.transform.GetComponent<PhotonView>().RPC("SpawnInjection", RpcTarget.All);
                isInjectionSpawned = true;
                GameManager.Instance.SpawnInjectionOnline();
            }
            
        }

        if (GameManager.Instance.Injection)
        {
            InjectionUI.SetActive(true);
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
        Video();
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
                ChangeObjects.GetComponent<PhotonView>().RPC("DeactivatePascualita", RpcTarget.All);
                GameManager.Instance.GetComponent<PhotonView>().RPC("EndingCutscene",RpcTarget.All);
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
            if (objectNameString != "Key" && puzzle.puzzle.name != null && puzzle.puzzle.name == "LockPick" && !puzzleCreated)
            {
                Debug.Log("You need a key");
                //UI You need a key
            }
            else if (puzzle != null && isInteractPressed && !puzzleCreated && !puzzleActive)
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
        if(hit.transform.tag == "Object")
        {
            bool isRightPressed = controls.Player.RightItem.ReadValue<float>() > 0.1f;
            if(isRightPressed && !grabObjR && objectGrabbedR && hit.transform.tag == "Object")
            {
                StartCoroutine(RightGrab());
            }
        }
    }

    private void Drop()
    {
        bool isRightPressed = controls.Player.RightThrow.ReadValue<float>() > 0.1f;

        if(isRightPressed && grabObjR)
        {
            StartCoroutine(RightDrop());
        }
    }

    private IEnumerator RightGrab()
    {
        
        objectGrabbedR = false;
        hit.transform.GetComponent<ObjectsData>().OnGrab(objectRightCamera);
        objectRightT = hit.transform;
        ObjectRightUI.SetActive(false);
        objectNameString = hit.transform.GetComponent<ObjectsData>().ObjectName;
        yield return new WaitForSeconds(0.5f);
        grabObjR = true;

    }

    public IEnumerator RightDrop()
    {
        objectRightT.GetComponent<ObjectsData>().OnRelease();
        yield return new WaitForSeconds(0.501f);
        grabObjR = false;
        objectGrabbedR = true;
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

    private void Video()
    {
        if(GameManager.Instance.ending.time == 35.0)
        {
            GameManager.Instance.GetComponent<PhotonView>().RPC("StartVideo", RpcTarget.All, 0);
        }

        if (GameManager.Instance.ending.time == 200.0)
        {
            GameManager.Instance.GetComponent<PhotonView>().RPC("StartVideo", RpcTarget.All, 1);
        }
    }
}