using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalRequest : MonoBehaviour
{
    private Image img;
    private TextMeshPro tmp;

    public void SetAnimalRequest(Sprite s, string t)
    {
        img.sprite = s;
        tmp.text = t;
    }
}
