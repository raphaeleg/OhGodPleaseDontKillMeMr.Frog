using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    private const int STEAL_SUCCESS_SUSPICION = 10;
    private const int STEAL_FAIL_SUSPICION = 25;

    [SerializeField] private int MAX_ATTEMPTS = 3;
    private int lives = 0;

    [Header("Lives")]
    [SerializeField] private Transform livesContainer;
    [SerializeField] private GameObject crossPrefab;
    private List<GameObject> crosses = new();

    [Header("Timer")]
    private const float TIMER_SECONDS = 10f;
    private float time = 0;
    [SerializeField] private Slider timerSlider;
    private string activeCoroutine = null;

    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject winOverlay;
    [SerializeField] private GameObject loseOverlay;

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
        for (int i = 0; i < MAX_ATTEMPTS; i++) {
            SetSprite(true, i);
        }
        lives = 0;
    }

    public void ResetTimer()
    {
        DeactivateTimerCoroutine();
        timerSlider.value = 1;
        time = TIMER_SECONDS;
        activeCoroutine = "UpdateTimer";
        ActivateTimerCoroutine();
    }

    private IEnumerator UpdateTimer()
    {
        const float frequency = 1.0f;
        while (time >= 0)
        {
            yield return new WaitForSeconds(frequency);
            time -= frequency;
            timerSlider.value = time/TIMER_SECONDS;
        }
        LoseLife();
        EventManager.TriggerEvent("MinigameTimerDeplete");
    }

    private void SetSprite(bool active, int index)
    {
        var currentLife = crosses[index];
        currentLife.SetActive(active);
        crosses[index] = currentLife;
    }

    public void LoseLife(int val = 0)
    {
        DeactivateTimerCoroutine();
        SetSprite(false, lives);
        lives++;

        if (lives < MAX_ATTEMPTS) {
            overlay.SetActive(true);
            return;
        }
        EventManager.TriggerEvent("LoseMinigame");

        loseOverlay.SetActive(true);

        EventManager.TriggerEvent("AddSuspicion", STEAL_FAIL_SUSPICION);
    }

    public void Win(int val = 0)
    {
        DeactivateTimerCoroutine();

        winOverlay.SetActive(true);

        EventManager.TriggerEvent("AddSuspicion", STEAL_SUCCESS_SUSPICION);
    }

    private void DeactivateTimerCoroutine()
    {
        if (activeCoroutine == null) { return; }
        StopCoroutine(activeCoroutine);
        activeCoroutine = null;
    }
    private void ActivateTimerCoroutine()
    {
        if (activeCoroutine != null)
        {
            DeactivateTimerCoroutine();
        }
        activeCoroutine = "UpdateTimer";
        StartCoroutine(activeCoroutine);
    }
}