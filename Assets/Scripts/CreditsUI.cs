using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CreditEntry
{
    public string label;
    public string value;
}

[System.Serializable]
public class CreditSection
{
    public string headerTitle;
    public List<CreditEntry> entries;
}

public class CreditsUI : MonoBehaviour
{
    [Header("Data")]
    public List<CreditSection> sections;

    [Header("Prefabs")]
    public GameObject sectionHeaderPrefab;
    public GameObject creditRowPrefab;
    public Transform contentParent;

    [Header("Scroll")]
    public ScrollRect scrollRect;
    public float autoScrollSpeed = 20f;
    public float autoScrollStartDelay = 1f;

    private RectTransform content;
    private RectTransform viewport;
    private bool isAutoScrolling = false;

    void OnEnable()
    {
        BuildList();
        content = scrollRect.content;
        viewport = scrollRect.viewport;
        content.anchoredPosition = Vector2.zero; 

        CancelInvoke(nameof(StartAutoScroll));
        Invoke(nameof(StartAutoScroll), autoScrollStartDelay);
    }

    void StartAutoScroll() => isAutoScrolling = true;

    void BuildList()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (CreditSection section in sections)
        {
            Instantiate(sectionHeaderPrefab, contentParent)
                .GetComponentInChildren<TextMeshProUGUI>().text = section.headerTitle;

            foreach (CreditEntry entry in section.entries)
            {
                GameObject row = Instantiate(creditRowPrefab, contentParent);
                TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = entry.label;
                texts[1].text = entry.value;
            }
        }
    }

    void Update()
    {
        if (!isAutoScrolling) return;

        float maxScroll = Mathf.Max(0, content.rect.height - viewport.rect.height);
        float newY = Mathf.Min(content.anchoredPosition.y + autoScrollSpeed * Time.deltaTime, maxScroll);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, newY);
        if (newY >= maxScroll) isAutoScrolling = false;
    }

    public void OnUserDragStart()
    {
        isAutoScrolling = false;
        CancelInvoke(nameof(StartAutoScroll));
    }
}