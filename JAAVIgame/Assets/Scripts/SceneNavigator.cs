using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// TODO - This script should be TEMPORARY!!!
// TODO - Replace this script with our actual scene navigator system
public class SceneNavigator : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("TEST_ONLINE_BATTLE");
    }
}
