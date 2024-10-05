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

    public static void LoadGameplayDay()
    {
        LoadScene("Gameplay_Day");
    }

    public static void LoadGameplayNight()
    {
        LoadScene("Gameplay_Night");
    }

    public static void LoadSettings()
    {
        LoadScene("Settings");
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }
}