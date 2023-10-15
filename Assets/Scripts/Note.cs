using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour
{
    private InputMaster controls;
    private GameObject noteCopy;
    private GameObject playerMove;
    private Transform playerCam;
    private RaycastHit hit;
    private TextMeshProUGUI textMesh;
    private Image img;
    private bool noteCreated = false;

    [SerializeField]
    private GameObject noteObj;
    [SerializeField]
    private float rayLine;
    [SerializeField]
    private string text;
    [SerializeField]
    private Image image;
    
    public void Awake()
    {
        controls = new InputMaster();
    }

    void Start()
    {
        playerMove = GameObject.Find("Santi");
        playerCam = playerMove.transform.GetChild(0).GetComponent<Transform>();
        GameObject child = noteObj.transform.GetChild(0).gameObject;
        textMesh = child.GetComponentInChildren<TextMeshProUGUI>();
        GameObject sonChild = child.transform.GetChild(1).gameObject;
        img = sonChild.GetComponent<Image>();
        img = image;
        textMesh.text = text;
    }

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
        if(IsInteractPressed && !noteCreated)
        {
            PlayerMovement();
            noteCopy = Instantiate(noteObj);
            noteCreated = true;
        }
    }

    private void CloseNote()
    {
        bool IsCancelPressed = controls.Player.Cancel.ReadValue<float>() > 0f;
        if(IsCancelPressed)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        if(!noteCreated)
        {
            playerMove.GetComponent<SantiController>().enabled = false;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(noteCreated)
        {
            playerMove.GetComponent<SantiController>().enabled = true;
            playerMove.GetComponentInChildren<PlayerLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            noteCreated = false;
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
