using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just here to be able to allow NetworkBehavour script to persist
public class OnlineManager : MonoBehaviour
{
    public static OnlineManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
