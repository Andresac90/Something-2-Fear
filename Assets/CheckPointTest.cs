using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CheckPointTest : MonoBehaviourPun
{
    public CheckPointManager CheckPointManager;
    void Start()
    {
        CheckPointManager = GameObject.Find("CheckPointManager").GetComponent<CheckPointManager>();
    }

    public void GoToCheckPoint()
    {
        CheckPointManager.GetComponent<PhotonView>().RPC("GoToCheckPoint", RpcTarget.All);
    }
}
