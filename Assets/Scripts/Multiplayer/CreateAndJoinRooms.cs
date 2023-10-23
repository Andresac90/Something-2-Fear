using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

using TMPro;
using Photon.Realtime;
using Unity.VisualScripting;
using JetBrains.Annotations;

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

    public ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    
    public GameObject playButton;

    public void CreateRoom()
    {
        if (createInput.text != ""){
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions(){
                MaxPlayers = 2,
                BroadcastPropsChangeToAll = true,
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
            Vector3 pos = new Vector3(0, 0, 0);
            if (i == 0){
                pos = player1pos;
            }
            else{
                pos = player2pos;
            }
            PlayerItem item = Instantiate(playerItemPrefab, playerItemParent);
            item.transform.localPosition = pos;
            item.SetPlayerInfo(player.Value);
            items.Add(item);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                item.SetUp();
            }

            i++;
            
        }
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom == false) return;
        PlayerItem[] playerItems = FindObjectsOfType<PlayerItem>();

        bool playerIsJose = false;
        bool playerIsSanti = false;

        foreach (PlayerItem item in playerItems)
        {
            if (item.playerName == "Jose")
            {
                playerIsJose = true;
            }
            if (item.playerName == "Santi")
            {
                playerIsSanti = true;
            }
        }

        bool roomIsFull = PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
        bool gameIsReady = playerIsJose && playerIsSanti && roomIsFull;

        if (PhotonNetwork.IsMasterClient && gameIsReady)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void Play()
    {
        PhotonNetwork.LoadLevel("Main");
    }
}
