using System.Collections;
using UnityEngine;

public class RobotCollision : MonoBehaviour
{
    [Header("Player")]
    public Transform playerStartPosition;
    public MonoBehaviour playerMovementScript;

    [Header("Player Visuals")]
    public SpriteRenderer playerSprite;
    public GlitchManager glitchManager;

    [Header("Robot")]
    public GameObject robotPrefab;
    public Transform robotStartPosition;

    [Header("Robot Explosion Animation")]
    public string explosionTrigger = "Explode";
    public float explosionDelay = 0.5f;

    [Header("Fade Settings")]
    public float fadeOutDuration = 0.3f;
    public float invisibleDuration = 0.2f;
    public float fadeInDuration = 0.3f;

    private bool isExploding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploding)
            return;

        if (collision.CompareTag("Robot"))
        {
            StartCoroutine(HandlePlayerCollision(collision.gameObject));
        }
    }

    private IEnumerator HandlePlayerCollision(GameObject robotPart)
    {
        isExploding = true;

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        Animator robotAnimator = robotPart.GetComponentInParent<Animator>();

        if (robotAnimator != null)
        {
            robotAnimator.SetTrigger(explosionTrigger);
        }

        yield return new WaitForSeconds(explosionDelay);

        if (glitchManager != null)
        {
            glitchManager.TriggerGlitch();
        }

        yield return StartCoroutine(
            FadePlayer(1f, 0f, fadeOutDuration)
        );

        yield return new WaitForSeconds(invisibleDuration);

        transform.position = playerStartPosition.position;

        Transform robotRoot = robotPart.transform.root;

        Destroy(robotRoot.gameObject);

        if (robotPrefab != null)
        {
            Instantiate(
                robotPrefab,
                robotStartPosition.position,
                robotStartPosition.rotation
            );
        }

        yield return StartCoroutine(
            FadePlayer(0f, 1f, fadeInDuration)
        );

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        isExploding = false;
    }

    private IEnumerator FadePlayer(
        float startAlpha,
        float targetAlpha,
        float duration
    )
    {
        if (playerSprite == null)
            yield break;

        Color color = playerSprite.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsedTime / duration
            );

            color.a = alpha;
            playerSprite.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        playerSprite.color = color;
    }
}