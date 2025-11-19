using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;              // unpause the game
        SceneManager.LoadScene("MainMenu");  //  dont change the name
    }
}
