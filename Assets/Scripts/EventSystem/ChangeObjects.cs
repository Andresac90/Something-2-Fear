using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjects : MonoBehaviourPun
{
    [PunRPC]
    public void ChangeObject(string name)
    {
        GameObject Objects = GameObject.Find(name);
        bool Value = Objects.activeInHierarchy;
        Objects.SetActive(!Value);
    }
}
