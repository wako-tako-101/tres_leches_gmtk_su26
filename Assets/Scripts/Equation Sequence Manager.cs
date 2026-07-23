using System.Collections;
using UnityEngine;

public class EquationSequenceManager : MonoBehaviour
{
    public GameObject[] equations;

    public float fadeDuration = 0.5f;

    public AudioSource audioSource;
    public AudioClip whooshSound;

    private int currentEquation = 0;
    private bool transitioning = false;

    private void Start()
    {
        for (int i = 0; i < equations.Length; i++)
        {
            equations[i].SetActive(i == 0);
        }
    }

    public void EquationCompleted()
    {
        if (transitioning)
        {
            return;
        }

        if (currentEquation >= equations.Length - 1)
        {
            Debug.Log("ALL EQUATIONS COMPLETED!");
            return;
        }

        StartCoroutine(TransitionToNextEquation());
    }

    private IEnumerator TransitionToNextEquation()
    {
        transitioning = true;

        if (audioSource != null && whooshSound != null)
        {
            audioSource.PlayOneShot(whooshSound);
        }

        SpriteRenderer[] currentRenderers =
            equations[currentEquation].GetComponentsInChildren<SpriteRenderer>();

        yield return StartCoroutine(FadeOut(currentRenderers));

        equations[currentEquation].SetActive(false);

        currentEquation++;

        equations[currentEquation].SetActive(true);

        SpriteRenderer[] nextRenderers =
            equations[currentEquation].GetComponentsInChildren<SpriteRenderer>();

        SetAlpha(nextRenderers, 0f);

        yield return StartCoroutine(FadeIn(nextRenderers));

        transitioning = false;
    }

    private IEnumerator FadeOut(SpriteRenderer[] renderers)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            SetAlpha(renderers, alpha);

            yield return null;
        }

        SetAlpha(renderers, 0f);
    }

    private IEnumerator FadeIn(SpriteRenderer[] renderers)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);

            SetAlpha(renderers, alpha);

            yield return null;
        }

        SetAlpha(renderers, 1f);
    }

    private void SetAlpha(SpriteRenderer[] renderers, float alpha)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }
}