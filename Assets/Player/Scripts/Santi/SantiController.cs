using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;

using Photon.Pun;
using Unity.Properties;

public class SantiController : MonoBehaviour
{   

    [Header("Player Components")] // ------------------------------ Player Components ------------------------------ //
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private AudioListener AudioListener;
    [SerializeField]
    private GameObject Canvas;
    private InputMaster Controls;
    private CharacterController CharController;


    [Header("Movement")] // ------------------------------ Movement ------------------------------ //
    [SerializeField]
    private float Speed;
    private float OriginalSpeed;
    private float Gravity = -9.81f;
    private Vector2 YVel;
    private bool IsGrounded;
    [SerializeField]
    private Transform GroundCheck;
    [SerializeField]
    private float RadiusGround;
    [SerializeField]
    private LayerMask GroundMask; 


    [Header("Hiding System")] // ------------------------------ Hiding System ------------------------------ //
    [SerializeField]
    private GameObject HideText;
    [SerializeField]
    private GameObject StopHideText;

    [SerializeField]
    private float Range;
    private GameObject hideObject;
    public bool isHiding = false;


    public bool isPlayerInjected = false;
    private int controlsModifier = 1;
    private PhotonView PV;

    void Awake()
    {
        Controls = new InputMaster();
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        // If the player is not mine, disable the camera, audio listener and canvas
        Camera.enabled = PV.IsMine;
        AudioListener.enabled = PV.IsMine;
        Canvas.SetActive(PV.IsMine);

        CharController = GetComponent<CharacterController>();
        OriginalSpeed = Speed;
    }
    
    void Update()
    {
        if (!PV.IsMine) return;
        

        Movement();
        CheckInteract();

        if (isHiding)
            CheckRange();
    }

    private void CheckInteract()
    {
        bool InteractPressed = Controls.Player.Interact.triggered;
        if (isHiding)
        {
            UIPrompt("StopHide");

            if (InteractPressed)
            {
                StopHide();
                return;
            }
        }

        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Obstacle");
        Physics.Raycast(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward), out hit, 2.0f, mask);

        if (hit.transform == null){
            UIPrompt("");
            return;
        }

        UIPrompt(hit.transform.tag);

        if (!InteractPressed) return;
        switch (hit.transform.tag)
        {
            case "Hide":
                Hide(hit.transform.gameObject);
                break;
            default:
                break;
        }

    }

    private void CheckRange()
    {
        float distance = Vector3.Distance(transform.position, hideObject.transform.position);

        if (distance > Range)
        {
            hideObject.GetComponent<Locker>().enemy.GetComponent<PhotonView>().RPC("SyncInRange", RpcTarget.All, false);
        }
    }
    private void Hide(GameObject locker)
    {
        if (isHiding) return;
        isHiding = true;

        var hidePosition = locker.GetComponent<Locker>().HidePosition;  
        transform.position = new Vector3(hidePosition.position.x, hidePosition.position.y, hidePosition.position.z);

        hideObject = locker;

    }

    private void StopHide()
    {
        if (!isHiding) return;
        isHiding = false;

        var outPosition = hideObject.GetComponent<Locker>().OutPosition;
        transform.position = new Vector3(outPosition.position.x, outPosition.position.y, outPosition.position.z);

    }

    private void Movement()
    {
        if (isHiding) return;

        IsGrounded = Physics.CheckSphere(GroundCheck.position, RadiusGround, GroundMask);
        YVel.y += Gravity * Time.deltaTime;

        Vector2 movement = Controls.Player.Movement.ReadValue<Vector2>();

        bool isFalling = YVel.y < 0;

        if (IsGrounded && isFalling)
            YVel.y = 0;

        Vector3 newPos = (transform.right * movement.x + transform.forward * movement.y + transform.up * YVel.y) * controlsModifier;

        CharController.Move(newPos * Speed * Time.deltaTime);
    }

    private void UIPrompt(string text)
    {   
        if (text == "Hide")
        {
            HideText.SetActive(true);
            StopHideText.SetActive(false);
        }
        else if (text == "StopHide")
        {
            StopHideText.SetActive(true);
            HideText.SetActive(false);
        }
        else
        {
            HideText.SetActive(false);
            StopHideText.SetActive(false);
        }
    }

    public void SetInjected()
    {
        isPlayerInjected = true;
        controlsModifier = -1;
    }

    public void SetCured()
    {
        isPlayerInjected = false;
        controlsModifier = 1;
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }

}
