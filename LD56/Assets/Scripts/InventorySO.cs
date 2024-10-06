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

    public List<Animal> currentAnimals;
    public int NORMAL_COUNT = 10;
    public Request requestAnimal;

    public void Reset()
    {
        currentAnimals.Clear();
        requestAnimal.Clear();
    }
    public void GainExoticAnimal()
    {
        currentAnimals.Add(requestAnimal.animal);
        requestAnimal.Clear();
    }
}