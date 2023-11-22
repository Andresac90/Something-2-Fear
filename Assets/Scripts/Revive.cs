using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Revive : MonoBehaviourPun
{
    private InputMaster controls;
    private RaycastHit hit;
    private float currentTime = 0f;
    private Transform playerCamera;
    private int playersJoined = 0;

    [SerializeField]
    private float objectTime;
    [SerializeField]
    private float rayLine;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image fill;

    public void Awake()
    {
        controls = new InputMaster();
    }
    public void Start()
    {
        playerCamera = transform.GetComponentInChildren<Camera>().gameObject.transform;
        // GameManager.OnPlayersJoined += HandlePlayersJoined;
        if (name == "Santi(Clone)")
        {
            layerMask = LayerMask.GetMask("PlayerJose");
        }
        else if (name == "Jose(Clone)")
        {
            layerMask = LayerMask.GetMask("PlayerSanti");
        }
    }

    public void Update()
    {
        if (!photonView.IsMine) return;
        

        CheckRevive();
    }

    private void CheckRevive()
    {
        bool isInteractPressed = controls.Player.Interact.ReadValue<float>() > 0f;
        if(isInteractPressed)
        {
            Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, rayLine, layerMask);
            if (hit.transform != null && (hit.transform.tag == "PlayerSanti" || hit.transform.tag == "PlayerJose"))
            {
                if (hit.transform.gameObject.GetComponent<Down>().isPlayerDowned == false) return;
                
                canvas.gameObject.SetActive(true);
                currentTime += Time.deltaTime;
                fill.fillAmount = currentTime / objectTime;
                if (currentTime >= objectTime)
                {
                    RevivePlayer(hit.transform.gameObject);
                    currentTime = 0f;
                    fill.fillAmount = currentTime / objectTime;
                }
            }
            else
            {
                canvas.gameObject.SetActive(false);
            }
        }
        else
        {
            currentTime = 0f;
            fill.fillAmount = currentTime / objectTime;
        }
    }

    private void RevivePlayer(GameObject playerObject)
    {
        PhotonView playerPV = playerObject.GetComponent<PhotonView>();
        if (playerPV == null) return;
        if (playerPV.IsMine) return;
        
        playerPV.RPC("SyncRevive", RpcTarget.All);
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
