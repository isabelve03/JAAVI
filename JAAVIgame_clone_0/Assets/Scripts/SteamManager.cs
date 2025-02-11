using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    public uint appId = 480; // TODO - Change when we get our own app id
    private void Awake()
    {
        DontDestroyOnLoad(this); // ensures SteamManager script persists past new scenes

        try // try to initialize steamclient and ensure it exists
        {
            if (Steamworks.SteamClient.RestartAppIfNecessary(appId)) return; // not launched with steam
            Steamworks.SteamClient.Init(appId, true);
            Debug.Log($"User's Steam name: {SteamClient.Name}");
        }catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }
}
