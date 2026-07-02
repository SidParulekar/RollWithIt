using UnityEngine;
using TMPro;

public class PlatformTutorial : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string message;

    [SerializeField] private TextMeshProUGUI tutorialText;

    [SerializeField] private float displayTime = 7f;

    [Header("Ball")]
    [SerializeField] private LayerMask ballLayer;

    private int ballLayerIndex;

    private bool hasShown;

    private void Start()
    {
        ballLayerIndex = (int)Mathf.Log(ballLayer.value, 2);

        tutorialText.gameObject.SetActive(false);

        hasShown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != ballLayerIndex)
            return;

        if (hasShown) return;

        tutorialText.text = message;

        tutorialText.gameObject.SetActive(true);
        hasShown = true;

        Invoke(nameof(Hide), displayTime);
    }   

    private void Hide()
    {
        tutorialText.gameObject.SetActive(false);
    }
}
