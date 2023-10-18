using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private Rejilla rejilla;

    public void Activation(bool state)
    {
        rejilla.OpenRejilla(state);
    }
}
