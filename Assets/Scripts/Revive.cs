using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    private InputMaster controls;
    private RaycastHit hit;
    private float currentTime = 0f;
    private Transform playerCamera;
    private GameObject player;
    
    [SerializeField]
    private float objectTime = 5f;
    [SerializeField]
    private float rayLine;
    
    public void Awake()
    {
        controls = new InputMaster();
    }
    public void Start()
    {
        playerCamera = this.transform.GetChild(0).GetComponent<Transform>();
        // PlayerManager.OnPlayerJoined += HandlePlayerJoined;
    }

    public void Update()
    {
        Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine);
        if (hit.transform != null && (hit.transform.tag == "PlayerSanti" || hit.transform.tag == "PlayerJose"))
        {
            Reviving();
        }
    }

    // private void OnDestroy()
    // {
    //     PlayerManager.OnPlayerJoined -= HandlePlayerJoined;
    // }

    // private void HandlePlayerJoined(GameObject playerObject)
    // {
    //     if (playerObject != this.gameObject)
    //     {
    //         player = playerObject;
    //         playerPV = GameObject.Find(player).GetComponent<PhotonView>();
    //     }
    // }

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
        // playerPV.RPC("updateDowned", RpcTarget.All, false);
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
