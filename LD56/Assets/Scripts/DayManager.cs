using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    private const int CUSTOMER_PER_DAY = 3;
    private int customerTracker = 0;

    private const float ENTER_DURATION = 1.5f;
    private const float DAILYSUS_DURATION = 3f;
    private bool isExoticRequest = false;
    private bool isServingCustomer = false;

    [SerializeField] private Inventory inventory;

    [SerializeField] private GameObject animalDisplay;
    [SerializeField] private GameObject tornadoAnim;
    [SerializeField] private List<Animal> exoticAnimals;

    [Header("Animal Counter")]
    [SerializeField] private Transform counter;
    [SerializeField] private GameObject animalPrefab;

    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new()
        {
            { "Day_SelectAnimal", ServeCustomer },
            { "ChoseBluff", Tornado },
            { "DisguiseAnimal", DisguiseAnimal },
        };
    }
    private void OnEnable()
    {
        customerTracker = 0;
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

        // If Not Day 0, Suspicious Person comes first
        isExoticRequest = inventory.day > 1;
        if (isExoticRequest) {
            CallSusCustomer();
        }
        else {  
            CallRandomCustomer();
        }
        yield return new WaitForSeconds(ENTER_DURATION);
        isServingCustomer = true;
    }

    private IEnumerator GameLoop()
    {
        if (customerTracker >= CUSTOMER_PER_DAY) {
            CallSusCustomer();
            int id = inventory.day - 1;
            inventory.requestAnimal = new Inventory.Request(exoticAnimals[id], 100 + 50 * (id));
            yield return new WaitForSeconds(DAILYSUS_DURATION);
            EventManager.TriggerEvent("NextDayCycle");
        } else {
            CallRandomCustomer();
            yield return new WaitForSeconds(ENTER_DURATION);
            isServingCustomer = true;
        }
    }

    private void CallRandomCustomer()
    {
        customerTracker++;
        EventManager.TriggerEvent("RandomCustomer");

        if (isExoticRequest) { isExoticRequest = false; }
    }
    private void CallSusCustomer()
    {
        EventManager.TriggerEvent("SpecialCustomer", (isExoticRequest ? 0 : 1));
    }

    public void ServeCustomer(int id)
    {
        if (!isServingCustomer) { return; }
        EventManager.TriggerEvent("PresentAnimal", id);
        StartCoroutine(PlaceAnimal(id, isExoticRequest ? DAILYSUS_DURATION : ENTER_DURATION));
        isServingCustomer = false;
    }

    private IEnumerator PlaceAnimal(int id, float duration)
    {
        // Set Animation
        animalDisplay.SetActive(true);
        animalDisplay.GetComponent<Animator>().Play(inventory.GetName(id));
        var v = animalDisplay.transform.localPosition;

        yield return new WaitForSeconds(duration);

        // Going Home with New Owner
        animalDisplay.transform.DOMoveX(-1920 / 2, ENTER_DURATION);

        yield return new WaitForSeconds(ENTER_DURATION);

        // Clean up
        animalDisplay.SetActive(false);
        animalDisplay.transform.localPosition = v;

        StartCoroutine(GameLoop());
    }

    #region Suspicious Behaviour
    public void Tornado(int val) { tornadoAnim.SetActive(true); }

    public void DisguiseAnimal(int val)
    {
        Animator a = animalDisplay.GetComponent<Animator>();
        // TODO: make sure each animal has D animation
        string currentName = a.GetCurrentAnimatorClipInfo(0)[0].clip.name + "D";
        a.Play(currentName);
    }
    #endregion
}