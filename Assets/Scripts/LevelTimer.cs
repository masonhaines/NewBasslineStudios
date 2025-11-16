using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelTimer : MonoBehaviour
{
    public bool countDown = false;
    public float startTimeSeconds = 120f;

    private float timeValue;
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        timeValue = countDown ? startTimeSeconds : 0f;
    }

    private void Update()
    {
        if (countDown)
        {
            timeValue -= Time.deltaTime;
            if (timeValue < 0f) timeValue = 0f;
        }
        else
        {
            timeValue += Time.deltaTime;
        }

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeValue / 60f);
        int seconds = Mathf.FloorToInt(timeValue % 60f);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
