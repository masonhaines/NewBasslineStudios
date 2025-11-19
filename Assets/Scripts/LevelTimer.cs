using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelTimer : MonoBehaviour
{
    [SerializeField] private bool countDown = false;
    [SerializeField] private float startTimeSeconds = 120f;

    private float timeValue;
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        // Always grab the text on the same object cuz otherwize i get 293i1299312938 erors per second 
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
        if (timerText == null) return; // extra safety 23u8192793812 eorors on console if this is not here 

        int minutes = Mathf.FloorToInt(timeValue / 60f);
        int seconds = Mathf.FloorToInt(timeValue % 60f);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
