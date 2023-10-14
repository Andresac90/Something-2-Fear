using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private InputMaster controls;
    private GameObject puzzleCopy;
    private GameObject playerMove;
    private Transform playerCam;
    private bool puzzleCreated = false;
    private RaycastHit hit;

    [SerializeField]
    private GameObject puzzle;
    [SerializeField]
    private float rayLine;
    [SerializeField]
    private Door door;
    [SerializeField]
    private int comprobationsNeeded;

    public int comprobations;
    public string password;
    
    public void Awake()
    {
        controls = new InputMaster();
    }

    void Start()
    {
        playerMove = GameObject.Find("Santi");
        playerCam = playerMove.transform.GetChild(0).GetComponent<Transform>();
    }

    void Update()
    {
        Physics.Raycast(playerCam.position, playerCam.TransformDirection(Vector3.forward), out hit, rayLine);
        if(hit.transform != null && hit.transform.tag == "Puzzle")
        {
            OpenPuzzle();
        }
        if(puzzleCreated)
        {
            ClosePuzzle();
        }
    }

    private void OpenPuzzle()
    {
        bool IsInteractPressed = controls.Player.Interact.ReadValue<float>() > 0f;
        if(IsInteractPressed && !puzzleCreated)
        {
            PlayerMovement();
            puzzleCopy = Instantiate(puzzle);
            puzzleCreated = true;
        }
        else if(IsInteractPressed && !puzzleCopy.activeSelf)
        {
            PlayerMovement();
            puzzleCopy.SetActive (true);
        }
    }

    private void ClosePuzzle()
    {
        bool IsCancelPressed = controls.Player.Cancel.ReadValue<float>() > 0f;
        if(IsCancelPressed && puzzleCopy.activeSelf)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        if(!puzzleCreated || !puzzleCopy.activeSelf)
        {
            playerMove.GetComponent<SantiController>().enabled = false;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(puzzleCopy.activeSelf)
        {
            playerMove.GetComponent<SantiController>().enabled = true;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            puzzleCopy.SetActive(false);
        }
    }

    public void Completed()
    {
        if(comprobations == comprobationsNeeded)
        {
            door.doorState = true;
            door.OpenDoor();
            Destroy(puzzleCopy.gameObject, 1f);
            PlayerMovement();
            Destroy(this);
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
