using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NightManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image exoticAnimalImg;
    [SerializeField] private TMP_Text exoticAnimalText;

    public void OnEnable()
    {
        exoticAnimalImg.sprite = inventory.requestAnimal.animal.sprite;
        exoticAnimalText.text = "Reward: $" + inventory.requestAnimal.money;
    }
}
