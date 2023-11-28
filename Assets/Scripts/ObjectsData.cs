using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class ObjectsData : MonoBehaviourPun
{
    public float ObjectScale;
    public float ObjectYScale;
    public float ObjectOriginalScale;
    public float ObjectOriginalYScale;

    public string ObjectName;

    public void OnGrab(Transform parent)
    {
        string parentName = parent.name;
        photonView.RPC("SyncItemParent", RpcTarget.All, parentName);
        photonView.RPC("SyncScale", RpcTarget.All, parent.localScale, ObjectScale, ObjectYScale);
    }

    public void OnRelease()
    {
        photonView.RPC("SyncItemParent", RpcTarget.All, "");
        photonView.RPC("SyncScale", RpcTarget.All, Vector3.one, ObjectOriginalScale, ObjectOriginalYScale);
    }

    public void onThrow(Vector3 direction, float force)
    {
        
        photonView.RPC("SyncItemParent", RpcTarget.All, "");
        photonView.RPC("SyncScale", RpcTarget.All, Vector3.one, ObjectOriginalScale, ObjectOriginalYScale);
        photonView.RPC("SyncItemForce", RpcTarget.All, direction, force);


    }

    [PunRPC]
    public void SyncItemForce(Vector3 direction, float force){
        GetComponent<Rigidbody>().AddForce(direction * force);
    }

    [PunRPC]
    public void SyncScale(Vector3 direction, float scale, float scale_y){
        transform.localScale = direction * scale;
        Vector3 newScale = transform.localScale;
        newScale.y = direction.y * scale_y;
        transform.localScale = newScale;
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
