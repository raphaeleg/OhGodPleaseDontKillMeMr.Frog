using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Disguise : MonoBehaviour
{
    private void ChangeAnimal()
    {
        EventManager.TriggerEvent("DisguiseAnimal");
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
