using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public void SendPopUp()
    {
        this.gameObject.SetActive(true);
        // Disable the pop up after 3 seconds
        Invoke("DisablePopUp", 3f);
    }

    public void DisablePopUp()
    {
        this.gameObject.SetActive(false);
    }
}
