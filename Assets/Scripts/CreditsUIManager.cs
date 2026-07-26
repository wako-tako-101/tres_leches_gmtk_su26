using UnityEngine;

public class CreditsUIManager : MonoBehaviour
{
    public static CreditsUIManager Instance;

    [Header("Credits")]
    public GameObject creditsUI;

    void Awake()
    {
        Instance = this;
    }

    public void ShowCredits() => creditsUI.SetActive(true);
    public void HideCredits() => creditsUI.SetActive(false);
}
