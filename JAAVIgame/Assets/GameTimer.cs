using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;  // Add this to reference UI text
    private float timeRemaining = 10f;
    private bool isGameActive = false;

    void Start()
    {
        isGameActive = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isGameActive)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);

            if (timeRemaining <= 0)
            {
                timeRemaining = 0f;
                DisplayTime(timeRemaining);
                EndGame();
            }
        }
    }

    void EndGame()
    {
        isGameActive = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("StartMenu"); // Load a Game Over screen instead
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeText != null)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
    }
}
