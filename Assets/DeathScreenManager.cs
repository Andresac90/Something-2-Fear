using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreenManager : MonoBehaviour
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
        deathScreen.SetActive(false);
        checkPointManager.GoToCheckPoint();
    }

    public void Die()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f; 
    }
}

