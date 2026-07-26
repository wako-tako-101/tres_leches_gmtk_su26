using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BatteryUI : MonoBehaviour
{
    public static BatteryUI Instance;

    [Header("Battery UI")]
    public Slider batterySlider;
    public TextMeshProUGUI batteryText;

    public Color greenColor = Color.green;
    public Color yellowColor = Color.yellow;
    public Color redColor = Color.red;

    [Range(0, 100)]
    public float battery = 100f;

    public float drainInterval = 10f;

    [Header("Game Over")]
    public GameObject gameOverScreen;
    public GameObject chargeSymbol;
    public float flashSpeed = 0.5f;

    [Header("Events")]
    public UnityEvent onBatteryEmpty;
    public UnityEvent onGameOverScreenShown;

    private Image fillImage;
    private float drainTimer;
    private bool batteryDead = false;
    private bool drainPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        fillImage = batterySlider.fillRect.GetComponent<Image>();

        batterySlider.minValue = 0;
        batterySlider.maxValue = 100;

        drainTimer = 0f;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        UpdateBatteryUI();
    }

    void Update()
    {
        // Stop draining once the battery reaches 0
        if (batteryDead || drainPaused)
            return;

        drainTimer += Time.deltaTime;

        if (drainTimer >= drainInterval)
        {
            drainTimer = 0f;
            ChangeBattery(-1f);
        }
    }

    public void ChangeBattery(float amount)
    {
        // Don't allow battery changes after game over
        if (batteryDead)
            return;

        battery += amount;

        battery = Mathf.Clamp(battery, 0f, 100f);

        UpdateBatteryUI();

        // Check if battery reached 0
        if (battery <= 0f)
        {
            BatteryEmpty();
        }
    }
    public void PauseBatteryDrain()
    {
        drainPaused = true;
    }

    public void ResumeBatteryDrain()
    {
        drainPaused = false;
        drainTimer = 0f;
    }
    public void SetBattery(float newBatteryLevel)
    {
        
        if (batteryDead)
            return;

        
        battery = Mathf.Clamp(newBatteryLevel, 0f, 100f);

        
        drainTimer = 0f;

        
        UpdateBatteryUI();

       
        if (battery <= 0f)
        {
            BatteryEmpty();
        }
    }
    void UpdateBatteryUI()
    {
        batterySlider.value = battery;

        batteryText.text = Mathf.RoundToInt(battery) + "%";

        if (battery >= 60)
        {
            fillImage.color = greenColor;
        }
        else if (battery >= 30)
        {
            fillImage.color = yellowColor;
        }
        else
        {
            fillImage.color = redColor;
        }
    }

    void BatteryEmpty()
    {
        if (batteryDead)
            return;

        batteryDead = true;


        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopWeatherGame();
        }

        // EVENTS BEFORE GAME OVER SCREEN
        onBatteryEmpty.Invoke();

        // Start game over sequence
        //StartCoroutine(GameOverSequence());
    }
    public void ResetBattery()
    {
        StopAllCoroutines();

        drainTimer = 0f;
        batteryDead = false;
        drainPaused = false;

        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Main Level":
            case "Calculator Scene":
                battery = 100f;
                break;

            case "Main Menu 2":
            case "WeatherScene":
                battery = 75f;
                break;

            case "Main Menu 3":
            case "SettingScene":
                battery = 50f;
                break;

            default:
                // Fallback if the scene isn't listed
                battery = 100f;
                Debug.LogWarning(
                    "Battery reset in an unrecognized scene: " + currentScene +
                    ". Defaulting to 100%."
                );
                break;
        }

        
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        if (chargeSymbol != null)
        {
            chargeSymbol.SetActive(false);
        }

        UpdateBatteryUI();

        Debug.Log(
            "Battery reset to " + battery + "% in scene: " + currentScene
        );
    }
    IEnumerator GameOverSequence()
    {
        // Wait before showing game over
        yield return new WaitForSeconds(2f);

        // Show Game Over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // EVENTS AFTER GAME OVER SCREEN IS SHOWN
        onGameOverScreenShown.Invoke();

        // Start flashing charge symbol
        if (chargeSymbol != null)
        {
            chargeSymbol.SetActive(true);
            StartCoroutine(FlashChargeSymbol());
        }
    }

    IEnumerator FlashChargeSymbol()
    {
        while (true)
        {
            // Toggle charge symbol on/off
            chargeSymbol.SetActive(!chargeSymbol.activeSelf);

            yield return new WaitForSeconds(flashSpeed);
        }
    }
}