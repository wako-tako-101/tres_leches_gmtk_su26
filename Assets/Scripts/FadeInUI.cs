using System.Collections;
using UnityEngine;

public class FadeInUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    public void FadeIn()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float elapsedTime = 0f;

        canvasGroup.alpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / fadeDuration;

            // Smooth fade
            t = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = t;

            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
    public void ResetFade()
    {
        StopAllCoroutines();

        canvasGroup.alpha = 0f;
    }
}