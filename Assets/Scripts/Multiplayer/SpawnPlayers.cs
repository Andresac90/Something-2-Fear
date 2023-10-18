using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject josePrefab;
    public GameObject santiPrefab;

    void Start()
    {
        int playerNumber = PhotonNetwork.CurrentRoom.PlayerCount;

        if(playerNumber == 1)
        {
            GameObject spawn = GameObject.FindGameObjectsWithTag("SantiSpawn")[0];
            PhotonNetwork.Instantiate(santiPrefab.name, spawn.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject spawn = GameObject.FindGameObjectsWithTag("JoseSpawn")[0];
            PhotonNetwork.Instantiate(josePrefab.name, spawn.transform.position, Quaternion.identity);
        }
        
    }
}
