using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Labyrinth : MonoBehaviour
{
    private string terminalCode;
    private string[] Passcodes; //lista de passcodes
    private GameObject Puzzle; //terminal
    private Puzzle Comprobations; //iteraciones de puertas
    public GameObject[] doorsList; //lista de puertas
    
    void Start()
    {
        doorsList = new GameObject[5];
        Puzzle = GameObject.Find("Labyrinth MiniGame");
        Comprobations = Puzzle.GetComponent<Puzzle>();
        for (int i = 0; i < doorsList.Length; i++)
        {
            doorsList[i] = GameObject.Find("L_Door" + (i+1).ToString()); 
            Passcodes[i] = doorsList[i].GetComponent<LabDoor>().Code;
        }
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
        if (terminalCode == Passcodes[GameManager.Instance.doorNumber])
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
                if (terminalCode[i] != Passcodes[GameManager.Instance.doorNumber][i])
                {
                    terminalCode = "";
                }
            }
        }
    }    
}
