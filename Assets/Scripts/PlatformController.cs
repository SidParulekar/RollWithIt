using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformController : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float maxTiltAngle = 25f;
    public float tiltForce = 150f;
    public float damping = 0.85f;
    public float gravityPull = 40f;

    private Vector2 tiltInput;
    private float tiltX;
    private float tiltZ;
    private float angularVelocityX;
    private float angularVelocityZ;

    private PlayerInput _playerInput;
    private Rigidbody rb;

    [SerializeField] private LayerMask opponentLayer;
    private int opponentLayerIndex;

    [SerializeField] private AudioClip ballThrow;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        TogglePlayerControl(false);
        opponentLayerIndex = (int)Mathf.Log(opponentLayer.value, 2);
    }

    public void OnTilt(InputAction.CallbackContext context)
    {
        tiltInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Input pushes angular velocity in the pressed direction
        angularVelocityX += tiltInput.y * tiltForce * Time.fixedDeltaTime;
        angularVelocityZ -= tiltInput.x * tiltForce * Time.fixedDeltaTime;

        // Gravity pulls back to flat when no input
        angularVelocityX -= tiltX * gravityPull * Time.fixedDeltaTime;
        angularVelocityZ -= tiltZ * gravityPull * Time.fixedDeltaTime;

        // Damping bleeds momentum over time
        angularVelocityX *= damping;
        angularVelocityZ *= damping;

        // Update angles
        tiltX += angularVelocityX * Time.fixedDeltaTime;
        tiltZ += angularVelocityZ * Time.fixedDeltaTime;

        // Clamp
        tiltX = Mathf.Clamp(tiltX, -maxTiltAngle, maxTiltAngle);
        tiltZ = Mathf.Clamp(tiltZ, -maxTiltAngle, maxTiltAngle);

        rb.MoveRotation(Quaternion.Euler(tiltX, 0f, tiltZ));
    }

    private void TogglePlayerControl(bool status)
    {
        _playerInput.enabled = status;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == opponentLayerIndex)
        {
            TogglePlayerControl(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == opponentLayerIndex)
        {
            TogglePlayerControl(false);            
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == opponentLayerIndex)
        {
            SoundManager.Instance.PlaySound(ballThrow);
        }
    }
}