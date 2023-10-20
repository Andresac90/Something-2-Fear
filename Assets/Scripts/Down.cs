using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down : MonoBehaviour
{
    private Camera camera;
    private CharacterController CharController;
    private GameObject playerCam;
    private GameObject pascualita;
    private AIControl pascualitaScript;
    private bool wasDowned = false;
    private bool wasntDowned = true;
    private float currentTime = 0f;
    
    [SerializeField]
    private float deadTime = 10f;

    public bool isPlayerDowned = false;

    public void Start()
    {
        playerCam = this.transform.GetChild(0).gameObject;
        camera = playerCam.GetComponent<Camera>();
        CharController = GetComponent<CharacterController>();
    }

    public void Downed(bool isPlayerCaught)
    {
        isPlayerDowned = isPlayerCaught;
        if(isPlayerDowned && wasntDowned)
        {
            this.GetComponent<SantiController>().enabled = false;
            this.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y - 0.879f, transform.position.z);
            this.GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x - 90f, Quaternion.identity.y, Quaternion.identity.z);
            CharController.stepOffset = 0;
            CharController.height = 0.1f;
            CharController.center = new Vector3(0, 0, 0);
            camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x + 70f, Quaternion.identity.y, Quaternion.identity.z);
            this.GetComponentInChildren<PlayerLook>().enabled = false;
            wasDowned = true;
            wasntDowned = false;
        }
        else if(!isPlayerDowned && wasDowned)
        {
            this.GetComponent<SantiController>().enabled = true;
            this.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 0.879f, transform.position.z);
            this.GetComponent<Transform>().rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
            CharController.height = 2f;
            CharController.center = new Vector3(0, 0, 0);
            CharController.stepOffset = 0.3f;
            camera.transform.rotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z);
            this.GetComponentInChildren<PlayerLook>().enabled = true;
            wasDowned = false;
            wasntDowned = true;
            currentTime = 0f;
        }
        else if(!wasntDowned)
        {
            currentTime += Time.deltaTime;
            Die();
        }
    }
    
    private void Die()
    {
        if(currentTime >= deadTime)
        {
            gameObject.SetActive(false);
        }
    }
}
