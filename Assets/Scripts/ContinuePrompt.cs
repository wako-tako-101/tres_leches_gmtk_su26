using System.Collections;
using UnityEngine;

public class ContinuePrompt : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.1f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(Pulse());
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