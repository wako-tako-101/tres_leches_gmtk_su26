using System.Collections.Generic;
using UnityEngine;

public class NumberSlot : MonoBehaviour
{
    public GameObject currentBox;

    private List<GameObject> boxesInside = new List<GameObject>();
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public Color correctColor = Color.green;
    public Color multipleBoxesColor = Color.red;

    public AudioSource audioSource;
    public AudioClip boxPlacedSound;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        originalColor.a = 1f;

        UpdateSlotColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsNumberBox(other.gameObject))
        {
            Debug.Log("Box entered");

            if (!boxesInside.Contains(other.gameObject))
            {
                boxesInside.Add(other.gameObject);
            }

            if (boxesInside.Count == 1)
            {
                if (audioSource != null && boxPlacedSound != null)
                {
                    audioSource.PlayOneShot(boxPlacedSound);
                }
            }

            UpdateSlotColor();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (boxesInside.Contains(other.gameObject))
        {
            boxesInside.Remove(other.gameObject);
        }

        if (currentBox == other.gameObject)
        {
            currentBox = null;
        }

        UpdateSlotColor();
    }

    private void UpdateSlotColor()
    {
        if (spriteRenderer == null)
            return;

        if (boxesInside.Count == 0)
        {
            currentBox = null;
            spriteRenderer.color = originalColor;
        }
        else if (boxesInside.Count == 1)
        {
            currentBox = boxesInside[0];

            Color color = correctColor;
            color.a = 1f;

            spriteRenderer.color = color;
        }
        else
        {
            currentBox = null;

            Color color = multipleBoxesColor;
            color.a = 1f;

            spriteRenderer.color = color;
        }
    }

    private bool IsNumberBox(GameObject obj)
    {
        return obj.CompareTag("Number0") ||
               obj.CompareTag("Number1") ||
               obj.CompareTag("Number2") ||
               obj.CompareTag("Number3") ||
               obj.CompareTag("Number4") ||
               obj.CompareTag("Number5") ||
               obj.CompareTag("Number6") ||
               obj.CompareTag("Number7") ||
               obj.CompareTag("Number8") ||
               obj.CompareTag("Number9") ||
               obj.CompareTag("Plus") ||
               obj.CompareTag("Minus") ||
               obj.CompareTag("Multiply") ||
               obj.CompareTag("Divide");
    }
}