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

    private const int MAX_STEAL_ATTEMPTS = 3;
    private const int STEAL_SUCCESS_SUSPICION = 10;
    private const int STEAL_FAIL_SUSPICION = 25;
    private int stealAttempts = 0;

    [SerializeField] private Button stealBtn;
    private TMP_Text stealText;

    public void OnEnable()
    {
        exoticAnimalImg.sprite = inventory.requestAnimal.animal.sprite;
        exoticAnimalText.text = "Reward: $" + inventory.requestAnimal.money;
        stealAttempts = MAX_STEAL_ATTEMPTS;
        stealText = stealBtn.transform.GetChild(0).GetComponent<TMP_Text>();
        stealText.text = "Steal (" + MAX_STEAL_ATTEMPTS + " attempts left)";
    }

    public void StealFailed()
    {
        if (stealAttempts < 0) { return; }
        
        stealAttempts--;
        if (stealAttempts <= 0)
        {
            EventManager.TriggerEvent("AddSuspicion", STEAL_FAIL_SUSPICION);
            // TODO: exit out of night
            return;
        }

        stealText.text = "Steal ("+ stealAttempts + " attempts left)";
    }

}
