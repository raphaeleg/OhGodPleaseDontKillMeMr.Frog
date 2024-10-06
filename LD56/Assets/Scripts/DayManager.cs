using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public void Start()
    {
        EventManager.TriggerEvent("RandomCustomer", 0);
    }


}