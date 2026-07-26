using System.Collections;
using UnityEngine;

public class RandomChildSpriteChanger : MonoBehaviour
{
    [Header("Parent containing the sprite children")]
    public Transform parentObject;

    [Header("Sprites that can be randomly selected")]
    public Sprite[] possibleSprites;

    [Header("Time between sprite changes")]
    public float minChangeTime = 1f;
    public float maxChangeTime = 5f;

    private void Start()
    {
        // Find every child under the parent
        foreach (Transform child in parentObject)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                StartCoroutine(ChangeSpriteRandomly(spriteRenderer));
            }
        }
    }

    private IEnumerator ChangeSpriteRandomly(SpriteRenderer spriteRenderer)
    {
        while (true)
        {
            // Wait a random amount of time
            float randomWaitTime = UnityEngine.Random.Range(minChangeTime, maxChangeTime);
            yield return new WaitForSeconds(randomWaitTime);

            // Pick a random sprite
            int randomIndex = UnityEngine.Random.Range(0, possibleSprites.Length);

            // Change the sprite
            spriteRenderer.sprite = possibleSprites[randomIndex];
        }
    }
}