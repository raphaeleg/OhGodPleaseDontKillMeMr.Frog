using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public struct Request
    {
        public Animal requestedAnimal;
        public int dayDuration;

        public Request(Animal a, int d)
        {
            requestedAnimal = a;
            dayDuration = d;
        }
        public void DecreaseDuration() { this.dayDuration--; }
    }

    public List<Animal> currentAnimals;
    public List<Request> requestAnimals;

    public void AddRequest(Animal a, int d) { requestAnimals.Add(new Request(a, d));}

    public void DecreaseRequestDuration()
    {
        // NEEDS TO BE TESTED
        foreach (Request r in requestAnimals)
        {
            r.DecreaseDuration();
            if (r.dayDuration > 0) { return; }
            
            // CASE TODO: When request expires
            requestAnimals.Remove(r);
        }
    }

    public void Reset()
    {
        currentAnimals.Clear();
        requestAnimals.Clear();
    }
}