using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stat_Tracker : MonoBehaviour
{
    [SerializeField] private TMP_Text money;
    [SerializeField] private TMP_Text sus;
    [SerializeField] private TMP_Text day;
    private const string dayText = "Day ";
    private const string nightText = "Night ";
    private string currentCycle = "Day ";
    #region EventManager
    public static Stat_Tracker Instance;    // Singleton
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
            { "UpdateTextDay", UpdateDay },
            { "LoadNight", UpdateNight },
            { "UpdateTextSus", UpdateSus },
            { "UpdateTextMoney", UpdateMoney },
        };
    }
    private void OnEnable()
    {
        StartCoroutine("DelayedSubscription");
    }
    private IEnumerator DelayedSubscription()
    {
        yield return new WaitForSeconds(0.001f);
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

    private void UpdateDay(int val)
    {
        currentCycle = dayText;
        day.text = currentCycle+val.ToString();
    }
    private void UpdateNight(int val)
    {
         currentCycle = nightText;
         day.text = currentCycle + val.ToString();
    }
    private void UpdateSus(int val)
    {
        sus.text = "Suspicion: " + val.ToString() + "%";
    }

    private void UpdateMoney(int val)
    {
        money.text = "Money:    $" + val.ToString();
    }
}