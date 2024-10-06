using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private TMP_Text gameEndText;
    //private static GameManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance.getSuspicion() >= 100)
        {
            gameEndText.text = "Police captured you!";
        }
        else if (GameManager.Instance.getMoney() < 400)
        {
            gameEndText.text = "Mafia killed you!";
        }
        else
        {
            gameEndText.text = "You get to live another day!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
