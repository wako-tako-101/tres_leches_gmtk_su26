using System.Collections;
using UnityEngine;

public class ContinuePrompt : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.1f;

    private Vector3 originalScale;
    private Coroutine pulseCoroutine;

    private void OnEnable()
    {
        originalScale = transform.localScale;
        pulseCoroutine = StartCoroutine(Pulse());
    }

    private void OnDisable()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }

        transform.localScale = originalScale;
    }

    private IEnumerator Pulse()
    {
        while (true)
        {
            float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

            transform.localScale = originalScale * scale;

            yield return null;
        }
    }
}