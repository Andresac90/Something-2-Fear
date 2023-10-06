using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;

using Photon.Pun;

public class JoseMovement : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;

    private InputMaster Controls;
    private Vector2 Move;

    public float Speed;
    private float OriginalSpeed;
    public float JumpForce;

    private Vector2 YVel;

    [SerializeField]
    private float Gravity = -9.81f;

    private CharacterController CharController;

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


    void Awake()
    {
        Controls = new InputMaster();
    }
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if(!PV.IsMine){
            Camera.enabled = false;
        }
        CharController = GetComponent<CharacterController>();
        OriginalSpeed = Speed;
    }

    void Update()
    {
        if (!PV.IsMine) return;

        Movement();
        Jump();
        Crouch();
        Sprint();
        // Camera.transform.position = new Vector3(transform.position.x, Camera.position.y, Camera.position.z);
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
        CharController.Move(MovementZ * Speed * Time.deltaTime);
        CharController.Move(YVel * Time.deltaTime);
    }

    void Sprint()
    {
        bool IsSprintPressed = Controls.Player.Run.ReadValue<float>() > 0.1f;
        if (IsSprintPressed && !HasRun && HasCrouched == false && IsGrounded == true)
        {
            Speed *= 1.9f;
            HasRun = true;
        }

        if (!IsSprintPressed && HasRun == true)
        {
            Speed = OriginalSpeed;
            HasRun = false;
        }
    }

    void Jump()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, RadiusGround, GroundMask);
        bool IsJumpPressed = Controls.Player.Jump.ReadValue<float>() > 0.1f;
        if (IsJumpPressed && IsGrounded && HasCeiling == false && IsCrouched == false && HasJump == false)
        {
            YVel.y = Mathf.Sqrt(Gravity * JumpForce * -2);
            HasJump = true;
        }
        if (IsGrounded && !IsJumpPressed)
        {
            HasJump = false;
        }
    }

    void Crouch()
    {
        HasCeiling = Physics.CheckSphere(HeadCheck.position, RadiusHead, GroundMask);
        bool IsCrouchPressed = Controls.Player.Crouch.ReadValue<float>() > 0.1f;
        if (IsCrouchPressed && !HasCrouched && HasJump == false)
        {
            CharController.height = 1;
            CharController.center = new Vector3(0, -0.5f, 0);
            // Camera.localPosition = new Vector3(0, 0.4f, 0.225f);
            Speed *= .40f;
            HasCrouched = true;
            IsCrouched = true;
        }

        if (HasCeiling == false && !IsCrouchPressed && HasRun == false)
        {
            CharController.height = 2;
            CharController.center = new Vector3(0, 0, 0);
            // Camera.localPosition = new Vector3(0, 0.894f, 0.225f);
            Speed = OriginalSpeed;
            HasCrouched = false;
            IsCrouched = false;
        }
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
