using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour
{
    public float currentTime;
    public bool firstV;
    public bool secondV;
    public bool firstPlaying = false;
    public bool secondPlaying = false;

    void Update()
    {
        TimeS();
        checkTime();
    }

    void TimeS()
    {
        if (GameManager.Instance.ending.time > 0)
        {
            currentTime += Time.deltaTime;
        }

        if (GameManager.Instance.ending.time == GameManager.Instance.ending.duration)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    void checkTime()
    {
        if (currentTime > 39.0f && firstV && !firstPlaying)
        {
            gameObject.GetComponent<VideoPlayer>().Play();
            firstPlaying = true;
        }

        if (currentTime > 199.0f && secondV && !secondPlaying)
        {
            gameObject.GetComponent<VideoPlayer>().Play();
            secondPlaying = true;
        }
    }
}
