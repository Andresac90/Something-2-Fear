using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cure : MonoBehaviourPun
{
    private float currentTime = 0f;
    private float cureTime = 4f;
    private bool isInteractPressed;

    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        checkTime();
    }

    public void updateCure(bool pressed, GameObject playerPressing)
    {
        isInteractPressed = pressed;
        player = playerPressing;
    }

    public void checkTime()
    {
        if (isInteractPressed)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0f;
        }

        if (currentTime > cureTime)
        {
            player.GetComponent<PhotonView>().RPC("updateCured", RpcTarget.All);
            isInteractPressed = false;
        }
    }
}
