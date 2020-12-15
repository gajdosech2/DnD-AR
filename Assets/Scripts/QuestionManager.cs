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

    public void activateSelf()
    {
        generateQuestion(diceRollManager.GetComponent<DiceRollManager>().getType());
        questionPanel.SetActive(true);
        submitBtn.SetActive(true);
        answerInpt.SetActive(true);
    }

    void generateQuestion(int dieType)
    {
        answer = -1;
        question = "";
        
        question += "On d" + dieType + " what is the probability that the number will be ";
        int rr = Random.Range(0, 3);
        int number = Random.Range(1, dieType + 1);
        if ( rr == 0)
        {
            question += "greater than " + number + "?";
            answer = Mathf.RoundToInt((dieType - number) / (float)dieType * 100);
        }
        else if ( rr == 1)
        {
            question += "less than " + number + "?";
            answer = Mathf.RoundToInt((number - 1) / (float)dieType * 100);
        }
        else if (rr == 2)
        {
            question += "at least " + number + "?";
            answer = Mathf.RoundToInt((dieType - (number - 1)) / (float)dieType * 100);
        } else
        {
            question += "at most " + number + "?";
            answer = Mathf.RoundToInt(number / (float)dieType * 100);
        }        

        questionText.text = question;
        answerText.text = "";
    }

    public void setUserAnswer(string strUserAnswer)
    {
        if (strUserAnswer.Length > 0)
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
            answerText.text = "That is not correct! The right answer is " + answer + ".";
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
