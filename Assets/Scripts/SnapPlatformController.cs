using UnityEngine;

public class SnapPlatformController : PlatformController
{
    public float snapSpeed = 60f;

    protected override void FixedUpdate()
    {
        if (tiltInput == Vector2.zero) return;

        tiltX += tiltInput.y * snapSpeed * Time.fixedDeltaTime;
        tiltX = Mathf.Clamp(tiltX, -maxTiltAngle, maxTiltAngle);

        rb.MoveRotation(Quaternion.Euler(tiltX, 0f, 0f));
    }
}