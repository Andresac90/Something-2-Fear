using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private bool TwoPlayers = true;

    [SerializeField]
    private bool Event = false;
    [SerializeField]
    private bool WinScreen = false;
    [SerializeField]
    private GameObject[] Objects;
    private bool RunOnce = false;

    public bool PlayersNear = false;
    public bool PlayerNear = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InteractPlayers();
        InteractPlayer();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerJose") && collision.CompareTag("PlayerSanti") && TwoPlayers)
        {
            PlayersNear = true;
        }

        if (collision.CompareTag("PlayerJose") && !TwoPlayers)
        {
            PlayerNear = true;
        }

        if (collision.CompareTag("PlayerSanti") && !TwoPlayers)
        {
            PlayerNear = true;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("PlayerJose") && TwoPlayers)
        {
            PlayersNear = false;
        }
        
        if (collision.CompareTag("PlayerJose") && !TwoPlayers)
        {
            PlayerNear = false;
        }

        if (collision.CompareTag("PlayerSanti") && TwoPlayers)
        {
            PlayersNear = false;
        }

        if (collision.CompareTag("PlayerSanti") && !TwoPlayers)
        {
            PlayerNear = false;
        }
    }

    private bool ArePlayersOnTrigger()
    {
        return PlayersNear;
    }

    private bool IsPlayerOnTrigger()
    {
        return PlayerNear;
    }

    void InteractPlayers()
    {
        if (ArePlayersOnTrigger() && TwoPlayers && !RunOnce && Event)
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                bool Value = Objects[i].activeInHierarchy;
                Console.WriteLine(Objects.Length);
                Objects[i].SetActive(!Value);
                RunOnce = true;
            }
            Destroy(this);
        }
        else if (ArePlayersOnTrigger() && TwoPlayers && !RunOnce && WinScreen)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    void InteractPlayer()
    {
        if (IsPlayerOnTrigger() && !TwoPlayers && !RunOnce && Event)
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                bool Value = Objects[i].activeInHierarchy;
                Console.WriteLine(Objects.Length);
                Objects[i].SetActive(!Value);
                RunOnce = true;
            }
            Destroy(this);
        }
        
    }
}
