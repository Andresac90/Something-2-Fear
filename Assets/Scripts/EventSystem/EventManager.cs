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
    private bool WinScreen = false;

    private GameObject ChangeObjects;

    public bool santiNear = false;
    public bool joseNear = false;

    // Update is called once per frame
    void Update()
    {
        Interact();
        JoseInteract();
        SantiInteract();
    }

    void Start()
    {
        ChangeObjects = GameObject.Find("ChangeObjects");
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

    void Interact()
    {
        if (joseNear && santiNear && TwoPlayers && Event)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivatePascualita", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivateLights", RpcTarget.All);
            Destroy(this.gameObject);
        }
        else if (joseNear && santiNear && TwoPlayers && WinScreen)
        {
            SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("WinScreen");
            Destroy(this.gameObject);
        }
    }

    void JoseInteract()
    {
        if (joseNear && JoseEvent && !SantiEvent)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ActivatePascualita", RpcTarget.All);
            Destroy(this.gameObject);
        }
        else if (joseNear && JoseEvent && !SantiEvent && WinScreen)
        {
            SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("WinScreen");
            Destroy(this.gameObject);
        }

    }
    void SantiInteract()
    {
        if (santiNear && !JoseEvent && SantiEvent)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ActivatePascualita", RpcTarget.All);
            Destroy(this.gameObject);
        }
        else if (santiNear && !JoseEvent && SantiEvent && WinScreen)
        { 
            SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("WinScreen");
            Destroy(this.gameObject);
        }

    }
}
