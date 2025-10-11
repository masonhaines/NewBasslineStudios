using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;       // main pause panel
    [SerializeField] private GameObject confirmPanel;     // “Go to main menu?” dialog

    [Header("First Selected")]
    [SerializeField] private GameObject firstPauseButton;   // e.g., Resume button
    [SerializeField] private GameObject firstConfirmButton; // e.g., Yes button

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // set in Inspector

    private bool isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause();
            else
            {
                // If already paused and the confirm dialog is open, close confirm
                if (confirmPanel != null && confirmPanel.activeSelf) HideConfirm();
                else Resume();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;                // freeze gameplay
        AudioListener.pause = true;         // pause audio
        if (pausePanel)  pausePanel.SetActive(true);
        if (confirmPanel) confirmPanel.SetActive(false);

        // select first UI element for gamepad/keyboard navigation
        if (firstPauseButton) EventSystem.current.SetSelectedGameObject(firstPauseButton);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (pausePanel)  pausePanel.SetActive(false);
        if (confirmPanel) confirmPanel.SetActive(false);
    }

    // Called by the “Main Menu” button in the pause panel
    public void ShowConfirm()
    {
        if (confirmPanel) confirmPanel.SetActive(true);
        if (firstConfirmButton) EventSystem.current.SetSelectedGameObject(firstConfirmButton);
    }

    public void HideConfirm()
    {
        if (confirmPanel) confirmPanel.SetActive(false);
        if (firstPauseButton) EventSystem.current.SetSelectedGameObject(firstPauseButton);
    }

    // Called by “Yes” on the confirm dialog
    public void GoToMainMenu()
    {
        // ensure time is normal again
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Called by “Quit Game” (optional)
    public void QuitGame()
    {
        // In Editor this does nothing; in a build it quits.
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Application.Quit();
    }

    // Safety: if this object gets disabled/destroyed, unpause
    void OnDisable()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }
}
