using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public FadeInUI fadeInUI;
    public BatteryUI batteryUI;
    public ShutDownEffect shutDownEffect;
    public EventSequence eventSequence;
    public SmoothMovePathUI smoothMovePathUI;
    public AudioManager audioManager; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;

        if (fadeInUI != null)
        {
            fadeInUI.ResetFade();
        }

        if (batteryUI != null)
        {
            batteryUI.ResetBattery();
        }

        if (shutDownEffect != null)
        {
            shutDownEffect.ResetShutdown();
        }

        if (eventSequence != null)
        {
            eventSequence.ResetSequence();
        }

        if (smoothMovePathUI != null)
        {
            smoothMovePathUI.ResetMovement();
        }
        if (audioManager != null)
        {
            audioManager.RestartCurrentLevelMusic();
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadNextScene()
    {
        Time.timeScale = 1f;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No next scene available.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }
}