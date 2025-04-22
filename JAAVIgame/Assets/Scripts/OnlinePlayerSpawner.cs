using FishNet;
using FishNet.Managing;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// called on trhe server when a plyer is spawned
    /// </summary>
    public event Action<NetworkObject> OnSpawned;
    private NetworkObject _playerPrefab;
    public void SetPlayerPrefab(NetworkObject nob) => _playerPrefab = nob;
    public Transform[] Spawns = new Transform[0];
    private NetworkManager _networkManager;
    private int _nextSpawn;

    //public void Spawn
}
