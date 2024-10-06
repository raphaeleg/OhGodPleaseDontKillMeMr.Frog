using System.Collections.Generic;
using System;
using DG.Tweening;
using UnityEngine;

public class CharacterMoving : MonoBehaviour
{
    [SerializeField] private Transform Lily;
    [SerializeField] private float cyclelength = 2;

    #region EventManager
    #region Customer
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void CharacterAnimation()
    {
        transform.DOMoveX(1300, 5);
        Lily.DORotate(new Vector3(0, 0, 7), cyclelength * 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void Awake()
    {
        SubscribedEvents = new() {
        { "RandomCustomer", Generate },
        { "SpecialCustomer", GenerateSpecial },
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

    private void Generate(int val)
    {
        CharacterAnimation();
    }

    private void GenerateSpecial(int val)
    {
        CharacterAnimation();
    }

    #endregion
    #endregion

    //transform.DOMove(new Vector3(1250, 560, 0), cyclelength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);//

    //transform.DOMoveX(1300, 5);
    //Lily.DORotate(new Vector3(0, 0, 7), cyclelength * 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

    //Asking for an animal [Event Dialogue / popup]

    //Wait until the the player does an action that provides them with the animal

    //Give Money based on if they liked it / Adjust Suspicion meter

    //Leave

    //Repeat 2 more times

    //After this go to Night Time
}
