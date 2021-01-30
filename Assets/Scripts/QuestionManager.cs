using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public GameObject diceRollManager;
    public GameObject questionPanel;
    public GameObject submitBtn;
    public GameObject answerInpt;
    public Text questionText;
    public Text answerText;

    int answer;
    int userAnswer;
    string question;

    float maxTime = 3.0f;
    float startTime = 0;

    DifficultyLevel difficulty = DifficultyLevel.Intermediate;

    enum DifficultyLevel {
        Basic,
        Intermediate,
        Advanced
    };

    public string getDifficulty()
    {
        return difficulty.ToString().ToLower();
    }

    public void setDifficulty(string dif)
    {
        switch (dif)
        {
            case "basic":
                difficulty = DifficultyLevel.Basic;
                break;

            case "intermediate":
                difficulty = DifficultyLevel.Intermediate;
                break;

            case "advanced":
                difficulty = DifficultyLevel.Advanced;
                break;
        }
    }

    public void activateSelf()
    {
        generateQuestion(diceRollManager.GetComponent<DiceRollManager>().getType(), diceRollManager.GetComponent<DiceRollManager>().getAmount());
        questionPanel.SetActive(true);
        answerInpt.SetActive(true);
     //   answerInpt.GetComponent<Text>().text = "";
        submitBtn.SetActive(true);
    }

    void generateQuestion(int dieType, int amount)
    {
        answer = -1;
        question = "";
           
        if (difficulty == DifficultyLevel.Basic)
        {
            generateBasicQuestion(dieType);
        }
        else if (difficulty == DifficultyLevel.Intermediate)
        {
            generateIntermediateQuestion(dieType);
        }
        else if (difficulty == DifficultyLevel.Advanced)
        {
            generateAdvancedQuestion(dieType, amount);
        }

        questionText.text = question;
        answerText.text = "";
    }

    void generateAdvancedQuestion(int dieType, int amount)
    {
        answer = -1;
        question = "";

        question += "On " + amount + " d" + dieType + (amount > 1 ? "s" : "") + " is it more probable that the sum ";
        int rr = Random.Range(0, 3);
        int number = Random.Range(amount, dieType * amount);
        question += "will be equal to " + number + " rather than ";

        int number2 = Random.Range(amount, dieType * amount);
        while (number == number2)
        {
            number2 = Random.Range(amount, dieType * amount);
        }

        question += number2 + "? (type yes/y or no/n)";

        int probability1 = calculateProbability(number, amount, dieType);
        int probability2 = calculateProbability(number2, amount, dieType);

        

        answer = (probability1 > probability2 ? 1 : 0);
        

        questionText.text = question;
        answerText.text = "";
    }

    int calculateProbability(int number, int dieAmount, int dieType)
    {
        //source: https://www.geeksforgeeks.org/dice-throw-dp-30/
        int[,] mem = new int[dieAmount + 1, number + 1];

        mem[0, 0] = 1;
        for (int i = 1; i <= dieAmount; i++)
        {
            for (int j = i; j <= number; j++)
            {
                mem[i, j] = mem[i, j - 1] + mem[i - 1, j - 1];
                if (j - dieType - 1 >= 0)
                    mem[i, j] -= mem[i - 1, j - dieType - 1];
            }
        }
        return mem[dieAmount, number];
    }

    void generateIntermediateQuestion(int dieType)
    {
        answer = -1;
        question = "";

        question += "On d" + dieType + " what is the probability that the number will be ";
        int rr = Random.Range(0, 8);
        int number = Random.Range(1, dieType + 1);
        if (rr == 0)
        {
            question += "greater than " + number + "?";
            answer = Mathf.RoundToInt((dieType - number) / (float)dieType * 100);
        }
        else if (rr == 1)
        {
            question += "less than " + number + "?";
            answer = Mathf.RoundToInt((number - 1) / (float)dieType * 100);
        }
        else if (rr == 2)
        {
            question += "at least " + number + "?";
            answer = Mathf.RoundToInt((dieType - (number - 1)) / (float)dieType * 100);
        }
        else if (rr == 3)
        {
            question += "at most " + number + "?";
            answer = Mathf.RoundToInt(number / (float)dieType * 100);
        }
        else
        {
            question += "equal to " + number + "?";
            answer = Mathf.RoundToInt(1 / (float)dieType * 100);
        }

        questionText.text = question;
        answerText.text = "";
    }

    void generateBasicQuestion(int dieType)
    {
        answer = -1;
        question = "";

        question += "On d" + dieType + " is it more probable that ";
        int rr = Random.Range(0, 3);
        int number = Random.Range(1, dieType + 1);
        if (rr == 0)
        {
            int rr2 = Random.Range(0, 2);
            if (rr2 == 0)
            {
                question += "the number " + number + " will fall rather than ";
                question += "an " + (number % 2 == 0 ? "odd " : "even ") + "number? (type yes/y or no/n)";
                answer = 0;
            }
            else
            {
                question += "an " + (number % 2 == 0 ? "odd " : "even ") + " number will fall rather than ";
                question +=  "number " + number+ "? (type yes/y or no/n)";
                answer = 1;
            }            
        }
        else
        {
            question += "the number " + number + " will fall rather than number ";
            int number2 = Random.Range(1, dieType + 1);
            while (number == number2)
            {
                number2 = Random.Range(1, dieType + 1);
            }
            question += number2 + "? (type yes/y or no/n)";
            answer = 0;
        }

        questionText.text = question;
        answerText.text = "";
    }

    public void setUserAnswer(string strUserAnswer)
    {

        if (difficulty == DifficultyLevel.Intermediate)
        {
            setIntermediateUserAnswer(strUserAnswer);
        }
        else 
        {
            if (strUserAnswer.ToLower().Contains("y"))
            {
                userAnswer = 1;
            } else if (strUserAnswer.ToLower().Contains("n"))
            {
                userAnswer = 0;
            } else
            {
                userAnswer = (answer == 1 ? 0 : 1);
            }
        }
    }

    public void setIntermediateUserAnswer(string strUserAnswer)
    {
        if (strUserAnswer.Contains("/"))
        {
            string strNumerator = strUserAnswer.Substring(0, (int)strUserAnswer.IndexOf("/"));
            string strDenominator = strUserAnswer.Substring((int)strUserAnswer.IndexOf("/") + 1, strUserAnswer.Length - (int)strUserAnswer.IndexOf("/") - 1);

            int nNumerator;
            int numerator = int.TryParse(strNumerator, out nNumerator) ? nNumerator : -1;

            int nDenominator;
            int denominator = int.TryParse(strDenominator, out nDenominator) ? nDenominator : -1;

            userAnswer = Mathf.RoundToInt(numerator / (float)denominator * 100);
        }
        else if (strUserAnswer.Length > 0)
        {
            int nUserAnswer;
            userAnswer = int.TryParse(strUserAnswer, out nUserAnswer) ? nUserAnswer : -1;
        }
    }

    public void checkAnswer()
    {
        questionText.text = "";
        submitBtn.SetActive(false);
        answerInpt.SetActive(false);
        
        if (userAnswer == answer)
        {
            answerText.text = "Correct!";
        } else
        {
            answerText.text = "That is not correct! The right answer is ";
            if (difficulty == DifficultyLevel.Intermediate)
            {
                answerText.text +=  answer + ".";
            } else
            {
                answerText.text += (answer == 0 ? "no" : "yes") + ".";
            }
        }

        startTime = Time.time;
    }

    private void Update()
    {
        if (startTime > 0)
        {
            float elapsedTime = Time.time - startTime;
            if (elapsedTime > maxTime)
            {
                questionPanel.SetActive(false);
                startTime = 0;
                diceRollManager.GetComponent<DiceRollManager>().displayRoll();
            }
        }
    }
}
