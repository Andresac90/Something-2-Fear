using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private bool TwoPlayers = true;
    [SerializeField]
    private bool JoseEvent = false;
    [SerializeField]
    private bool spawnPasc = false;
    [SerializeField]
    private bool SantiEvent = false;
    [SerializeField]
    private bool Event = false;
    [SerializeField]
    private bool Hospital = false;
    [SerializeField]
    private bool Labyrinth = false;
    [SerializeField]
    private bool WinScreen = false;

    private GameObject ChangeObjects;

    public bool santiNear = false;
    public bool joseNear = false;

    public bool HospitalEvent = false;

    // Update is called once per frame
    void Update()
    {
        Interact();
        SpawnPascuala();
        SantiInteract();
        HospitalLockdown();
        HospitalCleared();
        LabyrinthNina();
        if (GameManager.Instance.audioH && GameManager.Instance.AudioHospital.time > 17.0f)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ActivateNurse", RpcTarget.All);
        }
        if(GameManager.Instance.AudioHospital.time > 25.0f)
        {
            GameManager.Instance.audioH = false;
        }
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
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivateLights", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("EraseBlock1", RpcTarget.All);
            Destroy(this.gameObject);
        }
    }

    void SpawnPascuala()
    {
        if (joseNear && JoseEvent && spawnPasc)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ActivatePascualita", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivateDummy", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("EraseBlock2", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("EraseBlock3", RpcTarget.All);
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
        if (joseNear && santiNear && TwoPlayers && Hospital && !HospitalEvent)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ActivateLockdown", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("EnableBlock4", RpcTarget.All);
            GameManager.Instance.AudioHospital.Play();
            GameManager.Instance.audioH = true;
            HospitalEvent = true;
        }
    }

    void HospitalCleared()
    {
        if (GameManager.Instance.Key3)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("EraseBlock4", RpcTarget.All);
            ChangeObjects.GetComponent<PhotonView>().RPC("DeactivateLockdown", RpcTarget.All);
        }
    }

    void LabyrinthNina()
    {
        if (joseNear && Labyrinth)
        {
            ChangeObjects.GetComponent<PhotonView>().RPC("ChangeNina", RpcTarget.All);

            Destroy(this.gameObject);
        }
    }
}
