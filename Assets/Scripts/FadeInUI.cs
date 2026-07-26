using System.Collections;
using UnityEngine;

public class FadeInUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    public void FadeIn()
    {
        StartFade(0f, 1f);
    }

    public void FadeOut()
    {
        StartFade(1f, 0f);
    }

    private void StartFade(float startAlpha, float targetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(Fade(startAlpha, targetAlpha));
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;

        canvasGroup.alpha = startAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / fadeDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                t
            );

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        fadeCoroutine = null;
    }

    public void ResetFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        canvasGroup.alpha = 0f;
    }

    public void ResetFull()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        canvasGroup.alpha = 1f;
    }
}