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
        foreach (var checkPoint in PassedCheckPoints)
        {
            GameManager.Instance.Key1 = checkPoint.key == CheckPoint.Key.Key1;
            GameManager.Instance.Key2 = checkPoint.key == CheckPoint.Key.Key2;
            GameManager.Instance.Key3 = checkPoint.key == CheckPoint.Key.Key3;
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

        var santi = GameObject.Find("Santi(Clone)");
        var jose = GameObject.Find("Jose(Clone)");
        santi.transform.position = CurrentCheckPoint.transform.position;
        jose.transform.position = CurrentCheckPoint.transform.position;

        if (santi.GetComponent<Down>().isPlayerDowned)
        {
            santi.GetComponent<Revive>().RevivePlayer(santi);
        }
        if (jose.GetComponent<Down>().isPlayerDowned)
        {
            jose.GetComponent<Revive>().RevivePlayer(jose);
        }
    }

    public void ResetPascualita()
    {
        //enable or disable
        PascualitaDummy.SetActive(!CurrentCheckPoint.PascualitaIsOn);
        Pascualita.SetActive(CurrentCheckPoint.PascualitaIsOn);
    }
}
