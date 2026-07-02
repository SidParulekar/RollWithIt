using UnityEngine;
using TMPro;
using System.Collections;

public class LevelObjectiveUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float holdDuration = 8f;

    private Coroutine routine;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        messageText.gameObject.SetActive(false);
    }

    private void Start()
    {
        ShowObjective();
    }

    public void ShowObjective()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {        
        messageText.gameObject.SetActive(true);

        // Fade IN
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Hold
        yield return new WaitForSeconds(holdDuration);

        // Fade OUT
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float start, float end, float duration)
    {
        float t = 0f;

        canvasGroup.alpha = start;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}