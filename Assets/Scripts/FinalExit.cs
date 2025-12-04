using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinalLevelExitFade : MonoBehaviour
{
    public static bool GameCompleted = false;   // used by other scripts (pause, etc.)

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;    // full-screen black image
    [SerializeField] private float fadeDuration = 1f;

    [Header("End Screen")]
    [SerializeField] private GameObject endScreen;  // panel with your scroll + button

    private bool triggered = false;

    private void Start()
    {
        GameCompleted = false;

        // make sure end screen starts hidden
        if (endScreen != null)
            endScreen.SetActive(false);

        // make sure fade starts transparent
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(FadeAndShowEnd());
    }

    private IEnumerator FadeAndShowEnd()
    {
        // 1. Fade to black
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            float t = 0f;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float a = Mathf.Clamp01(t / fadeDuration);
                c.a = a;
                fadeImage.color = c;
                yield return null;
            }
        }

        // 2. Show end screen (scroll + button)
        if (endScreen != null)
            endScreen.SetActive(true);

        // 3. Mark game as finished + pause
        GameCompleted = true;
        Time.timeScale = 0f;
    }

    // 4. Called by the Main Menu button
    public void OnMainMenuButton()
    {
        GameCompleted = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");   // <- put your main menu scene name here
    }
}
