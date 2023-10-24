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

    private void Update()
    {
        if (PhotonNetwork.InRoom == false) return;
        checkCharacters();
    }

    void UpdatePlayerList(){
        foreach (PlayerItem item in items)
        {
            Destroy(item.gameObject);
        }

        items.Clear();

        foreach (PlayerItem item in items)
        {
            Destroy(item.gameObject);
        }

        GameObject playerObject;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1){
            playerObject = PhotonNetwork.Instantiate("PlayerItem", Vector3.zero, Quaternion.identity);
            playerObject.GetComponent<PhotonView>().RPC("SyncParent", RpcTarget.All, false);
        }
        else{
            playerObject = PhotonNetwork.Instantiate("PlayerItem", Vector3.zero, Quaternion.identity);
            playerObject.GetComponent<PhotonView>().RPC("SyncParent", RpcTarget.All, true);
        }

        items.Add(playerObject.GetComponent<PlayerItem>());
    }
    

    public void checkCharacters()
    {
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
