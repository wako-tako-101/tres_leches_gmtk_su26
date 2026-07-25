using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
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
        if (batteryDead)
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

        // EVENTS BEFORE GAME OVER SCREEN
        onBatteryEmpty.Invoke();

        // Start game over sequence
        //StartCoroutine(GameOverSequence());
    }
    public void ResetBattery()
    {
        StopAllCoroutines();

        battery = 100f;
        drainTimer = 0f;
        batteryDead = false;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        if (chargeSymbol != null)
        {
            chargeSymbol.SetActive(false);
        }

        UpdateBatteryUI();
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