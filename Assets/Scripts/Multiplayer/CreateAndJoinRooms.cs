using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    [SerializeField]
    private PopUpManager PopUp;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        // PhotonNetwork.LoadLevel("Main");
        PhotonNetwork.LoadLevel("Multijugador");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopUp.SendPopUp();        
    }



}
