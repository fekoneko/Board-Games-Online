using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviour
{
    [SerializeField] private Image fill;
    private Slider slider;
    private float timerDuration = 0.0f;
    private float timerStartTime = 0.0f;

    void Awake()
    {
        slider = GetComponent<Slider>();
        fill.enabled = false;
    }

    void Update()
    {
        if (fill.enabled)
        {
            float timerRemainTime = timerDuration - (Time.realtimeSinceStartup - timerStartTime);
            if (timerRemainTime >= 0.0f)
            {
                slider.value = timerRemainTime / timerDuration;
            }
            else
            {
                fill.enabled = false;
            }
        }
    }

    public void ShowTimer(float duration)
    {
        timerDuration = duration;
        timerStartTime = Time.realtimeSinceStartup;
        fill.enabled = true;
        slider.value = 1.0f;
    }

    public void HideTimer()
    {
        timerDuration = 0.0f;
        timerStartTime = 0.0f;
        fill.enabled = false;
    }
}
