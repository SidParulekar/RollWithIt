using UnityEngine;

public class PendulumPlatformController : PlatformController
{
    public float swingForce = 30f;    
    public float maxSwingAngle = 80f;
    
    private float currentAngle;
    private float angularVelocity;

    protected override void FixedUpdate()
    {
         // Apply input force to angular velocity
        angularVelocity += tiltInput.y * swingForce * Time.fixedDeltaTime;

        // Apply natural pendulum gravity pull back to center
        angularVelocity -= currentAngle * gravityPull * Time.fixedDeltaTime;

        // Apply damping so it doesnt swing forever
        angularVelocity *= damping;

        // Update and clamp angle
        currentAngle += angularVelocity * Time.fixedDeltaTime;
        currentAngle = Mathf.Clamp(currentAngle, -maxSwingAngle, maxSwingAngle);

        rb.MoveRotation(Quaternion.Euler(currentAngle, 0f, 0f));
    }
}