using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float distance = 6f;
    public float height = 2.5f;
    public float smoothSpeed = 8f;

    private Vector3 currentVelocity;

    private void Start()
    {
       
        transform.position = target.position + Vector3.up * height + Vector3.back * distance;

        transform.LookAt(target.position + Vector3.up * 1f);
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position
            + Vector3.up * height
            + Vector3.back * distance;

        transform.position = Vector3.SmoothDamp(
            transform.position, desiredPosition, ref currentVelocity, 1f / smoothSpeed);
       
    }
}