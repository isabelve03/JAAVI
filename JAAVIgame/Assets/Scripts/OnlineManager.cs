using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineManager : MonoBehaviour
{
    public static OnlineManager Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(GetComponent<NetworkObject>() == null)
        {
            Debug.LogError("No Network Object on OnlineManager");
            return;
        }
    }


}
