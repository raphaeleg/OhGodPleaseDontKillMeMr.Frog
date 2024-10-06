using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static Animator sceneAnimator;
    private static string nextLevel;

    private void Awake()
    {
        sceneAnimator = GetComponent<Animator>();
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public static void FadeLevel (string name)
    {
        nextLevel = name;
        sceneAnimator.SetTrigger("Fade");
    }

    public static void LoadMainMenu()
    {
        FadeLevel("MainMenu");
    }

    public static void LoadOpeningScene()
    {
        FadeLevel("Cutscene_Opening");
    }

    public static void LoadGameplayDay()
    {
        FadeLevel("Gameplay_Day");
    }

    public static void LoadGameplayNight()
    {
        FadeLevel("Gameplay_Night");
    }

    public static void LoadSettings()
    {
        FadeLevel("Settings");
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }
}