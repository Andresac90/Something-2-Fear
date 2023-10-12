using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private InputMaster controls;

    [SerializeField]
    private GameObject noteObj;
    [SerializeField]
    private float rayLine;
    
    private GameObject noteCopy;
    private GameObject playerMove;
    private GameObject child;
    private Transform playerCam;
    private bool playerNear;
    private bool noteCreated = false;
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
        if(hit.transform != null && hit.transform.tag == "Note")
        {
            OpenNote();
        }
        if(noteCreated)
        {
            CloseNote();
        }
    }

    private void OpenNote()
    {
        bool IsInteractPressed = controls.Player.Interact.ReadValue<float>() > 0f;
        if(IsInteractPressed && noteCreated == false)
        {
            PlayerMovement();
            noteCopy = Instantiate(noteObj);
            noteCreated = true;
        }
        else if(IsInteractPressed && noteCopy.activeSelf == false)
        {
            PlayerMovement();
            noteCopy.SetActive (true);
        }
    }

    private void CloseNote()
    {
        bool IsCancelPressed = controls.Player.Cancel.ReadValue<float>() > 0f;
        if(IsCancelPressed && noteCopy.activeSelf)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        if(noteCreated == false || noteCopy.activeSelf == false)
        {
            playerMove.GetComponent<SantiController>().enabled = false;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(noteCopy.activeSelf)
        {
            playerMove.GetComponent<SantiController>().enabled = true;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(noteCopy);
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
