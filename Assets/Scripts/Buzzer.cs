using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buzzer : MonoBehaviour
{
    [SerializeField]
    private GameObject Nina;

    // Start is called before the first frame update
    [PunRPC]
    public void Activation()
    {
        GameManager.Instance.Buzzer.Play();
        Nina.GetComponent<PhotonView>().RPC("InvertPlayers", RpcTarget.All);
    }

    public void Update()
    {
        
    }
}
