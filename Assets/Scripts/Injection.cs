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
    public bool cured = true;
    public float currentTime = 0f;
    private bool santiInjected = false;
    private bool joseInjected = false;
    private bool timeOver = false;
    public float downTime = 35f;

    private PhotonView JosePV;
    private PhotonView SantiPV;

    private SantiController santiController;
    private JoseMovement joseController;
    public bool isPlayerInjected;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = this.transform.GetChild(0).gameObject;
        camera = playerCam.GetComponent<Camera>();
        CharController = GetComponent<CharacterController>();
        santiController = GetComponent<SantiController>();
        joseController = GetComponent<JoseMovement>();
        if(santiController != null)
        {
            SantiPV = gameObject.GetComponent<PhotonView>();
        } 
        else
        {
            JosePV = gameObject.GetComponent<PhotonView>();
        }
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
        checkInjection();
    }

    public void Injected(bool isPlayerCaught)
    {
        isPlayerInjected = isPlayerCaught;

        if (this.name == "Santi(Clone)")
        {
            if (isPlayerInjected && cured)
            {
                santiController.SetInjected(true);
                camera.fieldOfView = 110;
                camera.GetComponent<PlayerLook>().SetInvert(true);
                currentTime = 0f;

                cured = false;
                santiInjected = true;
            }
            else if (!cured)
            {
                currentTime += Time.deltaTime;
            }
        }
        else if (this.name == "Jose(Clone)")
        {
            if (isPlayerInjected && cured)
            {
                joseController.SetInjected(isPlayerCaught);
                camera.fieldOfView = 110;
                camera.GetComponent<PlayerLook>().SetInvert(true);
                currentTime = 0f;

                cured = false;
                joseInjected = true;
            }
            else if (!cured)
            {
                currentTime += Time.deltaTime;
            }
        }
    }

    public void Cured()
    {
        if (this.name == "Santi(Clone)")
        {
            santiController.SetInjected(false);
            camera.fieldOfView = 60;
            camera.GetComponent<PlayerLook>().SetInvert(false);
            currentTime = 0f;
            cured = true;
        }
        else if (this.name == "Jose(Clone)")
        {
            joseController.SetInjected(false);
            camera.fieldOfView = 60;
            camera.GetComponent<PlayerLook>().SetInvert(false);
            currentTime = 0f;
            cured = true;
        }
    }

    public void checkInjection()
    {
        if (currentTime > downTime)
        {
            if(this.name == "Santi(Clone)")
            {
                isPlayerInjected = false;
                Cured();
                SantiPV.RPC("SyncDowned", RpcTarget.All);
            }
            else
            {
                isPlayerInjected = false;
                Cured();
                JosePV.RPC("SyncDowned", RpcTarget.All);
            }
        }
    }
}
