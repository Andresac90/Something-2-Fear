using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;

using Photon.Pun;
using Unity.Properties;

public class SantiController : MonoBehaviour
{   
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private AudioListener AudioListener;
    [SerializeField]
    private GameObject Canvas;
    private Vector2 Move;
    private Vector2 YVel;
    public float Speed;
    private float OriginalSpeed;
    private Transform playerCam;
    private InputAction movementAction;
    private InputAction lookAction;
    public float moveSpeed = 5.0f;
    public float lookSensitivity = 2.0f;
    public bool isPlayerInjected = false;
    private float rotationX = 0;

    [SerializeField]
    private float Gravity = -9.81f;

    private InputMaster Controls;
    private CharacterController CharController;
    private PlayerLook look;

    // public GameObject Task;
    //Grounded
    public bool IsGrounded;
    //Head Check in editor
    [SerializeField]
    private Transform GroundCheck;
    public float RadiusGround;
    public LayerMask GroundMask;

    private PhotonView PV;

    void Awake()
    {
        Controls = new InputMaster();
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        Camera.enabled = PV.IsMine;
        AudioListener.enabled = PV.IsMine;
        Canvas.SetActive(PV.IsMine);

        CharController = GetComponent<CharacterController>();
        look = GetComponent<PlayerLook>();
        OriginalSpeed = Speed;
        GameObject.Find("Locker").GetComponent<HidingSystem>().ActivateSanti();
        GameObject.Find("Locker2").GetComponent<HidingSystem>().ActivateSanti();
    }
    public void SetInjected(bool injected)
    {
        isPlayerInjected = injected;
    }
    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;
        if (isPlayerInjected)
        {
            InvertControls();
        }
        else
        {
            Movement();
        }
        // Camera.transform.position = new Vector3(transform.position.x, Camera.transform.position.y, transform.position.z);
        // Interact();
    }

    private void Movement()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, RadiusGround, GroundMask);
        Move = Controls.Player.Movement.ReadValue<Vector2>();

        if (IsGrounded && YVel.y < 0)
        {
            YVel.y = 0;
        }

        Vector3 MovementZ = (transform.right * Move.x + transform.forward * Move.y);
        YVel.y += Gravity * Time.deltaTime;
        CharController.Move(MovementZ * Speed * Time.deltaTime);
        CharController.Move(YVel * Speed * Time.deltaTime);
    }

    // private void Interact()
    // {
    //     bool IsInteractPressed = Controls.Player.Interact.ReadValue<float>() > 0f;
    //     if(isTaskActive() && IsInteractPressed)
    //     {
    //         Debug.Log("Iniciado");
    //         Instantiate(Task);
    //     }
    // }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }

    private void InvertControls()
    {
        Vector2 invertedMovementInput = Controls.Player.Movement.ReadValue<Vector2>() * -1;
        Vector2 invertedLookInput = Controls.Player.Look.ReadValue<Vector2>() * -1;

        // Move the player with inverted controls
        Vector3 moveDirection = transform.TransformDirection(new Vector3(invertedMovementInput.x, 0, invertedMovementInput.y));
        CharController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate the camera with inverted controls
        //look.SetInvert(true);
    }

}
