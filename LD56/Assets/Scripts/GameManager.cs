using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum DayCycle { DAY, NIGHT };

    private DayCycle cycle = DayCycle.DAY;

    [SerializeField] private Inventory inventory;

    private const int MAX_SUSPICION = 100;
    [SerializeField] private int suspicion = 0;

    [SerializeField] private int money = 0;

    #region EventManager
    public static GameManager Instance;    // Singleton
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
            { "AddSuspicion", AddSuspicion },
            { "SubtractSuspicion", SubtractSuspicion },
            { "AddMoney", AddMoney },
            { "SubtractMoney", SubtractMoney },
            { "WinMinigame", GainExoticAnimal }
        };
    }
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

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        cycle = DayCycle.NIGHT;
        suspicion = 0;
        money = 0;
        inventory.Reset();
        Debug.Log("Reset: "+inventory.day);
    }

    private void AddDay(int val) {
        ToggleCycle();
        inventory.day++;
        inventory.requestAnimal.Clear();
        EventManager.TriggerEvent("UpdateTextDay", inventory.day);
    }
    private void ToggleCycle(int val = 0) {
        if (cycle == DayCycle.DAY) { 
            cycle = DayCycle.NIGHT;
            EventManager.TriggerEvent("LoadNight", inventory.day);
        }
        else { cycle = DayCycle.DAY; }
    }
    private void AddSuspicion(int val)
    {
        suspicion += val;
        EventManager.TriggerEvent("UpdateTextSus", suspicion);
        if (suspicion >= MAX_SUSPICION)
        {
            // TODO: Lose condition
        }
    }
    private void SubtractSuspicion(int val) {
        suspicion -= 10;
        EventManager.TriggerEvent("UpdateTextSus", suspicion);
    }
    private void AddMoney(int val) {
        money += val; 
        EventManager.TriggerEvent("UpdateTextMoney", money);
    }
    private void SubtractMoney(int val) { 
        money -= val;
        EventManager.TriggerEvent("UpdateTextMoney", money);
        if (money < 0)
        {
            // TODO: Lose condition
        }
    }
    private void GainExoticAnimal(int val) { inventory.GainExoticAnimal();  }

    public int getMoney()
    {
        return money;
    }

    public int getSuspicion()
    {
        return suspicion;
    }
}
