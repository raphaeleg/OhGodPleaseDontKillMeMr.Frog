using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    private const int MAX_ATTEMPTS = 3;
    private int lives = 0;
    [SerializeField] private Transform livesContainer;
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private Sprite activeCross;
    [SerializeField] private Sprite deactiveCross;
    private List<GameObject> crosses = new();

    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new() {
            { "FailedMinigame", LoseLife },
            { "WinMinigame", Win },
        };
    }
    private void OnEnable()
    {
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StartListening(pair.Key, pair.Value);
        }
        Reset();
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

    private void Start()
    {
        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            GameObject c = Instantiate(crossPrefab, livesContainer);
            crosses.Add(c);
        }
    }

    private void Reset()
    {
        for (int i = 0; i < MAX_ATTEMPTS; i++) {
            SetSprite(deactiveCross, i);
        }
        lives = 0;
    }

    public void LoseLife(int val)
    {
        SetSprite(activeCross, lives);

        lives++;
        if (lives >= MAX_ATTEMPTS)
        {
            // TODO: Show player failed,
            SceneLoader.LoadGameplayDay();
            return;
        }
        // TODO: Replay minigame
    }

    private void SetSprite(Sprite s, int index)
    {
        var currentLife = crosses[index];
        currentLife.GetComponent<Image>().sprite = s;
        crosses[index] = currentLife;
    }

    private void Win(int val)
    {
        // TODO: Show player win
        SceneLoader.LoadGameplayDay();
    }
}
