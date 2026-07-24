using System;
using System.Collections;
using UnityEngine;

public class GlitchManager : MonoBehaviour
{
    public SpriteRenderer[] phoneSprites;

    public Material glitchMaterial;

    public float glitchDuration = 0.2f;

    public bool enableTimedGlitches = true;

    public float minTimeBetweenGlitches = 5f;

    public float maxTimeBetweenGlitches = 10f;

    [Range(0f, 0.1f)]
    public float glitchIntensity = 0.02f;

    [Range(0f, 0.05f)]
    public float chromaticAberration = 0.01f;

    [Range(0f, 1f)]
    public float glitchAmount = 0.5f;

    [Range(0f, 20f)]
    public float glitchSpeed = 10f;

    private Material[] originalMaterials;
    private Coroutine currentGlitch;
    private Coroutine timedGlitchRoutine;

    private void Start()
    {
        originalMaterials = new Material[phoneSprites.Length];

        for (int i = 0; i < phoneSprites.Length; i++)
        {
            if (phoneSprites[i] != null)
            {
                originalMaterials[i] = phoneSprites[i].sharedMaterial;
            }
        }

        if (enableTimedGlitches)
        {
            timedGlitchRoutine = StartCoroutine(TimedGlitches());
        }
    }

    private IEnumerator TimedGlitches()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(
                minTimeBetweenGlitches,
                maxTimeBetweenGlitches
            );

            yield return new WaitForSeconds(waitTime);

            TriggerGlitch();
        }
    }

    public void TriggerGlitch()
    {
        TriggerGlitch(glitchDuration);
    }

    public void TriggerGlitch(float duration)
    {
        if (currentGlitch != null)
        {
            StopCoroutine(currentGlitch);
        }

        currentGlitch = StartCoroutine(GlitchRoutine(duration));
    }

    private IEnumerator GlitchRoutine(float duration)
    {
        ApplyGlitchMaterial();

        yield return new WaitForSeconds(duration);

        RemoveGlitchMaterial();

        currentGlitch = null;
    }

    private void ApplyGlitchMaterial()
    {
        for (int i = 0; i < phoneSprites.Length; i++)
        {
            if (phoneSprites[i] == null)
                continue;

            Material materialInstance = new Material(glitchMaterial);

            materialInstance.SetFloat(
                "_GlitchIntensity",
                glitchIntensity
            );

            materialInstance.SetFloat(
                "_ChromaticAberration",
                chromaticAberration
            );

            materialInstance.SetFloat(
                "_GlitchAmount",
                glitchAmount
            );

            materialInstance.SetFloat(
                "_GlitchSpeed",
                glitchSpeed
            );

            phoneSprites[i].material = materialInstance;
        }
    }

    private void RemoveGlitchMaterial()
    {
        for (int i = 0; i < phoneSprites.Length; i++)
        {
            if (phoneSprites[i] == null)
                continue;

            phoneSprites[i].sharedMaterial = originalMaterials[i];
        }
    }
}