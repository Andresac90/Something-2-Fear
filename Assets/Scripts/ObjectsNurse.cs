using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectsNurse : MonoBehaviourPun
{
    [PunRPC]
    public void ObjectsNurseChange(string objectName)
    {
        GameObject objectNurse = GameObject.Find(objectName);
        objectNurse.SetActive(false);
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

    [PunRPC]
    public void SpawnInjection()
    {
        GameManager.Instance.InjectionSpawn = true;
    }

    [PunRPC]
    public void InjectionChange(string objectName)
    {
        GameObject injection = GameObject.Find(objectName);
        injection.SetActive(false);
        GameManager.Instance.Injection = true;
    }
}
