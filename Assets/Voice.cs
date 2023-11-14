using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;

public class Voice : MonoBehaviour
{
    [SerializeField]
    private GameObject MicOn;
    [SerializeField]
    private GameObject MicOff;
    private Recorder recorder;
    private InputMaster Controls;

    private bool isMuted = false;

    void Awake()
    {
        Controls = new InputMaster();
    }
    void Start()
    {
        recorder = GetComponent<Recorder>();

        
    }
    
    void Update()
    {
        if (Controls.Player.PushToTalk.ReadValue<float>() > 0f)
        {
            bool unMute = isMuted ? false : true;
            
            if (unMute)
            {
                MicOn.SetActive(true);
                MicOff.SetActive(false);
            }
            else
            {
                MicOn.SetActive(false);
                MicOff.SetActive(true);
            }

        }
        else
        {
            recorder.TransmitEnabled = false;
            MicOn.SetActive(false);
            MicOff.SetActive(true); 
        }

        // check if Mute is pressed
        if (Controls.Player.Mute.ReadValue<float>() > 0f)
        {
            isMuted = !isMuted;
            recorder.TransmitEnabled = !isMuted;

            if (isMuted)
            {
                MicOn.SetActive(false);
                MicOff.SetActive(true);
            }
            else
            {
                MicOn.SetActive(true);
                MicOff.SetActive(false);
            }
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
