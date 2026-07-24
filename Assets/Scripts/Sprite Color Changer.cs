using System.Collections;
using UnityEngine;

public class SpriteColorChanger : MonoBehaviour
{
    public SpriteRenderer[] sprites;

    public Color targetColor = Color.white;

    [Tooltip("0 = instant. Higher values fade more slowly.")]
    public float transitionTime = 1f;

    public void SetColor()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite == null)
                continue;

            if (transitionTime <= 0f)
            {
                // Instant change
                sprite.color = targetColor;
            }
            else
            {
                StartCoroutine(ChangeColor(sprite));
            }
        }
    }

    private IEnumerator ChangeColor(SpriteRenderer sprite)
    {
        Color startColor = sprite.color;
        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            sprite.color = Color.Lerp(startColor, targetColor, elapsed / transitionTime);
            yield return null;
        }

        sprite.color = targetColor;
    }
}