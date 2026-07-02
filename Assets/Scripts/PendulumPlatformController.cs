using UnityEngine;
using UnityEngine.InputSystem;

public class PendulumPlatformController : MonoBehaviour
{
    [Header("Pendulum Settings")]
    public float swingForce = 30f;
    public float damping = 0.999f;
    public float maxSwingAngle = 80f;
    public float gravityPull = 1f;

    private float currentAngle = 0f;
    private float angularVelocity = 0f;
    private Vector2 swingInput;

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

    public void OnSwing(InputAction.CallbackContext context)
    {
        swingInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Apply input force to angular velocity
        angularVelocity += swingInput.y * swingForce * Time.fixedDeltaTime;

        // Apply natural pendulum gravity pull back to center
        angularVelocity -= currentAngle * gravityPull * Time.fixedDeltaTime;

        // Apply damping so it doesnt swing forever
        angularVelocity *= damping;

        // Update and clamp angle
        currentAngle += angularVelocity * Time.fixedDeltaTime;
        currentAngle = Mathf.Clamp(currentAngle, -maxSwingAngle, maxSwingAngle);

        rb.MoveRotation(Quaternion.Euler(currentAngle, 0f, 0f));
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