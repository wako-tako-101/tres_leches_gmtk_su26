using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EquationCombination
{
    public string[] order;
}

public class EquationChecker : MonoBehaviour
{
    public NumberSlot[] slots;

    public EquationCombination[] correctOrders;

    public bool isFinalChallenge;
    public bool isDivisionChallenge;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    public EquationSequenceManager sequenceManager;

    [Header("Slot State Events")]
    public UnityEvent onEquationFilled;
    public UnityEvent onEquationNotFilled;

    public bool EquationReady { get; private set; }

    private bool lastEquationState;

    private bool answerSaved = false;

    private void Start()
    {
        EquationReady = AreAllSlotsFilled();

        lastEquationState = EquationReady;

        if (EquationReady)
        {
            onEquationFilled?.Invoke();
        }
        else
        {
            onEquationNotFilled?.Invoke();
        }
    }

    private void Update()
    {
        CheckIfEquationReady();
    }

    private void CheckIfEquationReady()
    {
        bool currentEquationState = AreAllSlotsFilled();

        EquationReady = currentEquationState;

        if (currentEquationState != lastEquationState)
        {
            lastEquationState = currentEquationState;

            if (currentEquationState)
            {
                onEquationFilled?.Invoke();
            }
            else
            {
                onEquationNotFilled?.Invoke();
            }
        }
    }

    private bool AreAllSlotsFilled()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentBox == null)
            {
                return false;
            }
        }

        return true;
    }

    public void CheckEquation()
    {
        if (!EquationReady)
        {
            return;
        }

        if (isFinalChallenge)
        {
            CheckFinalChallenge();
        }
        else
        {
            CheckNormalEquation();
        }
    }

    private void CheckNormalEquation()
    {
        foreach (EquationCombination combination in correctOrders)
        {
            if (combination.order.Length != slots.Length)
            {
                continue;
            }

            bool isCorrect = true;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].currentBox == null)
                {
                    isCorrect = false;
                    break;
                }

                if (!slots[i].currentBox.CompareTag(combination.order[i]))
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                if (!answerSaved)
                {
                    int answer;

                    if (!isDivisionChallenge)
                    {
                        answer = CalculateAnswer(combination.order);
                    }
                    else
                    {
                        answer = 5;
                    }

                    if (SaveCalculatorAnswers.Instance != null)
                    {
                        SaveCalculatorAnswers.Instance.SaveAnswer(answer);
                    }

                    answerSaved = true;
                }

                PlayCorrectSound();
                return;
            }
        }

        PlayIncorrectSound();
    }

    private void CheckFinalChallenge()
    {
        if (SaveCalculatorAnswers.Instance == null)
        {
            Debug.LogError("SaveCalculatorAnswers instance not found!");
            return;
        }

        if (slots.Length != SaveCalculatorAnswers.Instance.savedAnswers.Count)
        {
            PlayIncorrectSound();
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentBox == null)
            {
                PlayIncorrectSound();
                return;
            }

            int placedNumber = GetNumberFromTag(
                slots[i].currentBox.tag
            );

            if (placedNumber != SaveCalculatorAnswers.Instance.savedAnswers[i])
            {
                PlayIncorrectSound();
                return;
            }
        }

        PlayCorrectSound();
        Debug.Log("YOU BEAT THIS MINIGAME YAY!");
    }

    private int CalculateAnswer(string[] equation)
    {
        int number1 = GetNumberFromTag(equation[0]);
        string operation = equation[1];
        int number2 = GetNumberFromTag(equation[2]);

        switch (operation)
        {
            case "Plus":
                return number1 + number2;

            case "Minus":
                return number1 - number2;

            case "Multiply":
                return number1 * number2;

            case "Divide":
                if (number2 != 0)
                {
                    return number1 / number2;
                }
                break;
        }

        return 0;
    }

    private int GetNumberFromTag(string tag)
    {
        switch (tag)
        {
            case "Number0": return 0;
            case "Number1": return 1;
            case "Number2": return 2;
            case "Number3": return 3;
            case "Number4": return 4;
            case "Number5": return 5;
            case "Number6": return 6;
            case "Number7": return 7;
            case "Number8": return 8;
            case "Number9": return 9;
        }

        return 0;
    }

    private void PlayCorrectSound()
    {
        Debug.Log("CORRECT EQUATION!");

        if (audioSource != null && correctSound != null)
        {
            audioSource.PlayOneShot(correctSound);
        }

        if (sequenceManager != null)
        {
            sequenceManager.EquationCompleted();
        }
    }

    private void PlayIncorrectSound()
    {
        Debug.Log("INCORRECT EQUATION!");

        if (audioSource != null && incorrectSound != null)
        {
            audioSource.PlayOneShot(incorrectSound);
        }
    }
}