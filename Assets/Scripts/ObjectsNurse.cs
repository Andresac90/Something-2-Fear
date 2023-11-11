using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsNurse : MonoBehaviourPun
{
    [PunRPC]
    public void ObjectsNurseChange(string objectName)
    {
        GameObject masterKey = GameObject.Find(objectName);
        masterKey.SetActive(false);
        if (objectName == "Object1")
        {
            GameManager.Instance.Object1 = true;
        }
        if (objectName == "Object2")
        {
            GameManager.Instance.Object2 = true;
        }
        if (objectName == "Object3")
        {
            GameManager.Instance.Object3 = true;
        }
    }
}
