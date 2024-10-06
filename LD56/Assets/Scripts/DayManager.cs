using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    private const int CUSTOMER_PER_DAY = 4;
    private int customerTracker = 0;
    #region EventManager
    private static GameManager Instance;    // Singleton
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new()
        {
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
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(1f);
        if (customerTracker >= CUSTOMER_PER_DAY)
        {
            EventManager.TriggerEvent("NextDayCycle");
        }
        customerTracker++;
        EventManager.TriggerEvent("RandomCustomer");
    }

    public void OnAnimalClick()
    {
        // Check if it's presenting to special request

        EventManager.TriggerEvent("PresentAnimal");
    }
}