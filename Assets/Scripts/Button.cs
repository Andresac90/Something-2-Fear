using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Button : MonoBehaviourPun
{
    [SerializeField]
    private Rejilla rejilla;
    private PhotonView PV;
    
    public void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Activation(bool state)
    {
        PV.RPC("OpenR", RpcTarget.All, state);
        
    }

    [PunRPC]
    void OpenR(bool state)
    {
        rejilla.OpenRejilla(state);
    }
}
