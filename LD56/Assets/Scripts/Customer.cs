using DG.Tweening;
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

    private const float ENTER_DURATION = 1.0f;
    [SerializeField] private GameObject characterSprite;
    [SerializeField] private float cyclelength = 2;
    [SerializeField] private GameObject dialogueBox;

    [SerializeField] private List<string> CharacterAnimNames = new();
    private const string SuspiciousAnimName = "Suspicious";

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

    private void Generate(int val = 0)
    {
        string randChar = CharacterAnimNames[UnityEngine.Random.Range(0, CharacterAnimNames.Count)];
        characterSprite.GetComponent<Animator>().Play(randChar);
        animalID = UnityEngine.Random.Range(inventory.EXOTIC_COUNT, inventory.NORMAL_COUNT + inventory.EXOTIC_COUNT);
        StartCoroutine(CharacterAnimation());
    }

    private void GenerateSpecial(int val)
    {
        animalID = inventory.day-1;
        characterSprite.GetComponent<Animator>().Play(SuspiciousAnimName);
        StartCoroutine(CharacterAnimation());
    }

    private IEnumerator CharacterAnimation()
    {
        CharacterEnter();
        yield return new WaitForSeconds(ENTER_DURATION);
        dialogueBox.SetActive(true);
        if (animalID > 3)
        {
            dialogueBox.transform.GetChild(0).GetComponent<Animator>().Play(inventory.GetName(animalID));
        }
        else
        {
            dialogueBox.transform.GetChild(0).GetComponent<Animator>().Play("Envelope");
        }
    }
    private void CharacterEnter()
    {
        transform.DOMoveX(1920 / 2, ENTER_DURATION);
        //Lily.DORotate(new Vector3(0, 0, 7), cyclelength * 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void React(int id)
    {
        // TODO: Visually show results

        if (id == animalID)
        {
            EventManager.TriggerEvent("AddMoney", CORRECT_MONEY);
        } else
        {
            EventManager.TriggerEvent("AddSuspicion", INCORRECT_SUS);
        }

        dialogueBox.SetActive(false);

        transform.DOMoveX(-1920/2, ENTER_DURATION);
        //Lily.DORotate(new Vector3(0, 0, 7), cyclelength * 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
