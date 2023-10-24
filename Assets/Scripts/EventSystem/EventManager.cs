using Photon.Pun;
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
            Console.WriteLine("PlayersNear True");
            PlayersNear = true;
        }
        if (collision.CompareTag("PlayerJose") || collision.CompareTag("PlayerSanti") && !TwoPlayers)
        {
            Console.WriteLine("PlayerNear True");
            PlayerNear = true;
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
            Console.WriteLine("Players IN");
            for (int i = 0; i < Objects.Length; i++)
            {
                //bool Value = Objects[i].activeInHierarchy;
                Console.WriteLine(Objects.Length);
                Objects[i].GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Objects[i]);
                RunOnce = true;
            }
            //AI.SetActive(true);
            Destroy(this);
        }
        else if (ArePlayersOnTrigger() && TwoPlayers && !RunOnce && WinScreen)
        {
            PhotonNetwork.LoadLevel("WinScreen");
        }
    }

    void InteractPlayer()
    {
        if (IsPlayerOnTrigger() && !TwoPlayers && !RunOnce && Event)
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                //bool Value = Objects[i].activeInHierarchy;
                Console.WriteLine(Objects.Length);
                Objects[i].GetComponent<PhotonView>().RPC("ChangeObject", RpcTarget.All, Objects[i]);
                RunOnce = true;
            }
            //AI.SetActive(true);
            Destroy(this);
        }
        else if (ArePlayersOnTrigger() && TwoPlayers && !RunOnce && WinScreen)
        {
            SceneManager.LoadScene("WinScreen");
        }

    }
}
