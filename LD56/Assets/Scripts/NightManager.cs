using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NightManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animator exoticAnimalAnim;
    [SerializeField] private TMP_Text exoticAnimalText;

    public void OnEnable()
    {
        exoticAnimalAnim.Play(inventory.requestAnimal.animal.GetSpeciesName());
        exoticAnimalText.text = "Reward: $" + inventory.requestAnimal.money;
    }
}
