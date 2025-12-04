using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private Image fadeImage;   // your black full-screen image
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        if (fadeImage != null)
        {
            // start fully black
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;

            StartCoroutine(FadeInRoutine());
        }
    }

    private IEnumerator FadeInRoutine()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;    // works even if timescale was weird
            float a = 1f - Mathf.Clamp01(t / fadeDuration);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }
}
