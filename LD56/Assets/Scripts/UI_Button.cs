using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class UI_Button : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool isDark = false;

    public void Start()
    {
        if (isDark)
        {
            gameObject.GetComponent<Animator>().Play("Button_Dark");
            transform.GetChild(0).GetComponent<TMP_Text>().color = new(0,0,0,0);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOScale(10f, 0.5f);
        transform.DOScale(1f, 0.5f);
    }
}
