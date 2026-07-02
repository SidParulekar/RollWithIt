using UnityEngine;

public class MenuPlatformTilt : MonoBehaviour
{
    [Header("Auto Tilt Settings")]
    public float tiltAngle = 20f;
    public float tiltSpeed = 50f;
    public float pauseDuration = 1.5f;

    private Rigidbody rb;
    private float currentTiltZ = 0f;
    private float targetTiltZ = 0f;
    private float pauseTimer = 0f;
    private bool pausing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Start by tilting right
        targetTiltZ = tiltAngle;
    }

    void FixedUpdate()
    {
        if (pausing)
        {
            pauseTimer += Time.fixedDeltaTime;
            if (pauseTimer >= pauseDuration)
            {
                pauseTimer = 0f;
                pausing = false;
                // Flip to opposite direction
                targetTiltZ = -targetTiltZ;
            }
            return;
        }

        // Move toward target angle
        currentTiltZ = Mathf.MoveTowards(currentTiltZ, targetTiltZ, tiltSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.Euler(0f, 0f, currentTiltZ));

        // When reached target angle start pause
        if (Mathf.Approximately(currentTiltZ, targetTiltZ))
        {
            pausing = true;
        }
    }
}