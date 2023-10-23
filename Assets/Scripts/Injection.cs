using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Injection : MonoBehaviour
{
    private Camera camera;
    private CharacterController CharController;
    private GameObject playerCam;
    private bool wasInjected = false;
    private bool wasntInjected = true;
    private float currentTime = 0f;
    private bool santiInjected = false;
    private bool joseInjected = false;

    [SerializeField]
    private float downTime = 20f;

    [SerializeField]
    private PhotonView JosePV;
    private PhotonView SantiPV;

    public bool isPlayerInjected;

    public bool invertHorizontalInput = false;
    public bool invertVerticalInput = false;
    public bool invertCameraX = false;
    public bool invertCameraY = false;

    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = Input.GetAxis("Mouse Y");

    public float moveSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = this.transform.GetChild(0).gameObject;
        camera = playerCam.GetComponent<Camera>();
        CharController = GetComponent<CharacterController>();
    }

    [PunRPC]
    void updateInjected(bool status)
    {
        isPlayerInjected = status;
    }

    void Update()
    {
        if (isPlayerInjected)
        {
            Injected(isPlayerInjected);
        }
    }

    public void Injected(bool isPlayerCaught)
    {
        isPlayerInjected = isPlayerCaught;

        if (this.name == "Santi(Clone)")
        {
            if (isPlayerInjected && wasntInjected)
            {
                horizontalInput = -horizontalInput;
                verticalInput = -verticalInput;
                mouseX = -mouseX;
                mouseY = -mouseY;

                Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
                CharController.Move(moveDirection * moveSpeed * Time.deltaTime);

                wasInjected = true;
                wasntInjected = false;
                santiInjected = true;
            }
            else if (!isPlayerInjected && wasInjected)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
                CharController.Move(moveDirection * moveSpeed * Time.deltaTime);

                wasInjected = false;
                wasntInjected = true;
                santiInjected = false;
                currentTime = 0f;
            }
            else if (!wasntInjected)
            {
                currentTime += Time.deltaTime;
                SantiPV.RPC("updateDowned", RpcTarget.All, isPlayerCaught);
            }
        }
        else if (this.name == "Jose(Clone)")
        {
            if (isPlayerInjected && wasntInjected)
            {
                horizontalInput = -horizontalInput;
                verticalInput = -verticalInput;
                mouseX = -mouseX;
                mouseY = -mouseY;

                Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
                CharController.Move(moveDirection * moveSpeed * Time.deltaTime);

                wasInjected = true;
                wasntInjected = false;
                joseInjected = true;
            }
            else if (!isPlayerInjected && wasInjected)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
                CharController.Move(moveDirection * moveSpeed * Time.deltaTime);

                wasInjected = false;
                wasntInjected = true;
                joseInjected = false;
                currentTime = 0f;
            }
            else if (!wasntInjected)
            {
                currentTime += Time.deltaTime;
                JosePV.RPC("updateDowned", RpcTarget.All, isPlayerCaught);
            }
        }
    }
}
