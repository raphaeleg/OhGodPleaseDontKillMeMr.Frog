using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {
    [Serializable]
    public struct Request
    {
        public Animal animal;
        public int money;

        public Request(Animal a, int m) {
            animal = a;
            money = m;
        }

        public void Clear() {
            animal = null;
            money = 0;
        }
    }

    public List<Animal> allAnimals;
    public List<Animal> baseAnimals;
    public List<Animal> currentAnimals;
    public int EXOTIC_COUNT = 4;
    public int NORMAL_COUNT = 10;
    public Request requestAnimal;
    public int day = 0;

    public void Reset()
    {
        currentAnimals = new List<Animal>(baseAnimals);
        requestAnimal.Clear();
        day = 0;
    }
    public void GainExoticAnimal()
    {
        currentAnimals.Add(requestAnimal.animal);
        requestAnimal.Clear();
    }
    public string GetName(int id)
    {
        Animal a = allAnimals.Find(item => item.id == id);
        return a.GetSpeciesName();
    }
}