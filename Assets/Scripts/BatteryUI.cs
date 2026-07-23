using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    public Slider batterySlider;
    public TextMeshProUGUI batteryText;

    public Color greenColor = Color.green;
    public Color yellowColor = Color.yellow;
    public Color redColor = Color.red;

    [Range(0, 100)]
    public float battery = 100f;

    public float drainInterval = 10f;

    private Image fillImage;
    private float drainTimer;

    void Start()
    {
        fillImage = batterySlider.fillRect.GetComponent<Image>();

        batterySlider.minValue = 0;
        batterySlider.maxValue = 100;

        drainTimer = 0f;

        UpdateBatteryUI();
    }

    void Update()
    {
        drainTimer += Time.deltaTime;

        if (drainTimer >= drainInterval)
        {
            drainTimer = 0f;
            ChangeBattery(-1f);
        }
    }

    public void ChangeBattery(float amount)
    {
        battery += amount;

        battery = Mathf.Clamp(battery, 0f, 100f);

        UpdateBatteryUI();
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
}