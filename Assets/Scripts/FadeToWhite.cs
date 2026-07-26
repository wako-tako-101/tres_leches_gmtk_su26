using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeToWhite : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private Canvas canvas;
    public float fadeDuration = 3f;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
        else
        {
            Color color = Color.white;
            color.a = 0f;
            fadeImage = new GameObject("FadeImage").AddComponent<Image>();
        }
    }

    public void FadeOut(float fadeDuration)
    {
        if(fadeImage != null)
        {
            StartCoroutine(FadeOutCoroutine(fadeDuration));
           // fadeImage.CrossFadeAlpha(1f, fadeDuration, false);
            Debug.Log("Fading out to white over " + fadeDuration + " seconds.");
        }
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}
