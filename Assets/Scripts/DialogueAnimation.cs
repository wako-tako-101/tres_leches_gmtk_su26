using System.Collections;
using UnityEngine;

public class DialogueAnimation : MonoBehaviour
{
    public RectTransform panel;
    public float duration = 0.25f;

    Vector2 shownPos;
    Vector2 hiddenPos;

    private void Start()
    {
        shownPos = panel.anchoredPosition;
        hiddenPos = shownPos + new Vector2(1000, 0);

        panel.anchoredPosition = hiddenPos;
    }

    public void NotificationIn()
    {
        StartCoroutine(Slide(hiddenPos, shownPos));
    }

    public void NotificationOut()
    {
        StartCoroutine(Slide(shownPos, hiddenPos));
    }

    IEnumerator Slide(Vector2 from, Vector2 to)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            panel.anchoredPosition = Vector2.Lerp(from, to, t / duration);
            yield return null;
        }

        panel.anchoredPosition = to;
    }
}