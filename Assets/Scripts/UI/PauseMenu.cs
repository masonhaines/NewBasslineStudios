

// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;

// public class PauseMenu : MonoBehaviour
// {
//     [SerializeField] private GameObject pausePanel;
//     [SerializeField] private GameObject confirmPanel;
//     [SerializeField] private GameObject firstPauseButton;   // ResumeButton
//     [SerializeField] private GameObject firstConfirmButton; // YesButton
//     [SerializeField] private string mainMenuSceneName = "MainMenu";

//     bool isPaused;

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             if (!isPaused) Pause();
//             else if (confirmPanel.activeSelf) HideConfirm();
//             else Resume();
//         }
//     }

//     public void Pause()
//     {
//         isPaused = true;
//         Time.timeScale = 0f;
//         AudioListener.pause = true;
//         pausePanel.SetActive(true);
//         confirmPanel.SetActive(false);
//         if (firstPauseButton) EventSystem.current.SetSelectedGameObject(firstPauseButton);
//     }
//     public void Resume()
//     {
//         isPaused = false;
//         Time.timeScale = 1f;
//         AudioListener.pause = false;
//         pausePanel.SetActive(false);
//         confirmPanel.SetActive(false);
//     }
//     public void ShowConfirm()
//     {
//         confirmPanel.SetActive(true);
//         if (firstConfirmButton) EventSystem.current.SetSelectedGameObject(firstConfirmButton);
//     }
//     public void HideConfirm() => confirmPanel.SetActive(false);

//     public void GoToMainMenu()
//     {
//         Time.timeScale = 1f;
//         AudioListener.pause = false;
//         SceneManager.LoadScene(mainMenuSceneName);
//     }
//     public void QuitGame()
//     {
//         Time.timeScale = 1f;
//         AudioListener.pause = false;
//         // Application.Quit();

//         #if UNITY_EDITOR
//     // In the Editor, stop Play mode:
//     UnityEditor.EditorApplication.isPlaying = false;
// #else
//     // In a build, exit the app:
//     Application.Quit();
// #endif
//     }


// }

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject firstPauseButton;   // ResumeButton
    [SerializeField] private GameObject firstConfirmButton; // YesButton
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    bool isPaused;

    void Update()
    {
        
        // if (FinalLevelExitFade.GameCompleted)
        //     return;


        if (FinalLevelExitFade.GameCompleted)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoToMainMenu();
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause();
            else if (confirmPanel.activeSelf) HideConfirm();
            else Resume();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        pausePanel.SetActive(true);
        confirmPanel.SetActive(false);
        if (firstPauseButton) EventSystem.current.SetSelectedGameObject(firstPauseButton);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pausePanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    public void ShowConfirm()
    {
        confirmPanel.SetActive(true);
        if (firstConfirmButton) EventSystem.current.SetSelectedGameObject(firstConfirmButton);
    }

    public void HideConfirm() => confirmPanel.SetActive(false);

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
