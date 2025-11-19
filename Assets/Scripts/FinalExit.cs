// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class FinalLevelExitFade : MonoBehaviour
// {
//     [SerializeField] private Image fadeImage;        // EndFadeImage
//     [SerializeField] private GameObject endGamePanel;
//     [SerializeField] private float fadeDuration = 1f;

//     private bool triggered = false;

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (triggered) return;
//         if (!other.CompareTag("Player")) return;

//         triggered = true;
//         StartCoroutine(FadeAndShowPanel());
//     }

//     private IEnumerator FadeAndShowPanel()
//     {
//         // Fade to black
//         if (fadeImage != null)
//         {
//             Color c = fadeImage.color;
//             float t = 0f;

//             while (t < fadeDuration)
//             {
//                 t += Time.deltaTime;
//                 float a = Mathf.Clamp01(t / fadeDuration);
//                 c.a = a;
//                 fadeImage.color = c;
//                 yield return null;
//             }
//         }

//         // Show end game panel + pause game
//         if (endGamePanel != null)
//         {
//             endGamePanel.SetActive(true);
//         }

//         Time.timeScale = 0f;
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalLevelExitFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;        // EndFadeImage (or ScreenFadeG)
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private float fadeDuration = 1f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(FadeAndShowPanel());
    }

    private IEnumerator FadeAndShowPanel()
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

        // 2. Show end game panel
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
        }

        // 3. Put fade image away so panel is visible
        if (fadeImage != null)
        {
            fadeImage.enabled = false;   // hides the black image
        }

        // 4. Pause game
        Time.timeScale = 0f;
    }
}
