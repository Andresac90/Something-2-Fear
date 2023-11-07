using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoor : MonoBehaviour
{
    public int nSymbols;
    public string Code;
    private int iCode;
    private string[] Symbols = { "t", "s", "c", "p" };

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < nSymbols; i++)
        {
            iCode = Random.Range(0, Symbols.Length);
            Code = Symbols[iCode].ToString();
        }
    }
}
