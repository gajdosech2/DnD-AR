using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    public bool GetDown()
    {
        return pressed;
    }

    void LateUpdate()
    {
        pressed = false;
    }
}
