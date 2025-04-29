using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        string[] allowedScenes = { "StartMenu", "CharacterSelect", "CharacterSelectLocal", "OnlineOptions" }; // Add scene names where music should persist

        if (!System.Array.Exists(allowedScenes, scene => scene == SceneManager.GetActiveScene().name))
        {
            Destroy(gameObject);
            return;
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}