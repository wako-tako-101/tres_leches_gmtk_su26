using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShutDownEffect : MonoBehaviour
{
    [Header("UI")]
    public GameObject shutdownEffect;
    public Image blackOverlay;
    public RectTransform shutdownLine;

    [Header("Timing")]
    public float fadeDuration = 0.5f;
    public float lineDuration = 0.4f;
    public float dotDuration = 0.2f;

    private void Start()
    {
        // Make sure everything starts hidden
        blackOverlay.color = new Color(0f, 0f, 0f, 0f);

        shutdownLine.localScale = new Vector3(1f, 0f, 1f);

        shutdownEffect.SetActive(false);
    }

    public void PlayShutdown()
    {
        StopAllCoroutines();

        shutdownEffect.SetActive(true);

        StartCoroutine(ShutdownSequence());
    }
    public void ResetShutdown()
    {
        StopAllCoroutines();

        if (blackOverlay != null)
        {
            blackOverlay.color = new Color(0f, 0f, 0f, 0f);
        }

        if (shutdownLine != null)
        {
            shutdownLine.localScale = new Vector3(1f, 0f, 1f);
        }

        if (shutdownEffect != null)
        {
            shutdownEffect.SetActive(false);
        }
    }
    private IEnumerator ShutdownSequence()
    {
        
        yield return StartCoroutine(FadeToBlack());

        
        yield return StartCoroutine(ShowShutdownLine());

        
        yield return StartCoroutine(ShrinkToDot());

        
        yield return new WaitForSeconds(dotDuration);

        
        blackOverlay.color = Color.black;
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / fadeDuration;

            
            t = Mathf.SmoothStep(0f, 1f, t);

            Color color = blackOverlay.color;
            color.a = t;

            blackOverlay.color = color;

            yield return null;
        }

        Color finalColor = blackOverlay.color;
        finalColor.a = 1f;
        blackOverlay.color = finalColor;
    }

    private IEnumerator ShowShutdownLine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lineDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / lineDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            shutdownLine.localScale = new Vector3(
                1f,
                t,
                1f
            );

            yield return null;
        }

        shutdownLine.localScale = new Vector3(1f, 1f, 1f);
    }

    private IEnumerator ShrinkToDot()
    {
        float elapsedTime = 0f;

        Vector3 startScale = shutdownLine.localScale;
        Vector3 endScale = new Vector3(0f, 1f, 1f);

        while (elapsedTime < lineDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / lineDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            shutdownLine.localScale = Vector3.Lerp(
                startScale,
                endScale,
                t
            );

            yield return null;
        }

        shutdownLine.localScale = endScale;
    }
}