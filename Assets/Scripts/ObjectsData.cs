using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class ObjectsData : MonoBehaviourPun
{
    public float ObjectScale;

    public float ObjectOriginalScale;

    public string ObjectName;

    public void OnGrab(Transform parent)
    {
        string parentName = parent.name;
        photonView.RPC("SyncItemParent", RpcTarget.All, parentName);
        photonView.RPC("SyncScale", RpcTarget.All, parent.localScale, ObjectScale);
    }

    public void OnRelease()
    {
        photonView.RPC("SyncItemParent", RpcTarget.All, "");
        photonView.RPC("SyncScale", RpcTarget.All, Vector3.one, ObjectOriginalScale);
    }

    public void onThrow(Vector3 direction, float force)
    {
        
        photonView.RPC("SyncItemParent", RpcTarget.All, "");
        photonView.RPC("SyncScale", RpcTarget.All, Vector3.one, ObjectOriginalScale);
        photonView.RPC("SyncItemForce", RpcTarget.All, direction, force);


    }

    [PunRPC]
    public void SyncItemForce(Vector3 direction, float force){
        GetComponent<Rigidbody>().AddForce(direction * force);
    }

    [PunRPC]
    public void SyncScale(Vector3 direction, float scale){
        transform.localScale = direction * scale;
    }

    [PunRPC]
    public void SyncItemParent(string parentName)
    {
        if (parentName == "")
        {
            transform.parent = null;
            GetComponent<Rigidbody>().isKinematic = false;

            return;
        }

        GameObject parent = GameObject.Find(parentName);

        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        transform.GetComponent<Rigidbody>().isKinematic = true;
    }
}
