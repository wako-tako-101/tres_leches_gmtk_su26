using System.Collections.Generic;
using UnityEngine;

public class SaveCalculatorAnswers : MonoBehaviour
{
    public static SaveCalculatorAnswers Instance;

    public List<int> savedAnswers = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveAnswer(int answer)
    {
        if (answer >= 10)
        {
            string answerString = answer.ToString();

            foreach (char digit in answerString)
            {
                savedAnswers.Add(int.Parse(digit.ToString()));
            }
        }
        else
        {
            savedAnswers.Add(answer);
        }

        Debug.Log("Saved Answer: " + answer);
    }

    public void ClearAnswers()
    {
        savedAnswers.Clear();
    }
}