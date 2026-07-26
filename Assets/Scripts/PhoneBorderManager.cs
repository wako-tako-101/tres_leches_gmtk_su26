using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneVisualManager : MonoBehaviour
{
    public static SceneVisualManager Instance;

    [Header("Objects to Disable in Settings Scene")]
    public GameObject[] objectsToDisableInSettings;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isSettingsScene = scene.name == "SettingScene";

        foreach (GameObject obj in objectsToDisableInSettings)
        {
            if (obj != null)
            {
                obj.SetActive(!isSettingsScene);
            }
        }
    }
}