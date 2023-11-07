using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviourPun
{
    [SerializeField]
    private Animator door;

    public bool doorState;
    
    public void OpenDoor()
    {
        if(doorState)
        {
            door.SetBool("activate", true);
            door.SetBool("doorActive", true);
            doorState = false;
        }
        else if(!doorState)
        {
            door.SetBool("doorActive", false);
            doorState = true;
        }
    }

    [PunRPC]
    public void SyncDoor(bool state)
    {
        doorState = state;
        OpenDoor();
    }
}
