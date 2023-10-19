using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    private InputMaster controls;
    private RaycastHit hit;
    private float currentTime = 0f;
    private Transform playerCamera;
    
    [SerializeField]
    private float objectTime = 10f;
    [SerializeField]
    private float rayLine;
    [SerializeField]
    private GameObject player;
    
    public void Awake()
    {
        controls = new InputMaster();
    }
    public void Start()
    {
        playerCamera = player.transform.GetChild(0).GetComponent<Transform>();
    }

    public void Update()
    {
        Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine);
        if (hit.transform != null && (hit.transform.tag == "PlayerSanti" || hit.transform.tag == "PlayerJose"))
        {
            Reviving();
        }
    }

    private void Reviving()
    {
        bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0f;
        if(isInteractPressed)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= objectTime)
            {
                Cured();
            }
        }
        else
        {
            currentTime = 0f;
        }
    }

    private void Cured()
    {
        Down down = player.GetComponent<Down>();
        down.isPlayerDowned = false;
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
