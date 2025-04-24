using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; // for Legacy Text

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryScreen;
    public Text winnerText;
    public float delayBeforeReturn = 5f; // Time in seconds before returning to the main menu

    private void Start()
    {
        // Initially make sure the victory screen is hidden
        victoryScreen.SetActive(false); 
    }
    
    public void ShowVictoryScreen(string winnerName)
    {
        victoryScreen.SetActive(true);
        winnerText.text = $"{winnerName} got that";

        // Start coroutine to wait and return to the main menu
        StartCoroutine(WaitAndReturn());
    }

    private IEnumerator WaitAndReturn()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeReturn);

        // Return to the main menu scene
        SceneManager.LoadScene("StartMenu"); 
    }
}
