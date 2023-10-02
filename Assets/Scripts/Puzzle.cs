using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private InputMaster Controls;

    public GameObject Task;
    bool playerNear;

    void Awake()
    {
        Controls = new InputMaster();
    }

    // Update is called once per frame
    void Update()
    {
        OpenPuzzle();
        ClosePuzzle();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("Player"))
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
        bool IsInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0f;
        if(isPuzzleActive() && IsInteractPressed && Task.activeSelf == false)
        {
            Task.SetActive (true);
            //Instantiate(Task);
        }
    }

    private void ClosePuzzle()
    {
        bool IsCancelPressed = Controls.Player.Cancel.ReadValue<float>() > 0f;
        if(Task.activeSelf && IsCancelPressed)
        {
            Task.SetActive (false);
            //Instantiate(Task);
        }
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
