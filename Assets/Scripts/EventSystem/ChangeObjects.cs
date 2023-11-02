using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject HospitalDoor;
    [SerializeField]
    private GameObject HospitalLightsLockdown;

    [PunRPC]
    public void ActivatePascualita()
    {
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
        Nurse.SetActive(false);
        HospitalDoor.GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
    }
}
