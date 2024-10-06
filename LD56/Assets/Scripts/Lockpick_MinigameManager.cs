using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lockpick_MinigameManager : MonoBehaviour
{
    private const int STEAL_SUCCESS_SUSPICION = 10;
    private const int STEAL_FAIL_SUSPICION = 25;

    [SerializeField] private int MAX_ATTEMPTS = 3;
    private int lives = 0;

    [Header("Lives")]
    [SerializeField] private Transform livesContainer;
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private Sprite activeCross;
    [SerializeField] private Sprite deactiveCross;
    private List<GameObject> crosses = new();

    [Header("Timer")]
    private const float TIMER_SECONDS = 90f;
    private float time = 0;
    [SerializeField] private Slider timerSlider;
    private string timer = null;

    [SerializeField] private GameObject overlay;

    [SerializeField] private Image digitalLock;
    private Vector2 startpos;
    private Vector2 endpos;
    private float dropDuration = 2f;

    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new() {
            { "LoseMinigameAttempt", LoseLife },
            { "WinMinigame", Win },
        }; 
    }
    private void OnEnable()
    {
        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            GameObject c = Instantiate(crossPrefab, livesContainer);
            crosses.Add(c);
        }
        ResetTimer();
        Reset();
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

    private void Reset()
    {
        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            SetSprite(deactiveCross, i);
        }
        lives = 0;
    }

    public void ResetTimer()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        timerSlider.value = 1;
        time = TIMER_SECONDS;
        timer = "UpdateTimer";
        StartCoroutine(timer);
    }

    private IEnumerator UpdateTimer()
    {
        const float frequency = 1.0f;
        while (time >= 0)
        {
            yield return new WaitForSeconds(frequency);
            time -= frequency;
            timerSlider.value = time / TIMER_SECONDS;
        }
        LoseGame();
    }

    private void SetSprite(Sprite s, int index)
    {
        var currentLife = crosses[index];
        currentLife.GetComponent<Image>().sprite = s;
        crosses[index] = currentLife;
    }

    public void LoseLife(int val = 0)
    {
        SetSprite(activeCross, lives);
        lives++;

        if (lives < MAX_ATTEMPTS)
        {
            // overlay.SetActive(true);
            return;
        }

        // TODO: Show player failed, pause and ask for retry or give up
        EventManager.TriggerEvent("AddSuspicion", STEAL_FAIL_SUSPICION);
        SceneLoader.LoadGameplayDay();
    }

    public void LoseGame()
    {
        SceneLoader.LoadGameplayDay();
    }

    private IEnumerator DropLock()
    {
        digitalLock.transform.DOMoveY(-85, dropDuration);
        
        yield return new WaitForSeconds(3f);
        EventManager.TriggerEvent("AddSuspicion", STEAL_SUCCESS_SUSPICION);
        SceneLoader.LoadGameplayDay();
    }

    public void Win(int val = 0)
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }

        // TODO: Show player win
        StartCoroutine("DropLock");
    }
}
