using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum DayCycle { DAY, NIGHT };

    private int day = 0;
    private DayCycle cycle = DayCycle.DAY;

    [SerializeField] private Inventory inventory;

    #region EventManager
    private static GameManager Instance;    // Singleton
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        SubscribedEvents = new() {
            { "NextDay", AddDay },
            { "NextDayCycle", ToggleCycle },
        };
    }
    private void OnEnable()
    {
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StartListening(pair.Key, pair.Value);
        }
    }

    private void OnDisable()
    {
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StopListening(pair.Key, pair.Value);
        }
    }
    #endregion

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        day = 0;
        cycle = DayCycle.DAY;
        inventory.Reset();
    }

    private void AddDay(int val) { day++; }
    private void ToggleCycle(int val) {
        if (cycle == DayCycle.DAY) { cycle = DayCycle.NIGHT; }
        else { cycle = DayCycle.DAY; }
    }
}
