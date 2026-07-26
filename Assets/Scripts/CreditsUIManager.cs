using UnityEngine;
using UnityEngine.UI;
public class CreditsUIManager : MonoBehaviour
{
    public static CreditsUIManager Instance;

    [Header("Credits")]
    public GameObject creditsUI;

    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f;


    void Awake()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        Instance = this;
    }

    private void Update()
    {
        scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
    }

    public void ShowCredits() => creditsUI.SetActive(true);
    public void HideCredits() => creditsUI.SetActive(false);
}
