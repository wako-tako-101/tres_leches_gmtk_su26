using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    [Header("Objects To Reset")]
    public Transform[] objectsToReset;

    private Vector3[] originalPositions;

    [Header("Hover Effect")]
    public float hoverScale = 1.15f;
    public float scaleSpeed = 8f;
    public Color hoverColor = Color.white;
    public float brightnessMultiplier = 1.2f;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private Color originalColor;

    private bool playerInRange = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;
        originalColor = spriteRenderer.color;

        originalPositions = new Vector3[objectsToReset.Length];

        for (int i = 0; i < objectsToReset.Length; i++)
        {
            if (objectsToReset[i] != null)
            {
                originalPositions[i] = objectsToReset[i].position;
            }
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            Vector3 targetScale = originalScale * hoverScale;

            transform.localScale = Vector3.Lerp(
                transform.localScale,
                targetScale,
                Time.deltaTime * scaleSpeed
            );

            if (Input.GetKeyDown(interactKey))
            {
                ResetObjectsToOriginalPositions();
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                originalScale,
                Time.deltaTime * scaleSpeed
            );
        }
    }

    public void ResetObjectsToOriginalPositions()
    {
        for (int i = 0; i < objectsToReset.Length; i++)
        {
            if (objectsToReset[i] == null)
                continue;

            objectsToReset[i].position = originalPositions[i];

            Rigidbody2D rb = objectsToReset[i].GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            spriteRenderer.color = hoverColor * brightnessMultiplier;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            spriteRenderer.color = originalColor;
        }
    }
}