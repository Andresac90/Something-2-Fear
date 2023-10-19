using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down : MonoBehaviour
{
    private Camera camera;
    private CharacterController CharController;
    private GameObject playerCam;
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

    public void Update()
    {
        Downed();
    }

    private void Downed()
    {
        if(isPlayerDowned && wasntDowned)
        {
            this.GetComponent<SantiController>().enabled = false;
            CharController.height = 0.5f;
            CharController.center = new Vector3(0, -0.7f, 0);
            camera.transform.position = new Vector3(transform.position.x, transform.position.y - 0.43f, transform.position.z - 0.225f);
            this.GetComponentInChildren<PlayerLook>().enabled = false;
            wasDowned = true;
            wasntDowned = false;
        }
        else if(!isPlayerDowned && wasDowned)
        {
            this.GetComponent<SantiController>().enabled = true;
            CharController.height = 2f;
            CharController.center = new Vector3(0, 0, 0);
            camera.transform.position = new Vector3(transform.position.x, transform.position.y + 1.014f, transform.position.z + 0.096f);
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
