using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public string playerName;
    private bool isLocalPlayer = false;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    Player player;


    void Update()
    {
        if (!isLocalPlayer) return;
        Movement();
    }

    public void SetPlayerInfo(Player _player)
    {
        player = _player;
        UpdatePlayerItem(player);
    }

    private void Movement (){
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            if (playerName == ""){
                playerProperties["Player"] = "Jose";
            } 
            if (playerName == "Santi"){
                playerProperties["Player"] = "";
            }
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            if (playerName == ""){
                playerProperties["Player"] = "Santi";
            } 
            if (playerName == "Jose"){
                playerProperties["Player"] = "";
            }
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
    public void SetUp()
    {
        isLocalPlayer = true;
        player = PhotonNetwork.LocalPlayer;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer){
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player){
        if (player.CustomProperties.ContainsKey("Player")){
            playerName = (string)player.CustomProperties["Player"];
            if (playerName == "Jose"){
                transform.localPosition = new Vector3(-500, transform.localPosition.y, transform.localPosition.z);
            }
            if (playerName == ""){
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            }
            if (playerName == "Santi"){
                transform.localPosition = new Vector3(500, transform.localPosition.y, transform.localPosition.z);
            }
            playerProperties["Player"] = playerName;
        }
        else{
            playerProperties["Player"] = "";
        }
    }
}
