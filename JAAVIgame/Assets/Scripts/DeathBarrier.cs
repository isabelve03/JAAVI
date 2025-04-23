using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Scened;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{

    public void ProcessPlayerDeath() 
    {
        // code to be able to check how many players connected to the game, remove them from array
        // until there is one player left, save them as winner and end game
        
        ResetGameSession();
    }

    public void ResetGameSession() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

}
