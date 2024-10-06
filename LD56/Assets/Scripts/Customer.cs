using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private const int CORRECT_MONEY = 10;
    private const int INCORRECT_SUS = 10;

    private int animalID = 0;
    [SerializeField] private Inventory inventory;

    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new() {
            { "RandomCustomer", Generate },
            { "SpecialCustomer", GenerateSpecial },
            { "PresentAnimal", React },
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

    private void Generate(int val)
    {
        animalID = UnityEngine.Random.Range(0, inventory.NORMAL_COUNT);
    }

    private void GenerateSpecial(int val)
    {
        // animalID = day
    }

    private void React(int id)
    {
        if (id == animalID)
        {
            EventManager.TriggerEvent("AddMoney", CORRECT_MONEY);
        } else
        {
            EventManager.TriggerEvent("AddSuspicion", INCORRECT_SUS);
        }

        // Trigger Leave Animation
    }
}
