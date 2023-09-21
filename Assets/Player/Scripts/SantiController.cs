using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SantiController : MonoBehaviour
{
    private Vector2 move;
    
    private float moveSpeed = 6f;

    private Vector3 velocity;

    [SerializeField]
    private float gravity = -9.81f;

    private InputMaster controls;
    private CharacterController controller;

    private bool isGrounded;
    public Transform ground;
    public float distanceToGround = 0.4f;
    public LayerMask groundMask;
    
    void Awake()
    {
        controls = new InputMaster();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        PlayerMovement();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayerMovement()
    {
        move = controls.Player.Movement.ReadValue<Vector2>();

        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}
