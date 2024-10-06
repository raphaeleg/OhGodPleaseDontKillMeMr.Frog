using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_Fishing : MonoBehaviour
{
    // Add all your minigame logic here
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animator animalSprite;
    [SerializeField] private Animator fishingString;

    [SerializeField] private Slider fishingGuage;
    [SerializeField] private Material barMat;

    private const int SUCCESS_HITS_NEEDED = 5;
    private const int FAIL_HITS_THRESHOLD = -3;
    private const float DIFFICULTY_MOD = 0.07f;
    private const float FREQ = 0.05f;
    private float difficultyFreq = 0.05f;
    private int currentHits = 0;
    private float fishRange = 0.5f;
    private string activeCoroutine = null;
    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new() {
            { "MinigameTimerDeplete", LostAttempt },
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
        animalSprite.Play(inventory.requestAnimal.animal.GetSpeciesName());
        Restart();
    }
    public void Restart(int val = 0)
    {
        currentHits = 0;
        fishRange = 0.5f;
        difficultyFreq = FREQ;
        ActivateBarCoroutine();
        barMat.SetFloat("_MinBound", 0.5f - (fishRange / 2.0f));
        barMat.SetFloat("_MaxBound", 0.5f + (fishRange / 2.0f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeCoroutine != null)
        {
            UpdateHits(fishingGuage.value);
            CheckEndCondition();
        }
    }

    private IEnumerator Slide()
    {
        bool bobDown = false;
        float value = 0;
        while ((currentHits > FAIL_HITS_THRESHOLD && currentHits < SUCCESS_HITS_NEEDED) || activeCoroutine != null)
        {
            yield return new WaitForSeconds(FREQ);
            if (bobDown)
            {
                value -= difficultyFreq;
                fishingGuage.value = Mathf.Max(0, value);
                if (fishingGuage.value == 0) { bobDown = false; }
            }
            else
            {
                value += difficultyFreq;
                fishingGuage.value = Mathf.Min(1, value);
                if (fishingGuage.value == 1) { bobDown = true; }
            }
        }
    }

    private bool IsWithinBoundary(float value)
    {
        float minimum = 0.5f - (fishRange / 2.0f);
        float maximum = 0.5f + (fishRange / 2.0f);
        return value > minimum && value < maximum;
    }

    private void UpdateHits(float value)
    {
        if (IsWithinBoundary(value))
        {
            currentHits++;
            fishRange -= DIFFICULTY_MOD;
            difficultyFreq += 0.005f;

            barMat.SetFloat("_MinBound", 0.5f - (fishRange / 2.0f));
            barMat.SetFloat("_MaxBound", 0.5f + (fishRange / 2.0f));

            fishingString.SetTrigger("Correct");
        }
        else { 
            currentHits--;
            fishingString.SetTrigger("Incorrect");
        }
    }

    private void CheckEndCondition()
    {
        if (currentHits >= SUCCESS_HITS_NEEDED)
        {
            Win();
        }
        else if (currentHits <= FAIL_HITS_THRESHOLD)
        {
            LostAttempt();
        }
    }

    private void LostAttempt(int val = 0)
    {
        DeactivateBarCoroutine();
        EventManager.TriggerEvent("LoseMinigameAttempt");
    }
    private void Win()
    {
        DeactivateBarCoroutine();
        EventManager.TriggerEvent("WinMinigame");
    }
    private void DeactivateBarCoroutine()
    {
        if (activeCoroutine == null) { return; }
        StopCoroutine(activeCoroutine);
        activeCoroutine = null;
    }
    private void ActivateBarCoroutine()
    {
        if (activeCoroutine != null)
        {
            DeactivateBarCoroutine();
        }
        activeCoroutine = "Slide";
        StartCoroutine(activeCoroutine);
    }
}
