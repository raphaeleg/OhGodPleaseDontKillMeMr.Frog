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
    [SerializeField] private SceneLoader loader;

    private string[] minigames = { "StartMinigameLockpick", "StartMinigameFishing", "StartMinigameDino" };

    public void OnEnable()
    {
        exoticAnimalAnim.Play(inventory.requestAnimal.animal.GetSpeciesName());
        exoticAnimalText.text = "$" + inventory.requestAnimal.money;
    }

    private void Start()
    {
        EventManager.TriggerEvent("ChangeMusic", (int)Audio_MusicArea.NIGHT);
    }

    public void StealAction()
    {
        int day = inventory.day;
        Debug.Log(day);
        if (day == 1)
        {
            EventManager.TriggerEvent(minigames[0]);
        }
        else if (day == 2)
        {
            EventManager.TriggerEvent(minigames[1]);
        }
        else if (day == 3)
        {
            EventManager.TriggerEvent(minigames[2]);
        }
        else
        {
            EventManager.TriggerEvent(minigames[Random.Range(0, minigames.Length-1)]);
        }
    }
}
