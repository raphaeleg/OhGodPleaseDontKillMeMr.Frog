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
    public Request requestAnimal;

    public void Reset()
    {
        currentAnimals.Clear();
        requestAnimal.Clear();
    }
}