using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Night_RequestCanvas : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image exoticAnimalImg;
    [SerializeField] private TMP_Text exoticAnimalText;

    private const int STEAL_ATTEMPTS = 3;
    private int stealAttempts = 0;

    [SerializeField] private Button stealBtn;
    private TMP_Text stealText;

    public void OnEnable()
    {
        exoticAnimalImg.sprite = inventory.requestAnimal.animal.sprite;
        exoticAnimalText.text = "Reward: $" + inventory.requestAnimal.money;
        stealAttempts = 0;
        stealText = stealBtn.transform.GetChild(0).GetComponent<TMP_Text>();
        stealText.text = "Steal (" + STEAL_ATTEMPTS + " attempts left)";
    }

    public void StealFailed()
    {
        stealAttempts++;
        if (stealAttempts >= STEAL_ATTEMPTS)
        {
            // TODO: show suspicion up
            return;
        }

        stealText.text = "Steal ("+ stealAttempts + " attempts left)";
    }

}
