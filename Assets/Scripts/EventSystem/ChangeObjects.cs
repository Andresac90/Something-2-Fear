using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjects : MonoBehaviourPun
{
    [PunRPC]
    public void ChangeObject(GameObject Objects)
    {
        bool Value = Objects.activeInHierarchy;
        Objects.SetActive(!Value);
    }
}
