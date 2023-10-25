using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Injection : MonoBehaviour
{
    private Camera camera;
    private CharacterController CharController;
    private GameObject playerCam;
    private bool wasInjected = false;
    private bool wasntInjected = true;
    private float currentTime = 0f;
    private bool santiInjected = false;
    private bool joseInjected = false;

    [SerializeField]
    private float downTime = 20f;

    private PhotonView JosePV;
    private PhotonView SantiPV;

    private SantiController santiController;
    private JoseMovement joseController;
    public bool isPlayerInjected;

    public float moveSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = this.transform.GetChild(0).gameObject;
        camera = playerCam.GetComponent<Camera>();
        CharController = GetComponent<CharacterController>();
        santiController = GetComponent<SantiController>();
        joseController = GetComponent<JoseMovement>();
    }

    [PunRPC]
    void updateInjected(bool status)
    {
        isPlayerInjected = status;
    }

    void Update()
    {
        if (isPlayerInjected)
        {
            Injected(isPlayerInjected);
        }
    }

    public void Injected(bool isPlayerCaught)
    {
        isPlayerInjected = isPlayerCaught;

        if (this.name == "Santi(Clone)")
        {
            if (isPlayerInjected && wasntInjected)
            {
                santiController.SetInjected(true);
                camera.fieldOfView = 100;

                wasInjected = true;
                wasntInjected = false;
                santiInjected = true;
            }
            else if (!isPlayerInjected && wasInjected)
            {
                santiController.SetInjected(false);

                wasInjected = false;
                wasntInjected = true;
                santiInjected = false;
                currentTime = 0f;
            }
            else if (!wasntInjected)
            {
                currentTime += Time.deltaTime;
                //SantiPV.RPC("updateDowned", RpcTarget.All, isPlayerCaught);
            }
        }
        else if (this.name == "Jose(Clone)")
        {
            if (isPlayerInjected && wasntInjected)
            {
                joseController.SetInjected(isPlayerCaught);
                camera.fieldOfView = 100;

                wasInjected = true;
                wasntInjected = false;
                joseInjected = true;
            }
            else if (!isPlayerInjected && wasInjected)
            {
                joseController.SetInjected(false);

                wasInjected = false;
                wasntInjected = true;
                joseInjected = false;
                currentTime = 0f;
            }
            else if (!wasntInjected)
            {
                currentTime += Time.deltaTime;
                //JosePV.RPC("updateDowned", RpcTarget.All, isPlayerCaught);
            }
        }
    }
}
