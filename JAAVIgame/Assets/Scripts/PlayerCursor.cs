using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerCursor : MonoBehaviour
{
    public int controllerID;
    public float moveSpeed = 1000f;

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (controllerID == 0)
        {
            // Track mouse position in UI space
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out mousePos
            );

            rectTransform.anchoredPosition = mousePos;
        }
        else
        {
            // Joystick movement
            Vector2 input = new Vector2
            (
                Input.GetAxis("Joystick " + controllerID + " Horizontal"),
                -Input.GetAxis("Joystick " + controllerID + " Vertical") // flip Y
            );

            rectTransform.anchoredPosition += input * moveSpeed * Time.deltaTime;
        }

        CheckForButtonClick();
    }

    private void CheckForButtonClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = rectTransform.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult raycastResult in results)
        {
            CharacterIconSelector iconSelector = raycastResult.gameObject.GetComponent<CharacterIconSelector>();
            if (iconSelector != null)
            {
                bool isControllerClick = false;
                bool isMouseClick = false;

                if (controllerID == 0)
                {
                    isMouseClick = Input.GetMouseButtonDown(0);
                }
                else
                {
                    isControllerClick = Input.GetKeyDown("joystick " + controllerID + " button 0");
                }

                if (isControllerClick || isMouseClick)
                {
                    iconSelector.SelectCharacter(controllerID);
                    break; // stop after first valid hit
                }
            }
        }
    }

    public void SetControllerID(int id)
    {
        controllerID = id;
    }
}
