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

        string player = (string)PhotonNetwork.LocalPlayer.CustomProperties["Player"];

        if(player == "Santi")
        {
            GameObject spawn = GameObject.FindGameObjectsWithTag("SantiSpawn")[0];
            PhotonNetwork.Instantiate(santiPrefab.name, spawn.transform.position, Quaternion.identity);
        }
        if(player == "Jose")
        {
            GameObject spawn = GameObject.FindGameObjectsWithTag("JoseSpawn")[0];
            PhotonNetwork.Instantiate(josePrefab.name, spawn.transform.position, Quaternion.identity);
        }
        
    }
}
