using UnityEngine;

public class WeatherAudioManager : MonoBehaviour
{
    public static WeatherAudioManager Instance;

    [Header("Weather Sounds")]
    public AudioSource rainSplashAudio;
    public AudioSource lightningStruckAudio;

    [Header("Battery Sounds")]
    public AudioSource batteryDrainingAudio;

    private void Awake()
    {
        Instance = this;
    }

    // Rain hits umbrella
    public void PlayRainSplash()
    {
        if (rainSplashAudio != null)
            rainSplashAudio.Play();
    }

    // Player gets hit by lightning
    public void PlayLightningStruck()
    {
        if (lightningStruckAudio != null)
            lightningStruckAudio.Play();
    }

    // Battery starts draining
    public void PlayBatteryDraining()
    {
        if (batteryDrainingAudio != null)
            batteryDrainingAudio.Play();
    }

    // Stop battery draining sound
    public void StopBatteryDraining()
    {
        if (batteryDrainingAudio != null)
            batteryDrainingAudio.Stop();
    }
    public void StopAllAudio()
    {
        if (rainSplashAudio != null)
            rainSplashAudio.Stop();

        if (lightningStruckAudio != null)
            lightningStruckAudio.Stop();

        if (batteryDrainingAudio != null)
            batteryDrainingAudio.Stop();
    }
}