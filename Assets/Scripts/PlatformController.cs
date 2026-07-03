using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float maxTiltAngle = 25f;
    public float tiltForce = 150f;
    public float damping = 0.85f;
    public float gravityPull = 40f;

    protected Vector2 tiltInput;
    protected float tiltX, tiltZ;
    protected float angularVelocityX, angularVelocityZ;

    protected Rigidbody rb;

    [SerializeField] protected LayerMask opponentLayer;
    protected int opponentLayerIndex;

    [SerializeField] protected AudioClip ballThrow;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        opponentLayerIndex = (int)Mathf.Log(opponentLayer.value, 2);
    }
   
    public void ReceiveInput(Vector2 input)
    {
        tiltInput = input;
        //Debug.Log("Moving");
    }

    protected virtual void FixedUpdate()
    {
        angularVelocityX += tiltInput.y * tiltForce * Time.fixedDeltaTime;
        angularVelocityZ -= tiltInput.x * tiltForce * Time.fixedDeltaTime;

        angularVelocityX -= tiltX * gravityPull * Time.fixedDeltaTime;
        angularVelocityZ -= tiltZ * gravityPull * Time.fixedDeltaTime;

        angularVelocityX *= damping;
        angularVelocityZ *= damping;

        tiltX += angularVelocityX * Time.fixedDeltaTime;
        tiltZ += angularVelocityZ * Time.fixedDeltaTime;

        tiltX = Mathf.Clamp(tiltX, -maxTiltAngle, maxTiltAngle);
        tiltZ = Mathf.Clamp(tiltZ, -maxTiltAngle, maxTiltAngle);

        rb.MoveRotation(Quaternion.Euler(tiltX, 0f, tiltZ));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != opponentLayerIndex) return;

        PlatformManager.Instance.SetPlatform(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != opponentLayerIndex) return;

        Destroy(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == opponentLayerIndex)
        {
            SoundManager.Instance.PlaySound(ballThrow);
        }
    }
}