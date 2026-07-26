using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeExit : MonoBehaviour
{
    [Header("Player")]
    public MonoBehaviour playerMovementScript; 
    private ShutDownEffect shutdownEffect;
    private bool hasEscaped = false;

    [Header("Shutdown Audio")]
    public AudioSource audioSource;
    public AudioClip firstAudio;
    public AudioClip secondAudio;
    public float secondAudioDelay = 1f;


    public float delayTillSceneChange = 3f;


    private void Start()
    {
        shutdownEffect = FindFirstObjectByType<ShutDownEffect>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasEscaped)
            return;

        if (collision.CompareTag("Player"))
        {
            StartCoroutine(EscapeSequence());
        }
    }

    private IEnumerator EscapeSequence()
    {
        hasEscaped = true;

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        GameObject robot = GameObject.FindGameObjectWithTag("Robot");

        if (robot != null)
        {
            Transform robotRoot = robot.transform.root;
            Destroy(robotRoot.gameObject);
        }

        if (BatteryUI.Instance != null)
        {
            BatteryUI.Instance.PauseBatteryDrain();
        }
        if (audioSource != null && firstAudio!=null)
        {
            audioSource.PlayOneShot(firstAudio);
        }
        if (shutdownEffect != null)
        {
            shutdownEffect.PlayShutdown();
        }
        yield return new WaitForSeconds(secondAudioDelay);

        if (audioSource != null && secondAudio!=null)
        {
            audioSource.PlayOneShot(secondAudio);
        }

        yield return new WaitForSeconds(secondAudioDelay);

        if (AudioManager.Instance != null)
        {
                AudioManager.Instance.PlayMusicForScene("CreditsScene");
        }

        SceneManager.LoadScene("CreditsScene");
        
        yield return null;
    }
}