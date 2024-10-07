using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private TMP_Text gameEndText;
    [SerializeField] private GameObject background;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance.getSuspicion() >= 100)
        {
            gameEndText.text = "Oh No! You were too Suspicious!! The Police captured you! (Ending 1/3)";
        }
        else if (GameManager.Instance.getMoney() < 400)
        {
            gameEndText.text = "Oh No! You didn't collect enough Money!! The Mafia was unhappy and killed you! (Ending 2/3)";
        }
        else
        {
            gameEndText.text = "Horray!! You get to live another day!! (Ending 3/3)";
        }
    }
}
