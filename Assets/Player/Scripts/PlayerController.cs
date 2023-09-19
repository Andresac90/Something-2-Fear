using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private InputMaster controls;
    private float moveSpeed = 6f;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private Vector2 move;
    private float jumpHeight = 2.4f;
    private CharacterController controller;
    private bool isGrounded;
    
    public Transform ground;
    public float distanceToGround = 0.4f;
    public LayerMask groundMask;
    //True es Jose y false es Santi
    public bool Character = true;
    public Text DebugText;
    private string CharacterName;
    
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
        Jump();
        ChangeCharacters();

        //DebugText
        if (Character == true)
        {
            CharacterName = "Jose";
        }
        else
        {
            CharacterName = "Santi";
        }

        DebugText.text = "<color=green><b>Debug Menu</b></color>\r\nCurent Player: " + CharacterName + "\r\nSpeed: " + moveSpeed + "\r\nJump Status: " + Character;
    }

    private void ChangeCharacters()
    {
        if (controls.Player.ChangeCharacter.triggered)
        {
            Character = !Character;
        }
        
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayerMovement()
    {
        if (Character == false)
        {
            moveSpeed = 3f;
        }
        else
        {
            moveSpeed = 5f;
        }
        move = controls.Player.Movement.ReadValue<Vector2>();

        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (controls.Player.Jump.triggered && Character == true)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
        }
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
