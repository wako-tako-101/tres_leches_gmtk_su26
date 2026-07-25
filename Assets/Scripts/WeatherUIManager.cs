using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WeatherUIManager : MonoBehaviour
{
    public static WeatherUIManager Instance;

    [Header("Gameplay HUD")]
    public GameObject gameplayHUD; // parent object containing hearts and meter

    [Header("Hearts")]
    public Image[] heartIcons;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Water Meter")]
    public Slider waterSlider;
    public TextMeshProUGUI waterText;

    [Header("Round Banner")]
    public GameObject roundBannerPanel;
    public TextMeshProUGUI roundBannerText;

    public void ShowRoundBanner(int roundNumber)
    {
        StopAllCoroutines();
        StartCoroutine(RoundBannerRoutine(roundNumber));
    }

    private IEnumerator RoundBannerRoutine(int roundNumber)
    {
        roundBannerText.text = "Round " + roundNumber;
        roundBannerPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        roundBannerPanel.SetActive(false);
    }

    void Awake()
    {
        Instance = this;
    }

    public void ShowGameplayHUD() => gameplayHUD.SetActive(true);
    public void HideGameplayHUD() => gameplayHUD.SetActive(false);

    public void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < heartIcons.Length; i++)
            heartIcons[i].sprite = i < currentLives ? fullHeart : emptyHeart;
    }

    public void UpdateWaterMeter(int current, int max)
    {
        waterSlider.maxValue = max;
        waterSlider.value = current;
        if (waterText != null) waterText.text = $"{current} / {max}";
    }
}