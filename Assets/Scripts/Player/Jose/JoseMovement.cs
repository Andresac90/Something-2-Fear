using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;
using Photon.Realtime;
using Photon.Pun;

public class JoseMovement : MonoBehaviourPun
{
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private AudioListener AudioListener;
    [SerializeField]
    private GameObject Canvas;

    private InputMaster Controls;
    private Vector2 Move;

    public float Speed;
    private float OriginalSpeed;
    public float JumpForce;

    private Vector2 YVel;

    [SerializeField]
    private float Gravity = -9.81f;

    private Transform playerCam;
    public float moveSpeed = 5.0f;
    public float lookSensitivity = 2.0f;
    public bool isPlayerInjected = false;
    private float rotationX = 0;
    private CharacterController CharController;
    private PlayerLook look;
    private Animator joseAnimator;

    public bool HasRun = false;

    //Grounded
    public bool IsGrounded;
    //Head Check in editor
    [SerializeField]
    private Transform GroundCheck;
    public float RadiusGround;
    public LayerMask GroundMask;

    //Crouched
    public bool HasCeiling;
    public bool IsCrouched = false;
    //Head Check in editor
    [SerializeField]
    private Transform HeadCheck;
    public float RadiusHead;
    private bool HasCrouched = false;

    //Jump
    private bool HasJump = false;

    PhotonView PV;

    private int controlsModifier  = 1;


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
        look = GetComponent<PlayerLook>();
        CharController = GetComponent<CharacterController>();
        OriginalSpeed = Speed;
        joseAnimator = GetComponent<Animator>();
    }
    public void SetInjected(bool injected)
    {
        isPlayerInjected = injected;
        Debug.Log(isPlayerInjected);
    }
    void Update()
    {
        if (!PV.IsMine) return;


        Movement();
        Jump();
        Crouch();
        Sprint();
        Camera.transform.position = new Vector3(transform.position.x, Camera.transform.position.y, transform.position.z);

    }
    void Movement()
    {
        Move = Controls.Player.Movement.ReadValue<Vector2>();

        if (IsGrounded && YVel.y < 0)
        {
            YVel.y = 0;
        }

        Vector3 MovementZ = (transform.right * Move.x + transform.forward * Move.y);
        YVel.y += Gravity * Time.deltaTime;
        CharController.Move(MovementZ * Speed * Time.deltaTime * controlsModifier);
        CharController.Move(YVel * Time.deltaTime);

        if(Move.x != 0 || Move.y != 0)
        {
            photonView.RPC("UpdateWalkingAnimation", RpcTarget.All, true);
        }
        else
        {
            photonView.RPC("UpdateWalkingAnimation", RpcTarget.All, false);
        }
    }

    void Sprint()
    {
        bool IsSprintPressed = Controls.Player.Run.ReadValue<float>() > 0.1f;
        if (IsSprintPressed && !HasRun && !HasCrouched && IsGrounded)
        {
            photonView.RPC("UpdateRunningAnimation", RpcTarget.All, true);
            photonView.RPC("UpdateWalkingAnimation", RpcTarget.All, false);
            Speed *= 1.9f;
            HasRun = true;
        }
        else if (!IsSprintPressed && HasRun)
        {
            photonView.RPC("UpdateRunningAnimation", RpcTarget.All, false);
            Speed = OriginalSpeed;
            HasRun = false;
        }
    }

    void Jump()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, RadiusGround, GroundMask);
        bool IsJumpPressed = Controls.Player.Jump.ReadValue<float>() > 0.1f;
        if (IsJumpPressed && IsGrounded && !HasCeiling && !IsCrouched && !HasJump)
        {
            YVel.y = Mathf.Sqrt(Gravity * JumpForce * -2);
            HasJump = true;
        }
        else if (IsGrounded && !IsJumpPressed)
        {
            HasJump = false;
        }
    }

    void Crouch()
    {
        HasCeiling = Physics.CheckSphere(HeadCheck.position, RadiusHead, GroundMask);
        bool IsCrouchPressed = Controls.Player.Crouch.ReadValue<float>() > 0.1f;
        if (IsCrouchPressed && !HasCrouched && !HasJump)
        {
            photonView.RPC("UpdateBendingAnimation", RpcTarget.All, true);
            CharController.height = 1;
            CharController.center = new Vector3(0, -0.5f, 0);
            // Camera.localPosition = new Vector3(0, 0.4f, 0.225f);
            Camera.transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z + 0.225f);
            Speed *= .40f;
            HasCrouched = true;
            IsCrouched = true;
        }
        else if (!HasCeiling && !IsCrouchPressed && !HasRun)
        {
            photonView.RPC("UpdateBendingAnimation", RpcTarget.All, false);
            CharController.height = 2;
            CharController.center = new Vector3(0, 0, 0);
            // Camera.localPosition = new Vector3(0, 0.894f, 0.225f);
            Camera.transform.position = new Vector3(transform.position.x, transform.position.y + 0.894f, transform.position.z + 0.225f);
            Speed = OriginalSpeed;
            HasCrouched = false;
            IsCrouched = false;
        }
    }

    [PunRPC]
    void UpdateWalkingAnimation(bool isWalking)
    {
        joseAnimator.SetBool("IsWalking", isWalking);
    }

    [PunRPC]
    void UpdateBendingAnimation(bool isBending)
    {
        joseAnimator.SetBool("IsBending", isBending);
    }

    [PunRPC]
    void UpdateRunningAnimation(bool isRunning)
    {
        joseAnimator.SetBool("IsRunning", isRunning);
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

    // private void InvertControls()
    // {
    //     Vector2 invertedMovementInput = Controls.Player.Movement.ReadValue<Vector2>() * -1;
    //     Vector2 invertedLookInput = Controls.Player.Look.ReadValue<Vector2>() * -1;

    //     // Move the player with inverted controls
    //     Vector3 moveDirection = transform.TransformDirection(new Vector3(invertedMovementInput.x, 0, invertedMovementInput.y));
    //     CharController.Move(moveDirection * moveSpeed * Time.deltaTime);

    //     // Rotate the camera with inverted controls
    //     //look.SetInvert(true);
    // }
}
