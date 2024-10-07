using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Night_Envelope : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject dayShift;
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine("EnvelopeAnimation");
    }

    private IEnumerator EnvelopeAnimation()
    {
        float waiting = 2.75f;
        
        //yield return new WaitForSeconds(0.5f);

        dayShift.SetActive(true);
        dayShift.GetComponent<Animator>().Play("DayToNight");

        yield return new WaitForSeconds(waiting);

        //dayShift.GetComponent<Animator>().StopPlayback();
        dayShift.GetComponent<CanvasGroup>().DOFade(0, 1);
        //yield return new WaitForSeconds(2f);
        //dayShift.SetActive(false);




        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Envelope");
        gameObject.transform.GetChild(0).transform.localScale = Vector3.zero;
        gameObject.transform.GetChild(0).transform.DOScale(Vector3.one, 2).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(2f);

        gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 1);

        yield return new WaitForSeconds(1f);

        transform.GetComponent<CanvasGroup>().DOFade(0, 1);
    }
}
