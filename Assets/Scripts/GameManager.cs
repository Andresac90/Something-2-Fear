using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    private List<GameObject> joinedPlayers = new List<GameObject>();
    private int requiredPlayers = 2;
    public delegate void PlayersJoinedEventHandler(PhotonView player1PV, PhotonView player2PV);
    public static event PlayersJoinedEventHandler OnPlayersJoined;

    public static GameManager Instance;
    public int numpadLevel = 1;
    public int lockpickLevel = 1;
    public int doorNumber = 0;
    public bool win = false;

    public bool puzzle = false;
    public bool blinkJose;
    public bool blinkSanti;

    public bool Key1 = false;
    public bool Key2 = false;
    public bool Key3 = false;

    public AudioSource Buzzer;
    public AudioSource Success;
    public AudioSource Click;
    public AudioSource Keys;
    public AudioSource Door;
    public AudioSource PascualitaJumpscare;

    void Awake()
    {
        MakeSingleton();   
        PlayerManager.OnPlayerJoined += HandlePlayerJoined;
    }

    private void HandlePlayerJoined(GameObject playerObject)
    {
        if (!joinedPlayers.Contains(playerObject))
        {
            joinedPlayers.Add(playerObject);
            Debug.Log(playerObject.name + " se ha unido.");
        }

        if (joinedPlayers.Count >= requiredPlayers)
        {
            Debug.Log("Se han unido suficientes jugadores para realizar una acción.");
            PhotonView player1PV = joinedPlayers[0].GetComponent<PhotonView>();
            PhotonView player2PV = joinedPlayers[1].GetComponent<PhotonView>();

            if (OnPlayersJoined != null)
            {
                OnPlayersJoined(player1PV, player2PV);
            }
        }
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
