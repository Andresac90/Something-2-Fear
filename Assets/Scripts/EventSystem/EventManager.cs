using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private bool TwoPlayers = true;
    [SerializeField]
    private bool JoseEvent = false;
    [SerializeField]
    private bool SantiEvent = false;
    [SerializeField]
    private bool Event = false;
    [SerializeField]
    private bool Hospital = false;
    [SerializeField]
    private bool WinScreen = false;
   
    public AudioSource AudioHospital;

    private GameObject ChangeObjects;

    public bool santiNear = false;
    public bool joseNear = false;

    public float currentAudio;

    // Update is called once per frame
    void Update()
    {
        Interact();
        JoseInteract();
        SantiInteract();
        HospitalLockdown();
    }

    void Start()
    {
        ChangeObjects = GameObject.Find("ChangeObjects");
        AudioHospital = GameObject.Find("AudioHospital").GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerJose"))
        {
            joseNear = true;
        }
        if(collision.CompareTag("PlayerSanti"))
        {
            santiNear = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerJose"))
        {
            joseNear = false;
        }
        if (other.CompareTag("PlayerSanti"))
        {
            santiNear = false;
        }
    }

    void Win()
    {
        if (joseNear && santiNear && TwoPlayers && WinScreen)
        {
            SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("WinScreen");
            Destroy(this.gameObject);
        }
    }

    void Interact()
    {
        if (joseNear && santiNear && TwoPlayers && Event)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivateLights", RpcTarget.All);
            Destroy(this.gameObject);
        }
    }

    void JoseInteract()
    {
        if (joseNear && JoseEvent && !SantiEvent)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ActivatePascualita", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivateDummy", RpcTarget.All);
            Destroy(this.gameObject);
        }
    }
    void SantiInteract()
    {
        if (santiNear && !JoseEvent && SantiEvent)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivatePascualita", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivateNurse", RpcTarget.All);
            Destroy(this.gameObject);
        }
    }

    void HospitalLockdown()
    {
        if (joseNear && santiNear && TwoPlayers && Hospital)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ActivateLockdown", RpcTarget.All);
            AudioHospital.Play();
            currentAudio = AudioHospital.time;
            while(currentAudio > 17.0f)
            {
                ChangeObjects.GetComponent<PhotonView>().RPC("ActivateNurse", RpcTarget.All);
                Destroy(this.gameObject);
            }
        }
    }
}
