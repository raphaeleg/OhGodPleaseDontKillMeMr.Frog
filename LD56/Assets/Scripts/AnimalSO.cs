using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Animal : ScriptableObject
{
    public enum AnimalType { NORMAL, DISGUISE, EXOTIC };

    public string species;
    public AnimalType type;
    public int id;

    public string GetSpeciesName()
    {
        if (type == AnimalType.DISGUISE) { return species + "D"; }
        return species;
    }
}