using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;

public class SceneLoader : MonoBehaviour
{
    private static Animator sceneAnimator;
    private static string nextLevel;

    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void OnEnable()
    {
        StartCoroutine("DelayedSubscription");
    }
    private IEnumerator DelayedSubscription()
    {
        yield return new WaitForSeconds(0.0001f);
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StartListening(pair.Key, pair.Value);
        }
    }

    private void OnDisable()
    {
        if (SubscribedEvents == null) { return; }
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StopListening(pair.Key, pair.Value);
        }
    }
    #endregion

    private void Awake()
    {
        sceneAnimator = GetComponent<Animator>();
        SubscribedEvents = new() {
            { "StartMinigameLockpick", LoadMinigameLockpick },
            { "StartMinigameFishing", LoadMinigameFishing },
            { "StartMinigameDino", LoadMinigameDino },
            { "LoadNight", LoadGameplayNight },
        };
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
        EventManager.TriggerEvent("NextDay");
        FadeLevel("Gameplay_Day");
    }

    public static void LoadGameplayNight(int index = 0)
    {
        FadeLevel("Gameplay_Night");
    }

    public static void LoadMinigameLockpick(int index = 0)
    {
        FadeLevel("Minigame_Lockpick");
    }

    public static void LoadMinigameFishing(int index = 0)
    {
        FadeLevel("Minigame_Fishing");
    }

    public static void LoadMinigameDino(int index = 0)
    {
        FadeLevel("Minigame_Dino");
    }

    public static void LoadEnding()
    {
        FadeLevel("Gameplay_Ending");
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }
}