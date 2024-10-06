using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    private const int CUSTOMER_PER_DAY = 3;
    private int customerTracker = 0;
    [SerializeField] private Inventory inventory;

    [Header("Animal Counter")]
    [SerializeField] private Transform counter;
    [SerializeField] private GameObject animalPrefab;

    #region EventManager
    private static GameManager Instance;    // Singleton
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new()
        {
            { "Day_SelectAnimal", OnAnimalClick },
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
        foreach(Animal animal in inventory.currentAnimals)
        {
            var a = Instantiate(animalPrefab, counter);
            a.GetComponent<Day_AnimalOption>().animal = animal;
            a.GetComponent<Animator>().Play(animal.GetSpeciesName());
        }
        StartCoroutine(StartLoop());
    }

    private IEnumerator StartLoop()
    {
        yield return new WaitForSeconds(1f);
        customerTracker++;
        EventManager.TriggerEvent("RandomCustomer");
    }

    private IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(2f);
        if (customerTracker >= CUSTOMER_PER_DAY) {
            EventManager.TriggerEvent("NextDayCycle");
        } else {
            customerTracker++;
            EventManager.TriggerEvent("RandomCustomer");
        }
    }

    public void OnAnimalClick(int id)
    {
        // Check if it's presenting to special request
        // Check if it's a disguise pet

        EventManager.TriggerEvent("PresentAnimal", id);

        StartCoroutine(GameLoop());
    }
}