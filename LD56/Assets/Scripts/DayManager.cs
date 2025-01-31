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

    private const float ENTER_DURATION = 1.0f;
    private const float DAILYSUS_DURATION = 2f;
    private const float INSTRUCTION_DURATION = 5f;
    private bool isExoticRequest = false;
    private bool isServingCustomer = false;

    [SerializeField] private Inventory inventory;

    [SerializeField] private GameObject animalDisplay;
    [SerializeField] private GameObject tornadoAnim;
    [SerializeField] private GameObject spotlight;
    [SerializeField] private List<Animal> exoticAnimals;

    [Header("Instructions")]
    [SerializeField] private GameObject scrollInstruction;
    [SerializeField] private GameObject selectInstruction;
    [SerializeField] private GameObject moneyInstruction;
    [SerializeField] private GameObject suspicionInstruction;
    [SerializeField] private GameObject bluffInstruction;

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
        EventManager.TriggerEvent("ChangeMusic", (int)Audio_MusicArea.DAY);
        foreach(Animal animal in inventory.currentAnimals)
        {
            var a = Instantiate(animalPrefab, counter);
            a.GetComponent<Day_AnimalOption>().animal = animal;
            a.GetComponent<Animator>().Play(animal.GetSpeciesName());
        }
        EventManager.TriggerEvent("UpdateTextDay", inventory.day);
        EventManager.TriggerEvent("UpdateTextSus", GameManager.Instance.getSuspicion());
        EventManager.TriggerEvent("UpdateTextMoney", GameManager.Instance.getMoney());
        StartCoroutine(StartLoop());
    }

    private IEnumerator StartLoop()
    {
        if (inventory.day == 1)
        {
            StartCoroutine("InstructionsDayOne");
            yield return new WaitForSeconds(5f);
        }

        if (inventory.day == 2)
        {
            StartCoroutine("InstructionsDayTwo");
            yield return new WaitForSeconds(3f);
        }

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
            if (inventory.day < 5)
            {
                CallSusCustomer();
                int id = inventory.day - 1;
                inventory.requestAnimal = new Inventory.Request(exoticAnimals[id], 100 + 50 * (id));
                yield return new WaitForSeconds(DAILYSUS_DURATION);
                EventManager.TriggerEvent("NextDayCycle");
            }
            else
            {
                yield return new WaitForSeconds(DAILYSUS_DURATION);
                EventManager.TriggerEvent("LoadEnd");
            }
        } else {
            CallRandomCustomer();
            yield return new WaitForSeconds(ENTER_DURATION);
            isServingCustomer = true;
        }
    }

    private IEnumerator InstructionsDayOne()
    {
        yield return new WaitForSeconds(1f);

        scrollInstruction.SetActive(true);
        yield return new WaitForSeconds(INSTRUCTION_DURATION);
        scrollInstruction.SetActive(false);

        selectInstruction.SetActive(true);
        yield return new WaitForSeconds(INSTRUCTION_DURATION);
        selectInstruction.SetActive(false);

        moneyInstruction.SetActive(true);
        suspicionInstruction.SetActive(true);
        yield return new WaitForSeconds(INSTRUCTION_DURATION);
        yield return new WaitForSeconds(INSTRUCTION_DURATION);
        moneyInstruction.SetActive(false);
        suspicionInstruction.SetActive(false);
    }

    private IEnumerator InstructionsDayTwo()
    {
        yield return new WaitForSeconds(1f);
        bluffInstruction.SetActive(true);
        yield return new WaitForSeconds(INSTRUCTION_DURATION);
        bluffInstruction.SetActive(false);
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

        if (id < 4)
        {
            inventory.currentAnimals.Remove(inventory.currentAnimals.Find(item => item.id == id));
        }

        StartCoroutine(GameLoop());
    }

    #region Suspicious Behaviour
    public void Tornado(int val) 
    { 
        tornadoAnim.SetActive(true);
        StartCoroutine("ActivateSpotlight");
    }

    private IEnumerator ActivateSpotlight()
    {
        yield return new WaitForSeconds(0.5f);
        spotlight.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        spotlight.SetActive(false);
    }

    public void DisguiseAnimal(int val)
    {
        Debug.Log("Change Anim");
        Animator a = animalDisplay.GetComponent<Animator>();
        // TODO: make sure each animal has D animation
        string currentName = a.GetCurrentAnimatorClipInfo(0)[0].clip.name + "D";
        a.Play(currentName);
    }
    #endregion
}