using Photon.Pun;
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
    private GameObject[] Objects;
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
        if (joseNear && santiNear && TwoPlayers && !RunOnce && Event)
        {
            Console.WriteLine("Players IN");
            for (int i = 0; i < Objects.Length; i++)
            {
                //bool Value = Objects[i].activeInHierarchy;
                Console.WriteLine(Objects.Length);
                Objects[i].GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Objects[i]);
                RunOnce = true;
            }
            //AI.SetActive(true);
            Destroy(this);
        }
        else if (joseNear && santiNear && TwoPlayers && !RunOnce && WinScreen)
        {
            PhotonNetwork.LoadLevel("WinScreen");
        }
    }

    void JoseInteract()
    {
        if (joseNear && !TwoPlayers && JoseEvent && !SantiEvent && !RunOnce && Event)
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                //bool Value = Objects[i].activeInHierarchy;
                Console.WriteLine(Objects.Length);
                Objects[i].GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Objects[i]);
                RunOnce = true;
            }
            //AI.SetActive(true);
            Destroy(this);
        }
        else if (joseNear && TwoPlayers && JoseEvent && !SantiEvent && !RunOnce && WinScreen)
        {
            SceneManager.LoadScene("WinScreen");
        }

    }
    void SantiInteract()
    {
        if (santiNear && !TwoPlayers && !JoseEvent && SantiEvent && !RunOnce && Event)
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                //bool Value = Objects[i].activeInHierarchy;
                Console.WriteLine(Objects.Length);
                Objects[i].GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Objects[i]);
                RunOnce = true;
            }
            //AI.SetActive(true);
            Destroy(this);
        }
        else if (santiNear && TwoPlayers && !JoseEvent && SantiEvent && !RunOnce && WinScreen)
        {
            SceneManager.LoadScene("WinScreen");
        }

    }
}
