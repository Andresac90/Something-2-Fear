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
    public float downTime = 35f;

    private PhotonView JosePV;
    private PhotonView SantiPV;

    public AudioSource AudioInjection;

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
        AudioInjection = GameObject.Find("AudioInjection").GetComponent<AudioSource>();
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

    [PunRPC]
    void updateCured()
    {
        Cured();
    }

    void Update()
    {
        if (isPlayerInjected)
        {
            Injected(isPlayerInjected);
        }
        Debug.Log(cured);
        checkInjection();
    }

    public void Injected(bool isPlayerCaught)
    {
        isPlayerInjected = isPlayerCaught;

        if (this.name == "Santi(Clone)")
        {
            if (isPlayerInjected && cured)
            {
                AudioInjection.Play();
                santiController.SetInjected();
                camera.GetComponent<PlayerLook>().SetInvert(true);
                currentTime = 0f;

                cured = false;
            }
            else if (cured == false)
            {
                camera.fieldOfView++;
                currentTime += Time.deltaTime;
            }
        }
        else if (this.name == "Jose(Clone)")
        {
            if (isPlayerInjected && cured)
            {
                AudioInjection.Play();
                joseController.SetInjected();
                camera.GetComponent<PlayerLook>().SetInvert(true);
                currentTime = 0f;

                cured = false;
            }
            else if (cured == false)
            {
                camera.fieldOfView++;
                currentTime += Time.deltaTime;
            }
        }
    }

    public void Cured()
    {
        if (this.name == "Santi(Clone)")
        {
            AudioInjection.Stop();
            santiController.SetCured();
            camera.fieldOfView = 60;
            camera.GetComponent<PlayerLook>().SetInvert(false);
            currentTime = 0f;
            cured = true;
            isPlayerInjected = false;
        }
        else if (this.name == "Jose(Clone)")
        {
            AudioInjection.Stop();
            joseController.SetCured();
            camera.fieldOfView = 60;
            camera.GetComponent<PlayerLook>().SetInvert(false);
            currentTime = 0f;
            cured = true;
            isPlayerInjected = false;
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
