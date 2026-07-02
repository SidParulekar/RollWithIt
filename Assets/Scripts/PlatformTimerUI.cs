using TMPro;
using UnityEngine;

public class PlatformTimerUI : MonoBehaviour
{
    public static PlatformTimerUI Instance;

    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        Instance = this;
        HideTimer();
    }

    public void ShowTimer()
    {
        timerText.gameObject.SetActive(true);
    }

    public void HideTimer()
    {
        timerText.gameObject.SetActive(false);
    }

    public void UpdateTimer(float timeRemaining)
    {
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

    public void SetColor(Color color)
    {
        timerText.color = color;
    }

    public void SetVisible(bool visible)
    {
        timerText.enabled = visible;
    }
}