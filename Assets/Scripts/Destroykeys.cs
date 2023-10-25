using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroykeys : MonoBehaviourPun
{
    [PunRPC]
    public void DestroyKeyOnline(string name)
    {
        GameObject key = GameObject.Find(name);
        key.SetActive(false);

    }
}
