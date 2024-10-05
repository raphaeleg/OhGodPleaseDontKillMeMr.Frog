using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum DayCycle { DAY, NIGHT };

    private int day = 0;
    private DayCycle cycle = DayCycle.DAY;

    // Singleton
    private static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        day = 0;
        cycle = DayCycle.DAY;
    }

    private void AddDay() { day++; }
    private void ToggleCycle() {
        if (cycle == DayCycle.DAY) { cycle = DayCycle.NIGHT; }
        else { cycle = DayCycle.DAY; }
    }
}
