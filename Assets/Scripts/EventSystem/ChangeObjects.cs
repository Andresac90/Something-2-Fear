using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeObjects : MonoBehaviourPun
{
    [SerializeField]
    private GameObject pascualita;
    [SerializeField]
    private GameObject dummy;
    [SerializeField] 
    private GameObject lights;
    [SerializeField]
    private GameObject Nurse;
    [SerializeField]
    private GameObject Nina;
    [SerializeField]
    private GameObject HospitalDoor;
    [SerializeField]
    private GameObject HospitalLightsLockdown;
    [SerializeField]
    private Transform positionNina;
    [SerializeField]
    private GameObject Block1;
    [SerializeField]
    private GameObject Block2;
    [SerializeField]
    private GameObject Block3;
    [SerializeField]
    private GameObject Block4;
    [SerializeField]
    private GameObject Zone2Lights;
    [SerializeField]
    private GameObject Zone3Lights;

    [PunRPC]
    public void ActivatePascualita()
    {
        GameManager.Instance.PascualaLaugh.Play();
        pascualita.SetActive(true);
    }

    [PunRPC]
    public void DeactivatePascualita()
    {
        pascualita.SetActive(false);
    }

    [PunRPC]
    public void DeactivateDummy()
    {
        dummy.SetActive(false);
    }

    [PunRPC]
    public void ActivateLights()
    {
        lights.SetActive(true);
    }

    [PunRPC]
    public void DeactivateLights()
    {
        GameManager.Instance.Ambience1.Stop();
        lights.SetActive(false);
    }

    [PunRPC]
    public void ActivateNurse()
    {
        Nurse.SetActive(true);
        HospitalLightsLockdown.SetActive(false);
    }

    [PunRPC]
    public void DeactivateNurse()
    {
        Nurse.SetActive(false);
    }

    [PunRPC]
    public void ActivateLockdown()
    {
        HospitalLightsLockdown.SetActive(true);
        HospitalDoor.GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, false);
    }

    [PunRPC]
    public void DeactivateLockdown()
    {
        HospitalDoor.GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
    }

    [PunRPC]
    public void ChangeNina()
    {
        GameManager.Instance.JoseInsideLab = true; 
        Nina.GetComponent<NavMeshAgent>().Warp(positionNina.position);
    }

    [PunRPC]
    public void DeactivateNina()
    {
        Nina.SetActive(false);
    }

    [PunRPC]
    public void EraseBlock1()
    {
        Block1.SetActive(false);
    }

    [PunRPC]
    public void EraseBlock2()
    {
        Block2.SetActive(false);
        Zone2Lights.SetActive(true);
    }

    [PunRPC]
    public void EraseBlock3()
    {
        Block3.SetActive(false);
        Zone3Lights.SetActive(true);
    }

    [PunRPC]
    public void EnableBlock4()
    {
        Block4.SetActive(true);
    }

    [PunRPC]
    public void EraseBlock4()
    {
        Block4.SetActive(false);
    }
}
