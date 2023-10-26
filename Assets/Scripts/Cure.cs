using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure : MonoBehaviour
{
    private float currentTime = 0f;
    private float cureTime = 5f;
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
            player.GetComponent<Injection>().Cured();
            isInteractPressed = false;
        }
    }
}
