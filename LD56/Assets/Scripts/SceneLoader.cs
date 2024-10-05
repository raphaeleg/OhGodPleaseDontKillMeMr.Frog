using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public static void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public static void LoadOpeningScene()
    {
        LoadScene("Cutscene_Opening");
    }

    public static void LoadGameplay()
    {
        LoadScene("Gameplay");
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }
}