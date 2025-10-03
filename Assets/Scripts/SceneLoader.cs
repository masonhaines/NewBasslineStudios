using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadFirstLevel() { SceneManager.LoadScene(1); } // assumes Lv1 is build index 1
    public void QuitGame() {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// [RequireComponent(typeof(Button))]
// public class SceneLoader : MonoBehaviour
// {
//     [SerializeField] private string sceneName;

//     public void Load()
//     {
//         if (Application.CanStreamedLevelBeLoaded(sceneName))
//             SceneManager.LoadScene(sceneName);
//         else
//             Debug.LogWarning($"Scene '{sceneName}' is not in Build Settings (or not created yet).");
//     }

//     void Awake()
//     {
//         // Auto-disable if scene isn’t available yet
//         var btn = GetComponent<Button>();
//         if (btn) btn.interactable = Application.CanStreamedLevelBeLoaded(sceneName);
//     }
// }
