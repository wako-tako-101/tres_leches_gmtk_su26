using System.Collections;
using TMPro;
using UnityEngine;

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

    [Header("Rounds")]
    public RoundSettings[] rounds = new RoundSettings[3];

    [Header("Lightning Damage")]
    public float lightningBatteryDamage = 10f;
    public GameObject damageNumberPrefab;
    public Canvas damageNumberCanvas;

    private int currentRoundIndex = 0;
    private int rainCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        WeatherUIManager.Instance.ShowGameplayHUD();
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

        WeatherUIManager.Instance.UpdateWaterMeter(
            0,
            settings.rainToWin
        );

        StartCoroutine(RoundTransition(settings));
    }

    IEnumerator RoundTransition(RoundSettings settings)
    {
        Spawner.Instance.StopSpawning();
        Spawner.Instance.ClearAllFallingItems();

        WeatherUIManager.Instance.ShowRoundBanner(
            currentRoundIndex + 1
        );

        yield return new WaitForSeconds(1.5f);

        Spawner.Instance.ApplySettings(
            settings.spawnInterval,
            settings.lightningChance,
            settings.fallSpeed
        );
    }

    public void AddRain()
    {
        rainCount++;

        RoundSettings settings = rounds[currentRoundIndex];

        WeatherUIManager.Instance.UpdateWaterMeter(
            rainCount,
            settings.rainToWin
        );

        if (rainCount >= settings.rainToWin)
        {
            if (currentRoundIndex < rounds.Length - 1)
            {
                StartRound(currentRoundIndex + 1);
            }
            else
            {
                WinGame();
            }
        }
    }

    public void HitByLightning(Vector3 hitPosition)
    {
        // Drain battery
        if (BatteryUI.Instance == null)
        {
            Debug.LogError("BatteryUI.Instance is NULL!");
        }
        else
        {
            BatteryUI.Instance.ChangeBattery(-lightningBatteryDamage);

        }

        // Spawn damage number
        if (damageNumberPrefab != null && damageNumberCanvas != null)
        {
            RectTransform canvasRect = damageNumberCanvas.GetComponent<RectTransform>();

            Vector2 screenPosition = Camera.main.WorldToScreenPoint(hitPosition);

            Vector2 canvasPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPosition,
                damageNumberCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null
                    : Camera.main,
                out canvasPosition
            );

            GameObject damageNumber = Instantiate(
                damageNumberPrefab,
                damageNumberCanvas.transform
            );

            RectTransform damageRect =
                damageNumber.GetComponent<RectTransform>();

            damageRect.anchoredPosition = canvasPosition;

            DamageNumber damage =
                damageNumber.GetComponent<DamageNumber>();

            if (damage != null)
            {
                damage.SetDamage(lightningBatteryDamage);
                damage.StartAnimation();
            }
        }
    }
    public void StopWeatherGame()
    {
        Debug.Log("Battery empty! Stopping Weather game.");


        if (Spawner.Instance != null)
        {
            Spawner.Instance.StopSpawning();
            Spawner.Instance.ClearAllFallingItems();
        }


        if (WeatherAudioManager.Instance != null)
        {
            WeatherAudioManager.Instance.StopAllAudio();
        }


        if (WeatherUIManager.Instance != null)
        {
            WeatherUIManager.Instance.HideGameplayHUD();
        }

    }

    void WinGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusicForScene("Main Menu 3");
        }
        GameSceneManager.Instance.LoadScene("Main Menu 3");
    }

    void LoseGame()
    {
        Debug.Log("Game over!");

        GameSceneManager.Instance.LoadScene("GameOverScene");
    }
}