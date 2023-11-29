using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DeathScreenManager : MonoBehaviourPun
{
    [SerializeField]
    private CheckPointManager checkPointManager;
    [SerializeField]
    private GameObject deathScreen;

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void RespawnButton()
    {
        Time.timeScale = 1f;

        photonView.RPC("SyncRespawn", RpcTarget.All);
    }

    public void Die()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f; 
    }

    [PunRPC]
    public void SyncRespawn()
    {
        Time.timeScale = 1f;

        deathScreen.SetActive(false);
        checkPointManager.GoToCheckPoint();
    }
}

