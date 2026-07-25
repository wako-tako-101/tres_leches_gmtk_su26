using UnityEngine;
using System.Collections;

[System.Serializable]
public class RoundSettings
{
    public int rainToWin;
    public float spawnInterval;
    public float lightningChance;
    public float fallSpeed;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public RoundSettings[] rounds = new RoundSettings[3];
    public int startingLives = 3;

    private int currentRoundIndex = 0;
    private int rainCount = 0;
    private int lives;

    void Awake()
    {
        Instance = this;
        lives = startingLives;
    }

    void Start()
    {
        WeatherUIManager.Instance.ShowGameplayHUD();
        WeatherUIManager.Instance.UpdateHearts(lives);
        StartRound(0);
    }

    void OnDestroy()
    {
        if (WeatherUIManager.Instance != null)
            WeatherUIManager.Instance.HideGameplayHUD();
    }

    void StartRound(int index)
    {
        currentRoundIndex = index;
        rainCount = 0;
        RoundSettings settings = rounds[index];
        WeatherUIManager.Instance.UpdateWaterMeter(0, settings.rainToWin);
        StartCoroutine(RoundTransition(settings));
    }

    IEnumerator RoundTransition(RoundSettings settings)
    {
        Spawner.Instance.StopSpawning();
        Spawner.Instance.ClearAllFallingItems();
        WeatherUIManager.Instance.ShowRoundBanner(currentRoundIndex + 1);
        yield return new WaitForSeconds(1.5f);
        Spawner.Instance.ApplySettings(settings.spawnInterval, settings.lightningChance, settings.fallSpeed);
    }

    public void AddRain()
    {
        rainCount++;
        RoundSettings settings = rounds[currentRoundIndex];
        WeatherUIManager.Instance.UpdateWaterMeter(rainCount, settings.rainToWin);

        if (rainCount >= settings.rainToWin)
        {
            if (currentRoundIndex < rounds.Length - 1)
                StartRound(currentRoundIndex + 1);
            else
                WinGame();
        }
    }

    public void HitByLightning()
    {
        lives--;
        WeatherUIManager.Instance.UpdateHearts(lives);
        if (lives <= 0) LoseGame();
    }

    void WinGame()
    {
        Debug.Log("You win!");
        GameSceneManager.Instance.LoadScene("WinScene");
    }

    void LoseGame()
    {
        Debug.Log("Game over!");
        GameSceneManager.Instance.LoadScene("GameOverScene");
    }
}