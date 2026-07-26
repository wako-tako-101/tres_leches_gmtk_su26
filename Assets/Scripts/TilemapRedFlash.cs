using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRedFlash : MonoBehaviour
{
    public Tilemap tilemap;

    [Header("Flash Settings")]
    public Color normalColor = Color.white;
    public Color flashColor = Color.red;

    public float minTimeBetweenFlashes = 4f;
    public float maxTimeBetweenFlashes = 10f;

    public float fadeInDuration = 0.4f;
    public float holdDuration = 0.15f;
    public float fadeOutDuration = 0.8f;

    [Header("Random Variation")]
    [Range(0f, 1f)]
    public float maxFlashIntensity = 0.6f;

    private Material tilemapMaterial;

    private void Start()
    {
        if (tilemap == null)
            tilemap = GetComponent<Tilemap>();

        if (tilemap == null)
            return;

        TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();

        if (renderer == null)
            return;

        tilemapMaterial = renderer.material;

        tilemapMaterial.color = normalColor;

        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(
                minTimeBetweenFlashes,
                maxTimeBetweenFlashes
            );

            yield return new WaitForSeconds(waitTime);

            float intensity = UnityEngine.Random.Range(
                0.25f,
                maxFlashIntensity
            );

            yield return StartCoroutine(
                Flash(intensity)
            );
        }
    }

    private IEnumerator Flash(float intensity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / fadeInDuration;

            Color currentColor = Color.Lerp(
                normalColor,
                Color.Lerp(normalColor, flashColor, intensity),
                t
            );

            tilemapMaterial.color = currentColor;

            yield return null;
        }

        tilemapMaterial.color = Color.Lerp(
            normalColor,
            flashColor,
            intensity
        );

        yield return new WaitForSeconds(holdDuration);

        elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / fadeOutDuration;

            tilemapMaterial.color = Color.Lerp(
                Color.Lerp(normalColor, flashColor, intensity),
                normalColor,
                t
            );

            yield return null;
        }

        tilemapMaterial.color = normalColor;
    }
}