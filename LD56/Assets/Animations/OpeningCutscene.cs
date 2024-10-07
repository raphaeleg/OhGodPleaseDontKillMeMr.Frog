using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpeningCutscene : MonoBehaviour
{
    public GameObject part1;
    public GameObject part2;
    public CanvasGroup blackScreen;

    public GameObject frog;
    public GameObject frogDialogue;
    public GameObject frogMoney;
    public GameObject frogText;
    public GameObject frogSkull;

    public GameObject fly;
    public GameObject flyDialogue;

    public GameObject fly2;


    public void Start()
    {
        StartCoroutine(Animation());
    }
    float duration = 2f;
    public IEnumerator Animation()
    {
        blackScreen.DOFade(0, duration);
        part1.SetActive(true);

        var a = frog.transform.localPosition;
        a.y = 270;
        frog.transform.localPosition = a;

        frog.transform.DOMoveX(700, duration);
        yield return new WaitForSeconds(duration);

        frogDialogue.SetActive(true);
        frogMoney.SetActive(true);
        yield return new WaitForSeconds(duration);

        frogDialogue.SetActive(false);
        flyDialogue.SetActive(true);
        yield return new WaitForSeconds(duration);

        flyDialogue.SetActive(false);
        frogDialogue.SetActive(true);
        frogMoney.SetActive(false);
        frogText.SetActive(true);
        yield return new WaitForSeconds(duration);

        frogText.SetActive(false);
        frogSkull.SetActive(true);
        yield return new WaitForSeconds(duration);

        frogDialogue.SetActive(false);
        frog.transform.DOMoveX(-700, duration);
        yield return new WaitForSeconds(duration*2);

        part1.SetActive(false);
        part2.SetActive(true);
        fly2.transform.DOShakePosition(2.5f, 100);
        part2.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 2.5f);
        yield return new WaitForSeconds(2);

        EventManager.TriggerEvent("LoadMainMenu");
    }
}
