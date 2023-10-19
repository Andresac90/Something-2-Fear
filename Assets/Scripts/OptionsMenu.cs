using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Photon.Pun;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetSensitivity(float sensitivity)
    {
        Camera playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PlayerLook playerLook = playerCamera.GetComponent<PlayerLook>();
        playerLook.SetSens(sensitivity);

    }

    public void SetVolume(float volume)
    {
        // set volume with logarithmic scale
        volume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("volume", volume);
    }

    public void QuitGame (){
        PhotonNetwork.LeaveRoom();
    }
}
