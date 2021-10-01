using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 12f, gravity = -9.81f, groundCheckRadius = .4f, jumpHeight = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private PhysicMaterial physicMaterial;

    bool isGrounded, jump;
    Vector3 velocity;
    Vector3 moveDirection;
    PlayerInput playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        characterController.material = physicMaterial;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = playerInput.Player.Movement.ReadValue<Vector2>();
        this.moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jump = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (jump && isGrounded)
        {
            jump = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
