using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Minigame_Lockpick : MonoBehaviour
{

    private KeyCode submitButton = KeyCode.Return;

    [SerializeField] private TMP_InputField guessInput;
    [SerializeField] private TMP_Text guessMarker;
    [SerializeField] private MinigameManager manager;

    private string[] words = { "BOWLS", "FETCH", "TREAT", "LEASH", "SNACK" };
    private string guessWord;

    private void Awake()
    {
        guessWord = words[Random.Range(0, words.Length)];   // Choosing random word from list
    }

    // Update is called once per frame
    void Update()
    {
        // Check answer when user presses enter
        if (Input.GetKeyDown(submitButton))
        {
            if (validateInput(guessInput.text))
            {
                // Submit if valid
                SubmitWord(guessInput.text);
            }
        }
    }

    private bool validateInput(string guess)
    {
        // Check if string is empty, or not enough letters
        if (string.IsNullOrEmpty(guess))
        {
            return false;
        }
        else if (guess.Length != guessWord.Length)
        {
            return false;
        }
        return true;
    }

    private void SubmitWord(string guess)
    {
        guessMarker.text = "";
        string answerCopy = "" + guessWord;

        guess = guess.ToUpper();    // Convert to upper case for no case errors when checking

        if (guess.Equals(guessWord))
        {
            // Trigger Win on correct guess
            Win();
        }
        else
        {
            for (int i = 0; i < guess.Length; i++)
            {
                // Check which letters are guessed correctly and remove them from copy string
                if (guess.ElementAt(i) == guessWord.ElementAt(i))
                {
                    // Find position of letter and remove it from the string to know which letters are guessed
                    int letterPosition = answerCopy.IndexOf(guess.ElementAt(i));
                    answerCopy = answerCopy.Substring(0, letterPosition) + answerCopy.Substring(letterPosition + 1);
                }
            }

            // Determine which letters are right place, right letter wrong place, and wrong letters
            for (int i = 0; i < guess.Length; i++)
            {
                // If right place and right letter
                if (guess.ElementAt(i) == guessWord.ElementAt(i))
                {
                    guessMarker.text += guess.ElementAt(i);
                }

                else
                {
                    // If not right place, check if it is one of the remaining letters but in wrong position
                    if (answerCopy.Contains(guess.ElementAt(i))) 
                    {
                        guessMarker.text += "?";

                        // Removing from copy to avoid repetitions
                        int letterPosition = answerCopy.IndexOf(guess.ElementAt(i));
                        answerCopy = answerCopy.Substring(0, letterPosition) + answerCopy.Substring(letterPosition + 1);
                    }

                    // If wrong letter
                    else
                    {
                        guessMarker.text += "x";
                    }
                }
            }
            LostAttempt();  // Trigger lose if guessed wrong
        }
    }

    private void LostAttempt()
    {
        StartCoroutine("ClearLetters");
        EventManager.TriggerEvent("LoseMinigameAttempt");

        // Rememebr to reload the attempt for players to continue playing the game! 
        // MinigameManager will terminate the game once all attempts are depleted
    }

    private IEnumerator ClearLetters()
    {
        yield return new WaitForSeconds(2f);
        guessInput.text = string.Empty;
    }
    private void Win()
    {
        EventManager.TriggerEvent("WinMinigame");
    }
}
