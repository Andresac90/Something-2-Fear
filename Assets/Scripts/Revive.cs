using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Revive : MonoBehaviourPun
{
    private InputMaster controls;
    private RaycastHit hit;
    private float currentTime = 0f;
    private Transform playerCamera;
    private PhotonView playerPV;
    private PhotonView objectivePlayerPV;
    private int playersJoined = 0;
    
    [SerializeField]
    private float objectTime;
    [SerializeField]
    private float rayLine;
    
    public void Awake()
    {
        controls = new InputMaster();
    }
    public void Start()
    {
        playerCamera = this.transform.GetChild(0).GetComponent<Transform>();
        playerPV = GetComponent<PhotonView>();
        GameManager.OnPlayersJoined += HandlePlayersJoined;
    }

    public void Update()
    {
        Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine);
        if (hit.transform != null && (hit.transform.tag == "PlayerSanti" || hit.transform.tag == "PlayerJose"))
        {
            Reviving();
        }
    }

    private void HandlePlayersJoined(PhotonView player1PV, PhotonView player2PV)
    {
        playerPV = player1PV;
        objectivePlayerPV = player2PV;
    }

    private void Reviving()
    {
        bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0f;
        if(isInteractPressed)
        {
            currentTime += Time.deltaTime;
            Debug.Log("Presionado");
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
        Debug.Log("curado");
        objectivePlayerPV.RPC("updateDowned", RpcTarget.All, false);
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
