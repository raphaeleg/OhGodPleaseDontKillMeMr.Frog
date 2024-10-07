using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A signleton class that passes data to AudioManager based on a triggered Event.
/// </summary>
public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance { get; private set; }

    #region EventReference Variables
    [field: Header("Music")]
    [field: SerializeField] public EventReference BG { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField] public EventReference DisguiseAnimal { get; private set; }
    [field: SerializeField] public EventReference MoralityUp { get; private set; }
    [field: SerializeField] public EventReference MoralityDown { get; private set; }
    [field: SerializeField] public EventReference CloseShop { get; private set; }
    [field: SerializeField] public EventReference OpenShop { get; private set; }
    [field: SerializeField] public EventReference PageFlip { get; private set; }
    [field: SerializeField] public EventReference PageCrumble { get; private set; }
    [field: SerializeField] public EventReference MinigameCorrect { get; private set; }
    [field: SerializeField] public EventReference MinigameIncorrect { get; private set; }
    [field: SerializeField] public EventReference Kettle { get; private set; }
    [field: SerializeField] public EventReference MinigameLockBeep { get; private set; }
    [field: SerializeField] public EventReference Money { get; private set; }
    [field: SerializeField] public EventReference MinigameFail { get; private set; }
    [field: SerializeField] public EventReference Foliage { get; private set; }
    [field: SerializeField] public EventReference CustomerCat { get; private set; }
    [field: SerializeField] public EventReference CustomerMouse { get; private set; }
    [field: SerializeField] public EventReference CustomerGoat { get; private set; }
    [field: SerializeField] public EventReference CustomerSus { get; private set; }
    [field: SerializeField] public EventReference FrogMafia { get; private set; }

    [field: Header("Disguised Animals")]
    [field: SerializeField] public EventReference DSnake { get; private set; }
    [field: SerializeField] public EventReference DBird { get; private set; }
    [field: SerializeField] public EventReference DCat { get; private set; }
    [field: SerializeField] public EventReference DFish { get; private set; }

    [field: Header("Animals")]
    [field: SerializeField] public EventReference Cobra { get; private set; }
    [field: SerializeField] public EventReference Eagle { get; private set; }
    [field: SerializeField] public EventReference Shark { get; private set; }
    [field: SerializeField] public EventReference Tiger { get; private set; }
    [field: SerializeField] public EventReference Bird { get; private set; }
    [field: SerializeField] public EventReference Cat { get; private set; }
    [field: SerializeField] public EventReference Dog { get; private set; }
    [field: SerializeField] public EventReference Ferret { get; private set; }
    [field: SerializeField] public EventReference Fish { get; private set; }
    [field: SerializeField] public EventReference Mouse { get; private set; }
    [field: SerializeField] public EventReference Rabbit { get; private set; }
    [field: SerializeField] public EventReference Snake { get; private set; }
    [field: SerializeField] public EventReference Turtle { get; private set; }
    [field: SerializeField] public EventReference Hippo { get; private set; }
    #endregion

    #region Event Listeners
    private Dictionary<string, Action<int>> SubscribedEvents = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SubscribedEvents = new() {
                { "DisguiseAnimal", SFX_DisguiseAnimal },
                { "LoadNight", SFX_Night },
                { "NextDay", SFX_Day },
                { "ButtonClick", SFX_ButtonClick },
                { "MinigameSFX", SFX_Minigame_Response },
                { "MinigameTimerDeplete", SFX_Minigame_TimerUp },
                { "MinigameLockInput", SFX_Minigame_LockBeep },
                { "Day_SelectAnimal", SFX_Animal },
                { "AddMoney", SFX_Money },
                { "AddSuspicion", SFX_Sus },
                { "LoseMinigame", SFX_Minigame_Fail },
                { "ChangeMusic", ChangeMusic },
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
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StopListening(pair.Key, pair.Value);
        }
    }
    #endregion

    private void Play(EventReference eventRef)
    {
        AudioManager.Instance.PlayOneShot(eventRef);
    }
    private void SFX_DisguiseAnimal(int val = 0)
    {
        Play(DisguiseAnimal);
    }

    private void SFX_Day(int val = 0)
    {
        SFX_DayCycle(0);
    }
    private void SFX_Night(int val = 0)
    {
        SFX_DayCycle(1);
    }
    private void SFX_DayCycle(int val = 0)
    {
        // 0 = day, 1 = night
        Play(val == 0 ? OpenShop : CloseShop);
    }
    private void SFX_Sus(int val = 0)
    {
        Play(MoralityDown);
    }
    private void SFX_Minigame_Fail(int val = 0)
    {
        Play(MinigameFail);
    }
    private void SFX_ButtonClick(int val = 0)
    {
        //0 = default, other = negative / special (quit, pause, scene transitions, typewriter)
        Play(val == 0 ? PageFlip : PageCrumble);
    }
    private void SFX_Minigame_Response(int val = 0)
    {
        // 0 = fail, 1 = correct
        Play(val == 0 ? MinigameIncorrect : MinigameCorrect);
    }
    private void SFX_Minigame_TimerUp(int val = 0)
    {
        Play(Kettle);
    }
    private void SFX_Money(int val = 0)
    {
        Play(Money);
    }
    private void SFX_Minigame_LockBeep(int val = 0)
    {
        Play(MinigameLockBeep);
    }
    private void SFX_DAnimal(int id = 0)
    {
        switch(id)
        {
            case 4:
                Play(DBird);
                break;
            case 5:
                Play(DCat);
                break;
            case 8:
                Play(DFish);
                break;
            case 11:
                Play(DSnake);
                break;
            default:
                break;
        }
    }
    private void SFX_Animal(int id = 0)
    {
        switch(id)
        {
            case 0:
                Play(Cobra);
                break;
            case 1:
                Play(Eagle);
                break;
            case 2:
                Play(Shark);
                break;
            case 3:
                Play(Tiger);
                break;
            case 4:
                Play(Bird);
                break;
            case 5:
                Play(Cat);
                break;
            case 6:
                Play(Dog);
                break;
            case 7:
                Play(Ferret);
                break;
            case 8:
                Play(Fish);
                break;
            case 9:
                Play(Mouse);
                break;
            case 10:
                Play(Rabbit);
                break;
            case 11:
                Play(Snake);
                break;
            case 12:
                Play(Turtle);
                break;
            case 13:
                Play(Hippo);
                break;
            default:
                break;
        }
    }
    public void ChangeMusic(int music)
    {
        AudioManager.Instance.SetMusicArea((Audio_MusicArea)music);
    }
}