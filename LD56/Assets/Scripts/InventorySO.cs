using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public List<Animal> currentAnimals;
    public List<Animal> requestAnimals;

    public void Reset()
    {
        currentAnimals.Clear();
        requestAnimals.Clear();
    }
}