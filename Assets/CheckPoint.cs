using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviourPun
{
    [SerializeField]
    private CheckPointManager CheckPointManager;
    [SerializeField]
    private bool SantiHasPassed = false;    
    [SerializeField]
    private bool JoseHasPassed = false;


    public bool PascualitaIsOn = false;
    public bool EnfermeraIsOn = false;
    public bool NinaIsOn = false;
    public GameObject[] FoundKeys;
    public GameObject[] OpenedDoors;

    public void Start()
    {
        CheckPointManager = GameObject.Find("CheckPointManager").GetComponent<CheckPointManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Santi(Clone)" && !SantiHasPassed)
        {
            photonView.RPC("SantiPassed", RpcTarget.All);
        }
        else if (other.name == "Jose(Clone)" && !JoseHasPassed)
        {
            photonView.RPC("JosePassed", RpcTarget.All);
        }
    }

    [PunRPC]
    public void SantiPassed()
    {
        SantiHasPassed = true;
        CheckIfBothPlayersPassed();
    }

    [PunRPC]
    public void JosePassed()
    {
        JoseHasPassed = true;
        CheckIfBothPlayersPassed();
    }

    public void CheckIfBothPlayersPassed()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (SantiHasPassed && JoseHasPassed)
        {
            CheckPointManager.GetComponent<PhotonView>().RPC("SetCheckPoint", RpcTarget.All, photonView.ViewID);
        }
    }
}
