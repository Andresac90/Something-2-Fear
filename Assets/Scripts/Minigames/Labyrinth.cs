using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Labyrinth : MonoBehaviour
{
    private string terminalCode;
    public string[] Passcodes; //lista de passcodes
    private GameObject Puzzle; //terminal
    private Puzzle Comprobations; //iteraciones de puertas
    public GameObject[] doorsList; //lista de puertas
    public GameObject Nina;

    void Start()
    {
        doorsList = new GameObject[5];
        Passcodes = new string[5];
        Puzzle = GameObject.Find("Labyrinth MiniGame");
        Nina = GameObject.Find("Nina");
        Comprobations = Puzzle.GetComponent<Puzzle>();
        for (int i = 0; i < doorsList.Length; i++)
        {
            doorsList[i] = GameObject.Find("L_Door" + (i+1).ToString()); 
            if(i > 0)
            {
                Passcodes[i] = Passcodes[i-1];
            }
            Passcodes[i] += doorsList[i].GetComponent<LabDoor>().Code;
            Debug.Log(Passcodes[i]);
        }
    }

    public void AddSymbol(string symbol)
    {
        if (GameManager.Instance.JoseInsideLab)
        {
            GameManager.Instance.Click.Play();
            terminalCode += symbol;
            CheckPassword();
        }
    }    

    public void CheckPassword()
    {
        if (terminalCode == Passcodes[GameManager.Instance.doorNumber])
        {
            GameManager.Instance.Success.Play();
            doorsList[GameManager.Instance.doorNumber].GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
            doorsList[GameManager.Instance.doorNumber].GetComponent<Door>().doorState = true;
            doorsList[GameManager.Instance.doorNumber].GetComponent<Door>().OpenDoor();
            Nina.GetComponent<PhotonView>().RPC("InvertPlayers",RpcTarget.All);
            GameManager.Instance.doorNumber += 1;
            Comprobations.comprobations++;
            if (Comprobations.comprobations == 5)
            {
                Destroy(gameObject);
            }
            Comprobations.Completed();
            terminalCode = "";
        }
        else
        {
            for (int i = 0; i < terminalCode.Length; i++)
            {
                if (terminalCode[i] != Passcodes[GameManager.Instance.doorNumber][i])
                {
                    GameManager.Instance.Buzzer.Play();
                    terminalCode = "";
                }
            }
        }
    }    
}
