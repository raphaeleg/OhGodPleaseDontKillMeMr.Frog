using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Minigame_ResultAnimation : MonoBehaviour
{
    private enum Result { WIN, LOSE };
    [SerializeField] private Result result;
    [SerializeField] private Inventory inventory;
    private void OnEnable()
    {
        if (result == Result.WIN)
        {
            gameObject.GetComponent<Animator>().Play(inventory.currentAnimals[inventory.currentAnimals.Count-1].GetSpeciesName());
            gameObject.transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 1).SetEase(Ease.InOutSine);
        }
        else
        {
            gameObject.GetComponent<Animator>().Play(inventory.requestAnimal.animal.GetSpeciesName());
            gameObject.transform.localScale = Vector3.one;
            transform.DOScale(Vector3.zero, 1).SetEase(Ease.InOutSine);
        }
    }

    private void OnDisable()
    {
        gameObject.transform.localScale = Vector3.one;
    }
}
