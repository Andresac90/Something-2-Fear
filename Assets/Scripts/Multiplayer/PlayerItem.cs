using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.TextCore.Text;

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
    public void SetUp()
    {
        isLocalPlayer = true;
        player = PhotonNetwork.LocalPlayer;
    }

    private void SetCharacterSelection(string characterName)
    {
        playerName = characterName;
        playerProperties["Player"] = playerName;
        photonView.RPC("SyncCharacterSelection", RpcTarget.All, characterName);
    }

    [PunRPC]
    public void SyncCharacterSelection(string _playerName){
        playerName = _playerName;
        if (playerName == "Jose"){
            Debug.Log("Jose");
            transform.localPosition = new Vector3(-500, transform.localPosition.y, transform.localPosition.z);
        }
        if (playerName == ""){
            Debug.Log("");
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
        if (playerName == "Santi"){
            Debug.Log("Santi");
            transform.localPosition = new Vector3(500, transform.localPosition.y, transform.localPosition.z);
        }
        Debug.Log(isLocalPlayer);
    }
}
