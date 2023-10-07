using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject josePrefab;
    public GameObject santiPrefab;

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    void Start()
    {
        
        Vector2 randomPosition = new Vector3(-53, 2, 4);

        int playerNumber = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(playerNumber);

        if(playerNumber == 1)
        {
            PhotonNetwork.Instantiate(josePrefab.name, new Vector3(6, 1.04f, 5), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(santiPrefab.name, new Vector3(6, 1.04f, 14), Quaternion.identity);
        }
        
    }
}
