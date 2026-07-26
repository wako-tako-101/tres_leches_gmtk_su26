using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelChange : MonoBehaviour
{
    [SerializeField] private int levelToLoad;
    [SerializeField] private float delaySeconds = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(levelToLoad < 0 || levelToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Invalid level: " + levelToLoad);
            return;
        }

        else if (delaySeconds < 0f)
        {
            Debug.LogError("Delay seconds cannot be negative: " + delaySeconds);
            return;
        }
    }

    public void DelayLevelLoad()
    {
        StartCoroutine(LoadLevelAfterDelay());
    }

    private IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        Debug.Log("Loading level " + levelToLoad + " after" + delaySeconds + " seconds.");
        SceneManager.LoadScene(levelToLoad);
    }
}
