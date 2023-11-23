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

    private void Start()
    {
        if (firstV)
        {
            gameObject.GetComponent<VideoPlayer>().url = Application.streamingAssetsPath + "/" + "FinalCutsceneV2.mp4";
        }
        else if (secondV)
        {
            gameObject.GetComponent<VideoPlayer>().url = Application.streamingAssetsPath + "/" + "AnimacionLogo.mp4";
        }
    }

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
        if (currentTime > 38.0f && firstV && !firstPlaying)
        {
            gameObject.GetComponent<VideoPlayer>().Play();
            firstPlaying = true;
        }

        if (currentTime > 120.0f && secondV && !secondPlaying)
        {
            gameObject.GetComponent<VideoPlayer>().Play();
            secondPlaying = true;
        }
    }
}
