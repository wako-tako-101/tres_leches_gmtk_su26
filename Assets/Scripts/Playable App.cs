using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayableApp : MonoBehaviour
{
    [Header("App Settings")]
    public string sceneToLoad;

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
                LoadAppScene();
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

    private void LoadAppScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene assigned to " + gameObject.name);
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