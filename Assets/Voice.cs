using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;

public class Voice : MonoBehaviour
{
    private Recorder recorder;
    private InputMaster Controls;
    void Start()
    {
        recorder = GetComponent<Recorder>();
        Controls = new InputMaster();   
    }
    
    void Update()
    {
        if (Controls.Player.PushToTalk.ReadValue<float>() > 0f)
        {
            recorder.TransmitEnabled = true;
        }
        else
        {
            recorder.TransmitEnabled = false;
        }
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
