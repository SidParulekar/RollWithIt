using UnityEngine;
using UnityEngine.InputSystem;

public class SphereController : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 10f;
    public float maxSpeed = 8f;
    public float jumpForce = 5f;

    [Header("Rolling")]
    public float extraGravity = 20f;
    public float rollTorque = 15f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.6f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private Camera mainCam;

    private int groundLayerIndex;

    private bool extraGravityMode = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundLayerIndex = (int)Mathf.Log(groundMask.value, 2);
    }

    void Start()
    {
        mainCam = Camera.main;
        rb.sleepThreshold = 0f;
        rb.angularDamping = 0.01f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpPressed = true;
    }

    void FixedUpdate()
    {
        Vector3 camForward = mainCam.transform.forward;
        Vector3 camRight = mainCam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x);

        // Linear force for movement
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(moveDir * moveForce, ForceMode.Force);
        }

        // Torque makes the ball actually roll instead of slide
        Vector3 torqueDir = new Vector3(moveDir.z, 0f, -moveDir.x);
        rb.AddTorque(torqueDir * rollTorque, ForceMode.Force);

        if (extraGravityMode)
        {
            // Extra gravity so ball rolls more aggressively on slopes
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
       
        // Jump
        if (jumpPressed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpPressed = false;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }

    private void ToggleExtraGravity(bool mode)
    {
        extraGravityMode = mode;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == groundLayerIndex)
        {
            ToggleExtraGravity(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == groundLayerIndex)
        {
            ToggleExtraGravity(false);
        }
    }
}