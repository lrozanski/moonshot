using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private const string TotalShots = "TotalShots";

    public static LevelManager Instance { get; private set; }

    private void Start() {
        Instance = this;
    }

    public static void NextLevel() {
        var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        var nextSceneName = SceneUtility.GetScenePathByBuildIndex(nextIndex);

        if (SceneUtility.GetScenePathByBuildIndex(nextIndex).Length == 0) {
            Debug.Log("Game Over");

#if UNITY_EDITOR
            if (EditorApplication.isPlaying) {
                EditorApplication.ExitPlaymode();
            } else {
                Application.Quit();
            }
#else
            Application.Quit();
#endif
        } else {
            var totalShots = PlayerPrefs.GetInt(TotalShots, 0) + GameManager.Instance.Shots;

            PlayerPrefs.SetInt(TotalShots, totalShots);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
