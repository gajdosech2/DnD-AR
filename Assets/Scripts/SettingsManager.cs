using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public Button basicDifficultyBtn;
    public Button intermediateDifficultyBtn;
    public Button advancedDifficultyBtn;

    public GameObject questionManager;

    public void highlightActiveButtons()
    {
        string difficulty = questionManager.GetComponent<QuestionManager>().getDifficulty();
        switch (difficulty)
        {
            case "basic":
                basicDifficultyBtn.Select();
                break;

            case "intermediate":
                intermediateDifficultyBtn.Select();
                break;

            case "advanced":
                advancedDifficultyBtn.Select();
                break;
        }
    }
}
