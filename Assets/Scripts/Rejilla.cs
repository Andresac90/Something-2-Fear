using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rejilla : MonoBehaviour
{
    [SerializeField]
    private Animator rejilla;
    
    public void OpenRejilla(bool rejillaState)
    {
        if(rejillaState)
        {
            rejilla.SetBool("activate", true);
            rejilla.SetBool("rejillaActivate", true);
            Debug.Log("true");
        }
        else if(!rejillaState)
        {
            rejilla.SetBool("rejillaActivate", false);
            rejilla.SetBool("activate", false);
            Debug.Log("false");
        }
    }
}
