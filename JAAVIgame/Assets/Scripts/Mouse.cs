using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else if (SceneManager.GetActiveScene().name == "OnlineOptions")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else if (SceneManager.GetActiveScene().name == "CharacterSelectLocal")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    
}