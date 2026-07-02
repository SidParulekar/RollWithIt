using UnityEngine;
using UnityEngine.InputSystem;

public class SnapPlatformController : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float maxTiltAngle = 25f;
    public float tiltSpeed = 60f;

    private Vector2 tiltInput;
    private float tiltX;

    private PlayerInput _playerInput;
    private Rigidbody rb;

    [SerializeField] private float ballVerticalPushForce = 20f;
    [SerializeField] private float ballHorizontalPushForce = 20f;
    [SerializeField] private LayerMask opponentLayer;
    private int opponentLayerIndex;
    private bool snapped = false;

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
        snapped = true;
    }

    void FixedUpdate()
    {
        tiltX += tiltInput.y * tiltSpeed * Time.fixedDeltaTime;
        tiltX = Mathf.Clamp(tiltX, -maxTiltAngle, maxTiltAngle);
        rb.MoveRotation(Quaternion.Euler(tiltX, 0f, 0f));
    }

    private void TogglePlayerControl(bool status)
    {
        _playerInput.enabled = status;
    }

    private void OnCollisionEnter(Collision other)
    {
        /*if (other.gameObject.layer == opponentLayerIndex && snapped)
        {
            snapped = false;
            Rigidbody ballRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 vertical = Vector3.up * ballVerticalPushForce;
            Vector3 horizontal = new Vector3(0f, 0f, tiltInput.y) * ballHorizontalPushForce;
            Vector3 hitDirection = (vertical + horizontal).normalized;
            ballRb.AddForce(hitDirection * (ballVerticalPushForce + ballHorizontalPushForce), ForceMode.Impulse);
        }*/
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
            Debug.Log("Exit snap");
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