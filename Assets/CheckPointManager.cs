using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CheckPointManager : MonoBehaviourPun
{    
    [SerializeField]
    private CheckPoint CurrentCheckPoint;
    [SerializeField]
    private List<CheckPoint> PassedCheckPoints;

    [SerializeField]
    private GameObject PascualitaDummy;
    [SerializeField]
    private GameObject Pascualita;

    [PunRPC]
    public void SetCheckPoint(int checkPointViewID)
    {
        var checkPoint = PhotonView.Find(checkPointViewID).GetComponent<CheckPoint>();
        CurrentCheckPoint = checkPoint;
        PassedCheckPoints.Add(checkPoint);
    }

    [PunRPC]
    public void GoToCheckPoint()
    {
        ResetKeys();
        ResetDoors();
        ResetPlayers();
        ResetPascualita();
    }  

    public void ResetKeys()
    {
        foreach (var key in CurrentCheckPoint.FoundKeys)
        {
            key.SetActive(false);
            // TODO: add key to player inventory
        }
    }

    public void ResetDoors()
    {

        foreach (var checkPoint in PassedCheckPoints)
        {
            foreach (var door in checkPoint.OpenedDoors)
            {
                door.GetComponent<PhotonView>().RPC("SyncDoor", RpcTarget.All, true);
            }
        }
    }

    public void ResetPlayers()
    {
        GameObject.Find("Santi(Clone)").transform.position = CurrentCheckPoint.transform.position;
        GameObject.Find("Jose(Clone)").transform.position = CurrentCheckPoint.transform.position;
    }

    public void ResetPascualita()
    {
        //enable or disable
        PascualitaDummy.SetActive(!CurrentCheckPoint.PascualitaIsOn);
        Pascualita.SetActive(CurrentCheckPoint.PascualitaIsOn);
    }
}
