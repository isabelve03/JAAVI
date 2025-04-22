using FishNet;
using FishNet.Object;
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
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Instance is not null");
            Destroy(gameObject);
        }
    }

    public void SpawnOnServer()
    {
            if (InstanceFinder.IsServerStarted)
            {
                NetworkObject netObj = GetComponent<NetworkObject>();
                if(netObj == null)
                {
                    Debug.Log("Net obj is null");
                }else if (!@netObj.IsSpawned)
                {
                    Debug.Log("Net obj is not null, but it is not spanwed");
                }
                else
                {
                    Debug.Log("Net obj is not null and is spawned");
                }
            }
            else
            {
                Debug.Log("Server is not started");
            }
    }
}
