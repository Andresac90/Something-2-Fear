using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private InputMaster Controls;

    public GameObject Task;
    bool openTask;

    void Awake()
    {
        Controls = new InputMaster();
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        bool IsInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0f;
        if(isTaskActive() && IsInteractPressed)
        {
            Instantiate(Task);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            openTask = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            openTask = false;
        }
    }

    private bool isTaskActive()
    {
        return openTask && !GameObject.FindWithTag("Task");
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
