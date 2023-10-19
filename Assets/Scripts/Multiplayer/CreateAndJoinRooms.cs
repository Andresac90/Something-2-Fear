using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

using TMPro;
using Photon.Realtime;
using Unity.VisualScripting;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    [SerializeField]
    private GameObject roomPanel;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private TextMeshProUGUI roomNameText;

    [SerializeField]
    private PopUpManager PopUp;

    public List<PlayerItem> items = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public void CreateRoom()
    {
        if (createInput.text != ""){
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions(){
                MaxPlayers = 2
            });
        }  
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);

    }

    public override void OnJoinedRoom()
    {
        // PhotonNetwork.LoadLevel("Main");
        // PhotonNetwork.LoadLevel("Multijugador");
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name;

        UpdatePlayerList();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopUp.SendPopUp();        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    void UpdatePlayerList(){
        foreach (PlayerItem item in items)
        {
            Destroy(item.gameObject);
        }

        items.Clear();

        if (PhotonNetwork.InRoom == false) return;


        Vector3 player1pos = new Vector3(0, 60, 0);
        Vector3 player2pos = new Vector3(0, -340, 0);
        int i = 0;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (i == 0){
                PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
                newPlayerItem.transform.localPosition = player1pos;
                items.Add(newPlayerItem);
            }
            else{
                PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
                newPlayerItem.transform.localPosition = player2pos;
                items.Add(newPlayerItem);
            }
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                items[i].SetUp();
            }

            i++;
            
        }
    }

}
