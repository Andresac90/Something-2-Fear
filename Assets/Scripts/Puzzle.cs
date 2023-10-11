using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private InputMaster controls;

    [SerializeField]
    private GameObject task;
    [SerializeField]
    private float rayLine;
    
    private GameObject taskCopy;
    private GameObject playerMove;
    private GameObject child;
    private Transform playerCam;
    private bool playerNear;
    private bool taskCreated = false;
    private RaycastHit hit;
    

    public void Awake()
    {
        controls = new InputMaster();
    }
    void Start()
    {
        playerMove = GameObject.Find("Santi");
        child = playerMove.transform.GetChild(0).gameObject;
        playerCam = child.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(playerCam.position, playerCam.TransformDirection(Vector3.forward), out hit, rayLine);
        if(hit.transform != null && hit.transform.tag == "Puzzle")
        {
            OpenPuzzle();
        }
        ClosePuzzle();
    }

    // public void OnTriggerEnter(Collider collision)
    // {
    //     if(collision.CompareTag("PlayerSanti"))
    //     {
    //         playerNear = true;
    //     }
    // }

    // public void OnTriggerExit(Collider collision)
    // {
    //     if(collision.CompareTag("PlayerSanti"))
    //     {
    //         playerNear = false;
    //     }
    // }

    // private bool isPuzzleActive()
    // {
    //     return playerNear;
    // }

    private void OpenPuzzle()
    {
        bool IsInteractPressed = controls.Player.Interact.ReadValue<float>() > 0f;
        if(IsInteractPressed && taskCreated == false)
        {
            PlayerMovement();
            taskCopy = Instantiate(task);
            taskCreated = true;
        }
        else if(IsInteractPressed && taskCopy.activeSelf == false)
        {
            PlayerMovement();
            taskCopy.SetActive (true);
        }
    }

    private void ClosePuzzle()
    {
        bool IsCancelPressed = controls.Player.Cancel.ReadValue<float>() > 0f;
        if(IsCancelPressed && taskCopy.activeSelf)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        if(taskCreated == false || taskCopy.activeSelf == false)
        {
            playerMove.GetComponent<SantiController>().enabled = false;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(taskCopy.activeSelf)
        {
            playerMove.GetComponent<SantiController>().enabled = true;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            taskCopy.SetActive(false);
        }
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
