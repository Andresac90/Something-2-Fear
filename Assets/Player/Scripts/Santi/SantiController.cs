using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;

public class SantiController : MonoBehaviour
{
    private Vector2 Move;
    private Vector2 YVel;
    public float Speed;
    private float OriginalSpeed;

    [SerializeField]
    private float Gravity = -9.81f;

    private InputMaster Controls;
    private CharacterController CharController;

    // public GameObject Task;
    //Grounded
    public bool IsGrounded;
    //Head Check in editor
    [SerializeField]
    private Transform GroundCheck;
    public float RadiusGround;
    public LayerMask GroundMask;

    void Awake()
    {
        Controls = new InputMaster();
    }

    void Start()
    {
        CharController = GetComponent<CharacterController>();
        OriginalSpeed = Speed;
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
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

}
