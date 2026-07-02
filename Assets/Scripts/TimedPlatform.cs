using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TimedPlatform : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float lifetime = 15f;
    [SerializeField] private float warningTime = 5f;
    [SerializeField] private float flashInterval = 0.25f;

    [Header("Warning")]
    [SerializeField] private Color warningColor = Color.red;

    [Header("Ball")]
    [SerializeField] private LayerMask ballLayer;

    private MeshRenderer platformRenderer;
    private Material platformMaterial;
    private Color originalColor;

    private int ballLayerIndex;

    private float timer;
    private float flashTimer;

    private bool timerRunning;
    private bool flashing;

    private void Start()
    {
        platformRenderer = GetComponent<MeshRenderer>();

        // Creates a unique material instance for this platform
        platformMaterial = platformRenderer.material;
        originalColor = platformMaterial.color;

        timer = lifetime;

        ballLayerIndex = (int)Mathf.Log(ballLayer.value, 2);
    }

    private void Update()
    {
        if (!timerRunning)
            return;

        timer -= Time.deltaTime;

        PlatformTimerUI.Instance.UpdateTimer(timer);

        if (timer <= warningTime)
        {
            FlashWarning();
        }

        if (timer <= 0f)
        {
            PlatformTimerUI.Instance.HideTimer();

            Destroy(gameObject);
        }
    }

    private void FlashWarning()
    {
        PlatformTimerUI.Instance.SetColor(Color.red);

        flashTimer += Time.deltaTime;

        if (flashTimer >= flashInterval)
        {
            flashTimer = 0f;

            flashing = !flashing;

            // Flash timer text
            PlatformTimerUI.Instance.SetVisible(flashing);

            // Flash platform
            platformMaterial.color = flashing ? warningColor : originalColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != ballLayerIndex)
            return;

        timerRunning = true;

        PlatformTimerUI.Instance.ShowTimer();
        PlatformTimerUI.Instance.SetColor(Color.white);
        PlatformTimerUI.Instance.UpdateTimer(timer);

        platformMaterial.color = originalColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != ballLayerIndex)
            return;

        PlatformTimerUI.Instance.HideTimer();

        // Restore original appearance
        platformMaterial.color = originalColor;
    }

    private void OnDestroy()
    {
        if (platformMaterial != null)
        {
            platformMaterial.color = originalColor;
        }
    }
}