using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Template : MonoBehaviour
{
    // Add all your minigame logic here




    private void LostAttempt()
    {
        EventManager.TriggerEvent("LoseMinigameAttempt");
        // Rememebr to reload the attempt for players to continue playing the game! 
        // MinigameManager will terminate the game once all attempts are depleted
    }
    private void Win()
    {
        EventManager.TriggerEvent("WinMinigame");
    }
}
