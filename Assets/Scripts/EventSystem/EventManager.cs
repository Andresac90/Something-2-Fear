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
    [SerializeField]
    private GameObject Pascuala;
    [SerializeField]
    private GameObject Lights;
    private bool RunOnce = false;

    public bool santiNear = false;
    public bool joseNear = false;

    // Update is called once per frame
    void Update()
    {
        Interact();
        JoseInteract();
        SantiInteract();
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
        if (joseNear && santiNear && TwoPlayers && !RunOnce && Event && Pascuala.activeSelf && Lights.activeSelf)
        {
            Pascuala.GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Pascuala.name);
            Lights.GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Lights.name);
            RunOnce = true;
            
            //AI.SetActive(true);
            Destroy(this.gameObject);
        }
        else if (joseNear && santiNear && TwoPlayers && !RunOnce && WinScreen)
        {
            //PhotonNetwork.AutomaticallySyncScene = true;
            //PhotonNetwork.LoadLevel("WinScreen");
            SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("WinScreen");
            RunOnce = true;
        }
    }

    void JoseInteract()
    {
        if (joseNear && !TwoPlayers && JoseEvent && !SantiEvent && !RunOnce && Event)
        {
            Pascuala.GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Pascuala.name);
            //Lights.GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Lights.name);
            RunOnce = true;
            //AI.SetActive(true);
            Destroy(this.gameObject);
        }
        else if (joseNear && TwoPlayers && JoseEvent && !SantiEvent && !RunOnce && WinScreen)
        {
            SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("WinScreen");
        }

    }
    void SantiInteract()
    {
        if (santiNear && !TwoPlayers && !JoseEvent && SantiEvent && !RunOnce && Event)
        {
            Pascuala.GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Pascuala.name);
            //Lights.GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Lights.name);
            RunOnce = true;
            //AI.SetActive(true);
            Destroy(this.gameObject);
        }
        else if (santiNear && TwoPlayers && !JoseEvent && SantiEvent && !RunOnce && WinScreen)
        {
            SoundFollow.Instance.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("WinScreen");
        }

    }
}
