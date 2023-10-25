using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterKeys : MonoBehaviourPun
{
    [PunRPC]
    public void MasterKeysChange(string keyName)
    {
        GameObject masterKey = GameObject.Find(keyName);
        masterKey.SetActive(false);
        if (keyName == "Key1")
        {
            GameManager.Instance.Key1 = true;
        }
        if (keyName == "Key2")
        {
            GameManager.Instance.Key2 = true;
        }
        if (keyName == "Key3")
        {
            GameManager.Instance.Key3 = true;
        }
    }
}
