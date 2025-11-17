using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;

    public void TriggerEndGame()
    {
        endGamePanel.SetActive(true);
        Time.timeScale = 0f; // freeze game
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
