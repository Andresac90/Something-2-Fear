using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class Down : MonoBehaviourPun
{
    private Camera Camera;
    private CharacterController CharController;
    private GameObject playerCam;
    private float currentTime = 0f;
    [SerializeField]
    private float deadTime;

    public bool isPlayerDowned;

    public void Start()
    {
        playerCam = transform.GetComponentInChildren<Camera>().gameObject;
        Camera = playerCam.GetComponent<Camera>();
        CharController = GetComponent<CharacterController>();
    }

    void Update()
    {
      
    }

    [PunRPC]
    public void SyncDowned()
    {
        if (isPlayerDowned) return;

        Debug.Log("downing");
        // look for santiController or JoseMovement
        if (name == "Santi(Clone)")
        {
            GetComponent<SantiController>().enabled = false;
            
        }
        else if (name == "Jose(Clone)")
        {
            GetComponent<JoseMovement>().enabled = false;
        }
        GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - 0.879f, transform.position.z);
        GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x - 90f, Quaternion.identity.y, Quaternion.identity.z);
        CharController.stepOffset = 0;
        CharController.height = 0.1f;
        CharController.center = new Vector3(0, 0, 0);
        Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x + 70f, Quaternion.identity.y, Quaternion.identity.z);
        GetComponentInChildren<PlayerLook>().enabled = false;
        isPlayerDowned = true;
    }

    [PunRPC]
    public void SyncRevive()
    {
        if (!isPlayerDowned) return;
        Debug.Log("reviving");
        if (name == "Santi(Clone)")
        {
            GetComponent<SantiController>().enabled = true;
            
        }
        else if (name == "Jose(Clone)")
        {
            GetComponent<JoseMovement>().enabled = true;
            
        }
        GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 0.879f, transform.position.z);
        GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
        CharController.height = 2f;
        CharController.center = new Vector3(0, 0, 0);
        CharController.stepOffset = 0.3f;
        Camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
        GetComponentInChildren<PlayerLook>().enabled = true;
        isPlayerDowned = false;
    }
    private void Die()
    {
        if(currentTime >= deadTime)
        {
            Debug.Log("eliminado");
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("LoseScreen"); // change to PhotonNetwork.LoadLevel("LoseScreen");
            // gameObject.SetActive(false);
        }
    }
}
