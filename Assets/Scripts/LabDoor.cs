using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LabDoor : MonoBehaviour
{
    public int nSymbols;
    public string Code = "";
    private int iCode;
    private string[] Symbols = { "t", "s", "c", "p" };
    public GameObject visions;
    public GameObject[] symbols;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < nSymbols; i++)
            {
                iCode = Random.Range(0, Symbols.Length);
                Code += Symbols[iCode].ToString();
            }
            this.GetComponent<PhotonView>().RPC("SetCodes", RpcTarget.All, iCode, Code);
        }
    }
    [PunRPC]
    void SetCodes(int iC, string C)
    {
        iCode = iC;
        Code = C;

        if (Code.Length == 1)
        {
            for (int i = 0; i < Code.Length; i++)
            {
                for (int j = 0; j < symbols.Length; j++)
                {
                    string codename = symbols[j].name[0].ToString();
                    if(codename == Code[i].ToString())
                    {
                        symbols[j].SetActive(true);
                    }
                }
            }
        } else if (Code.Length == 2)
        {
            int j = 0;
            for (int i = 0; i < Code.Length; i++)
            {
                while (j < symbols.Length)
                {
                    string codename = symbols[j].name;
                    if (codename == Code[i].ToString() + j.ToString())
                    {
                        symbols[j].SetActive(true);

                        if (j < 4) 
                        { 
                            j = 4; 
                        }
                        break;
                    }
                    j++;
                }
            }
        }
        else if (Code.Length == 3)
        {
            int j = 0;
            for (int i = 0; i < Code.Length; i++)
            {
                while (j < symbols.Length)
                {
                    string codename = symbols[j].name;
                    if (codename == Code[i].ToString() + j.ToString())
                    {
                        symbols[j].SetActive(true);

                        if (j < 4)
                        {
                            j = 4;
                        }
                        if ((j > 3 && j < 8) && i == 1)
                        {
                            j = 8;
                        }
                        break;
                    }
                    j++;
                }
            }
        }
    }
}
