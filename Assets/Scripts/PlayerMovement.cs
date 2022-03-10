using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField]
    private float speed = 12f,
                  speedMultiplier = 100f,
                  movementSmoothing = .3f,
                  groundCheckRadius = .4f,
                  jumpHeight = 3f;
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float jumpGravity = 9.8f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private PhysicMaterial physicMaterial;

    bool isGrounded = false, jump;
    Vector3 velocity;
    PlayerInput playerInput;

    private Vector3 velocityRef = Vector3.zero;

    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        //characterController.material = physicMaterial;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = playerInput.Player.Movement.ReadValue<Vector2>();
        Vector3 movementDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        movementDirection *= speed * speedMultiplier * Time.fixedDeltaTime;
        rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, movementDirection,
                                                    ref velocityRef, movementSmoothing);
        if (!isGrounded && rigidbody.velocity.y < 0f)
        {
            rigidbody.AddForce(Vector3.down * gravityForce);
        }
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
            //velocity.y = -2f;
        }

        if (jump && isGrounded)
        {
            jump = false;
            Vector3 jumpForce = Vector3.up * Mathf.Sqrt(jumpHeight * 2f * jumpGravity);
            if (jumpForce != null)
                rigidbody.AddForce(jumpForce, ForceMode.Impulse);
        }

        //velocity.y += gravity * Time.deltaTime;

        //characterController.Move(velocity * Time.deltaTime);
    }
}
