using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private bool TwoPlayers = true;

    [SerializeField]
    private bool Event0 = false;
    [SerializeField]
    private bool Event1 = false;
    [SerializeField]
    private GameObject[] Objects;

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

        if (collision.CompareTag("PlayerJose") && collision.CompareTag("PlayerSanti") && !TwoPlayers)
        {
            PlayerNear = true;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("PlayerJose") && collision.CompareTag("PlayerSanti") && TwoPlayers)
        {
            PlayersNear = false;
        }
        
        if (collision.CompareTag("PlayerJose") && collision.CompareTag("PlayerSanti") && !TwoPlayers)
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
        if (ArePlayersOnTrigger() && TwoPlayers)
        {
            for (int i = 0; i <= Objects.Length; i++)
            {
                bool Value = Objects[i].activeInHierarchy;
                Objects[i].SetActive(!Value);
            }
        }
    }

    void InteractPlayer()
    {
        if (IsPlayerOnTrigger() && !TwoPlayers)
        {
            for (int i = 0; i <= Objects.Length; i++)
            {
                bool Value = Objects[i].activeInHierarchy;
                Objects[i].SetActive(!Value);
            }
        }
    }
}
