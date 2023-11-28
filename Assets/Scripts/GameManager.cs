using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Playables;
using Photon.Realtime;
using Unity.VisualScripting;

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
    public bool JoseInsideLab = false;
    public bool audioH;

    public bool Key1 = false;
    public bool Key2 = false;
    public bool Key3 = false;

    public bool Object1 = false;
    public bool Object2 = false;
    public bool Object3 = false;
    public bool InjectionSpawn = false;
    public bool Injection = false;

    public AudioSource Buzzer;
    public AudioSource Success;
    public AudioSource Click;
    public AudioSource Keys;
    public AudioSource Door;
    public AudioSource Switch;
    public AudioSource PascualitaJumpscare;
    public AudioSource AudioHospital;
    public AudioSource NurseScream;
    public AudioSource Ambience1;
    public AudioSource Ambience2;
    public AudioSource PascualaLaugh;
    public AudioSource Healing;
    public AudioSource Footsteps;
    public AudioSource NurseRunning;
    public AudioSource DoorLocked;
    public AudioSource Whispers;

    public PlayableDirector ending;

    public GameObject endingCanva;
    public GameObject Jose;
    public GameObject Santi;

    [SerializeField]
    private SpawnInjection InjectionScript;

    public bool tutorialJump = false;
    public bool tutorialRun = false;
    public bool tutorialCrouch = false;

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

    public void SpawnInjectionOnline()
    {
        if (InjectionSpawn)
        {
            InjectionScript.GetComponent<SpawnInjection>().Spawn();
        }
    }
    [PunRPC]
    public void EndingCutscene()
    {
        Jose = GameObject.Find("Jose(Clone)");
        Santi = GameObject.Find("Santi(Clone)");
        Jose.GetComponent<JoseMovement>().enabled = false;
        Jose.GetComponentInChildren<PlayerLook>().enabled = false;
        Jose.GetComponentInChildren<Camera>().enabled = false;
        Jose.GetComponentInChildren<Canvas>().enabled = false;
        Santi.GetComponent<SantiController>().enabled = false;
        Santi.GetComponentInChildren<PlayerLook>().enabled = false;
        Santi.GetComponentInChildren<Camera>().enabled = false;
        Santi.GetComponentInChildren<Canvas>().enabled = false;
        Instantiate(endingCanva);
        ending.Play();
    }
}
