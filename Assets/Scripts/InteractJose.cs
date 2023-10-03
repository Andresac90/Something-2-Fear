using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractJose : MonoBehaviour
{
    private InputMaster Controls;

    public GameObject Task;
    public bool PlayerNear;
    // Start is called before the first frame update
    void Awake()
    {
        Controls = new InputMaster();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerJose"))
        {
            PlayerNear = true;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("PlayerJose"))
        {
            PlayerNear = false;
        }
    }

    private bool isInteractActive()
    {
        return PlayerNear;
    }

    void Interact()
    {
        bool IsInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0.1f;
        if (isInteractActive() && IsInteractPressed && Task.activeSelf == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Task.SetActive(true);
            
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
