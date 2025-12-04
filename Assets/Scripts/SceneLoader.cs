

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] string sceneToLoad;

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn && !string.IsNullOrEmpty(sceneToLoad))
        {
            btn.onClick.AddListener(() => LoadByName(sceneToLoad));
            btn.interactable = Application.CanStreamedLevelBeLoaded(sceneToLoad);
        }
    }


    public void LoadLevel1() => LoadByName("Level11");  // cuz my lv 1 is 11 
    public void LoadLevel2() => LoadByName("Level2");
    public void LoadLevel3() => LoadByName("Level3");

    // Generic loaders
    public void LoadByIndex(int buildIndex)
    {
        if (buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(buildIndex);
        else
            Debug.LogWarning($"Build index {buildIndex} isn't in Build Settings.");
    }

    public void LoadByName(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogWarning($"Scene '{sceneName}' is not in Build Settings or not saved.");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

