using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public string playerName;
    private bool isLocalPlayer = false;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();


    void Update()
    {
        if (!photonView.IsMine) return;
        Movement();
    }

    private void Movement (){
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            if (playerName == ""){
                SetCharacterSelection("Jose");
            } 
            if (playerName == "Santi"){
                SetCharacterSelection("");
            }
            photonView.RPC("SyncCharacterSelection", RpcTarget.All, (string)playerProperties["Player"]);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            if (playerName == ""){
                SetCharacterSelection("Santi");
            } 
            if (playerName == "Jose"){
                SetCharacterSelection("");
            }
            photonView.RPC("SyncCharacterSelection", RpcTarget.All, (string)playerProperties["Player"]);
        }
    }

    private void SetCharacterSelection(string characterName)
    {
        playerName = characterName;
        playerProperties["Player"] = playerName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        photonView.RPC("SyncCharacterSelection", RpcTarget.All, characterName);
    }

    [PunRPC]
    public void SyncCharacterSelection(string _playerName){
        playerName = _playerName;
        if (playerName == "Jose"){
            transform.localPosition = new Vector3(-500, transform.localPosition.y, transform.localPosition.z);
        }
        if (playerName == ""){
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
        if (playerName == "Santi"){
            transform.localPosition = new Vector3(500, transform.localPosition.y, transform.localPosition.z);
        }
    }

    [PunRPC]
    public void SyncParent(bool playerNumber){
        transform.SetParent(GameObject.Find("LobbyPanel").transform);

        if (playerNumber == true){
            transform.localPosition = new Vector3(0, -340, 0);
        }
        else{
            transform.localPosition = new Vector3(0, 60, 0);
        }
    }
}
