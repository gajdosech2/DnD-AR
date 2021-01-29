using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlightManager : MonoBehaviour
{

    public Text description;
    public List<string> descriptions;

    public List<Button> amountButtons;
    public List<Sprite> amountSpritesOn;
    public List<Sprite> amountSpritesOff;

    public List<Button> typeButtons;
    public List<Sprite> typeSpritesOn;
    public List<Sprite> typeSpritesOff;

    public void highlightTypeButton(int id)
    {
        for (int i = 0; i < 7; i++)
        {
            if (i == id)
            {
                typeButtons[i].image.sprite = typeSpritesOn[i];
                description.text= descriptions[i];
            }
            else
            {
                typeButtons[i].image.sprite = typeSpritesOff[i];
            }
        }
    }

    public void highlightAmountButton(int id)
    {
        for (int i = 0; i < 9; i++)
        {
            if (i == id)
            {
                amountButtons[i].image.sprite = amountSpritesOn[i];
            } else
            {
                amountButtons[i].image.sprite = amountSpritesOff[i];
            }            
        }
    }

}
