using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Level2";
    [SerializeField] private ScreenFade screenFade;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (screenFade != null)
            {
                screenFade.FadeToScene(nextSceneName);
            }
            else
            {
                // fallback in case fade isn't set but this should work on all levels cuz i pushed and text it 
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
