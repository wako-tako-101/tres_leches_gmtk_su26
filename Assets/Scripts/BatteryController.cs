using UnityEngine;

public class BatteryController : MonoBehaviour
{
    private BatteryUI batteryUI;

    private void Start()
    {
        batteryUI = FindFirstObjectByType<BatteryUI>();

        if (batteryUI == null)
        {
            Debug.LogWarning("BatteryUI could not be found.");
        }
    }

    public void SetBattery(float amount)
    {
        if (batteryUI != null)
        {
            batteryUI.SetBattery(amount);
        }
    }

    public void PauseBatteryDrain()
    {
        if (batteryUI != null)
        {
            batteryUI.PauseBatteryDrain();
        }
    }

    public void ResumeBatteryDrain()
    {
        if (batteryUI != null)
        {
            batteryUI.ResumeBatteryDrain();
        }
    }
}