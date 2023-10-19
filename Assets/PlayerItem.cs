using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public void SetUp()
    {
        GetComponent<PlayerItemController>().enabled = true;
    }
}
