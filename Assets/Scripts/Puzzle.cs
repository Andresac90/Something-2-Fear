using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private InputMaster controls;

    public GameObject task;
    
    private GameObject taskCopy;
    private GameObject playerMove;
    private bool playerNear;
    private bool taskCreated = false;

    void Awake()
    {
        controls = new InputMaster();
        playerMove = GameObject.Find("Santi");
    }

    // Update is called once per frame
    void Update()
    {
        OpenPuzzle();
        ClosePuzzle();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("PlayerSanti"))
        {
            playerNear = true;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("PlayerSanti"))
        {
            playerNear = false;
        }
    }

    private bool isPuzzleActive()
    {
        return playerNear;
    }

    private void OpenPuzzle()
    {
        bool IsInteractPressed = controls.Player.Interact.ReadValue<float>() > 0f;
        if(isPuzzleActive() && IsInteractPressed && taskCreated == false)
        {
            PlayerMovement();
            taskCopy = Instantiate(task);
            taskCreated = true;
        }
        else if(isPuzzleActive() && IsInteractPressed && taskCopy.activeSelf == false)
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
