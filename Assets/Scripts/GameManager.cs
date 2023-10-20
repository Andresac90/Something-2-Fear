using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public int numpadLevel = 1;
    public int lockpickLevel = 1;
    public bool win = false;

    public bool puzzle = false;
    public bool blinkJose;
    public bool blinkSanti;

    void Awake()
    {
        MakeSingleton();   
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
       SceneManager.LoadScene(0);
    }
}
