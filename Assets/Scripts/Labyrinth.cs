using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Labyrinth : MonoBehaviour
{
    private string terminalCode;
    private string doorCode;
    private bool doorActive;
    private GameObject Puzzle;
    private Puzzle Comprobations;

    public string minigameName;
    public GameObject[] doorsList;
    
    void Start()
    {
        Puzzle = GameObject.Find("Labyrinth MiniGame");
        

        Comprobations = Puzzle.GetComponent<Puzzle>();
        doorCode = doorsList[GameManager.Instance.doorNumber].GetComponent<Door>().doorCode;
        doorActive = doorsList[GameManager.Instance.doorNumber].GetComponent<Door>().doorState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSymbol(string symbol)
    {
        terminalCode += symbol;
        CheckPassword();
    }    

    public void CheckPassword()
    {
        if (terminalCode == doorCode)
        {
            doorsList[GameManager.Instance.doorNumber].GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
            doorsList[GameManager.Instance.doorNumber].GetComponent<Door>().doorState = true;
            doorsList[GameManager.Instance.doorNumber].GetComponent<Door>().OpenDoor();
            GameManager.Instance.doorNumber += 1;
            Comprobations.comprobations++;
            Comprobations.Completed();
            terminalCode = "";
        }
        else
        {
            for (int i = 0; i < terminalCode.Length; i++)
            {
                if (terminalCode[i] != doorCode[i])
                {
                    terminalCode = "";
                }
            }
        }
    }    
}
