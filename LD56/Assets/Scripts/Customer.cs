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
    private bool isSus = false;

    private Dictionary<int, int> disguiseMappings;

    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        disguiseMappings = new()
        {
            {0, 11},    // cobra, snake
            {1, 4 },    // eagle, bird
            {2, 8},     // shark, fish
            {3, 5},     // tiger, cat
        };

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
        if (isSus) { isSus = false; }
        string randChar = CharacterAnimNames[UnityEngine.Random.Range(0, CharacterAnimNames.Count)];
        characterSprite.GetComponent<Animator>().Play(randChar);
        animalID = UnityEngine.Random.Range(inventory.EXOTIC_COUNT, inventory.NORMAL_COUNT + inventory.EXOTIC_COUNT);
        StartCoroutine(CharacterAnimation(-1));
    }

    private void GenerateSpecial(int val)
    {
        isSus = true;
        animalID = inventory.day-1;
        if (val == 0) { animalID--; }
        characterSprite.GetComponent<Animator>().Play(SuspiciousAnimName);
        StartCoroutine(CharacterAnimation(val));
    }

    private IEnumerator CharacterAnimation(int opt)
    {
        CharacterEnter();
        yield return new WaitForSeconds(ENTER_DURATION);
        dialogueBox.SetActive(true);
        if (opt == 1)
        {
            PlayDialogueAnim("Envelope"); 
        }
        else
        {
            PlayDialogueAnim(inventory.GetName(animalID));
        }
    }
    private void CharacterEnter()
    {
        transform.DOMoveX(1920 / 2, ENTER_DURATION);
        //Lily.DORotate(new Vector3(0, 0, 7), cyclelength * 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
    private void React(int id)
    {
        bool isCorrect = id == animalID;
        if (isSus && id > 3) {
            // Bluffed
            EventManager.TriggerEvent("ChoseBluff");
            // Visuals
            isCorrect = id == disguiseMappings[animalID];
        }
        StartCoroutine(React(isCorrect, id));
    }
    private IEnumerator React(bool isCorrect, int id)
    {
        float duration = ENTER_DURATION;
        if (isSus)
        {
            //duration /= 2;
            PlayDialogueAnim("Emote_Hesitate");
            yield return new WaitForSeconds(duration);
        }

        if (isCorrect)
        {
            if (id < 4)
            {
                EventManager.TriggerEvent("AddMoney", 100 + 50 * (id));
            }
            else
            {
                EventManager.TriggerEvent("AddMoney", CORRECT_MONEY);
            }
            PlayDialogueAnim("Emote_Correct");
        } else
        {
            EventManager.TriggerEvent("AddSuspicion", INCORRECT_SUS);
            PlayDialogueAnim("Emote_Incorrect");
        }
        yield return new WaitForSeconds(duration);

        dialogueBox.SetActive(false);


        transform.DOMoveX(-1920/2, ENTER_DURATION);
        //Lily.DORotate(new Vector3(0, 0, 7), cyclelength * 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void PlayDialogueAnim(string val)
    {
        dialogueBox.transform.GetChild(0).GetComponent<Animator>().Play(val);
    }
}
