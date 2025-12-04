using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    public float fadeDuration = 1f;

    private Image fadeImage;
    private bool isFading = false;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;           // start fully transparent so its not blocking the level before fade
            fadeImage.color = c;
        }
    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeRoutine(sceneName));
        }
    }

    private System.Collections.IEnumerator FadeRoutine(string sceneName)
    {
        isFading = true;

        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
