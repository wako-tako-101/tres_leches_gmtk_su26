using System;
using TMPro;
using UnityEngine;

public class RealTimeClock : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTime), 0f, 1f);
    }

    void UpdateTime()
    {
        timeText.text = DateTime.Now.ToString("h:mm tt");
    }
}