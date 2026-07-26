using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [Header("Animation")]
    public float floatDistance = 50f;
    public float floatDuration = 1f;

    [Header("Text")]
    public float fadeDuration = 0.5f;

    private TextMeshProUGUI damageText;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetDamage(float damage)
    {
        damageText.text = "-" + Mathf.RoundToInt(damage);
    }

    public void StartAnimation()
    {
        StartCoroutine(FloatAndFade());
    }

    IEnumerator FloatAndFade()
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + Vector2.up * floatDistance;

        float timer = 0f;

        while (timer < floatDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / floatDuration;

            // Move upward
            rectTransform.anchoredPosition = Vector2.Lerp(
                startPosition,
                endPosition,
                progress
            );

            // Fade out
            canvasGroup.alpha = Mathf.Lerp(
                1f,
                0f,
                progress
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}