using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Day_AnimalOption : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Animal animal;
    private Vector3 startMouse = Vector3.zero;
    private bool calcDrag = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!calcDrag)
        {
            calcDrag = true;
            startMouse = Input.mousePosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (calcDrag) {
            Vector3 r = Input.mousePosition - startMouse;
            if (Mathf.Abs(r.magnitude) < 0.01f)
            {
                EventManager.TriggerEvent("Day_SelectAnimal", animal.id);
            }
            startMouse = Vector3.zero;
            calcDrag = false;
        }
    }

}
