using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;
    public delegate void PlayerJoinedEventHandler(GameObject playerObject);
    public static event PlayerJoinedEventHandler OnPlayerJoined;
    
    void Start()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
        if (OnPlayerJoined != null)
        {
            OnPlayerJoined(gameObject);
        }
    }
}
