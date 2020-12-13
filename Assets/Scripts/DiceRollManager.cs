using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollManager : MonoBehaviour
{

    int amount;
    int type;

    List<int> results;

    public GameObject panel;
    public GameObject btnResults;
    public GameObject btnResultsSum;

    float maxTime = 3.0f;
    float startTime = 0;

    public void throwDice()
    {
        if (amount > 0 && type > 0)
        {
            results = new List<int>();
            int sum = 0;
            string strResults = "";
            for (int i = 0; i < amount; i++)
            {
                int value = Random.Range(1, type + 1);
                results.Add( value );
                sum += value;
                strResults += value + ((i+1) == amount ? "" : " + ");
            }
            panel.SetActive(false);
            startTime = Time.time;

            btnResults.SetActive(true);
            btnResultsSum.SetActive(true);

            btnResults.GetComponent<UnityEngine.UI.Text>().text = strResults;
            btnResultsSum.GetComponent<UnityEngine.UI.Text>().text = sum + "";
        }
    }

    public int getResultsSum()
    {
        int sum = 0;
        for (int i = 0; i < results.Count; i++)
        {
            sum += results[i];
        }
        return sum;
    }

    public List<int> getIntResults()
    {
        return results;
    }

    public string getStrResults()
    {
        string strResults = "";
        for (int i = 0; i < results.Count; i++)
        {
            strResults += results[i] + " ";
        }
        return strResults;
    }

    public void setAmount(int nAmount)
    {
        amount = nAmount;
    }

    public void setAmount(string strAmount)
    {
        if (strAmount.Length > 0)
        {
            int nAmount;
            amount = int.TryParse(strAmount, out nAmount) ? nAmount : 0;
        }        
    }

    public void setType(int nType)
    {
        type = nType;
    }

    private void Update()
    {
        if (btnResults != null && startTime > 0)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime > maxTime)
                {
                    btnResults.SetActive(false);
                    btnResultsSum.SetActive(false);
                }
            }
    }
}
