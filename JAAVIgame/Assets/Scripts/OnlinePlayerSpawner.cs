using FishNet;
using FishNet.Connection;
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
    private int _nextSpawn = 0;

    public void Awake()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
    }
    public void Spawn(NetworkObject playerPrefab, NetworkConnection conn)
    {
        Vector3 position;
        Quaternion rotation;
        SetSpawn(_playerPrefab.transform, out position, out rotation);

        NetworkObject nob = _networkManager.GetPooledInstantiated(playerPrefab, position, rotation, InstanceFinder.IsServerStarted);
        _networkManager.ServerManager.Spawn(nob, conn);
    }

    private void SetSpawn(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        Transform result = Spawns[_nextSpawn];

        if(result == null)
        {
            pos = prefab.position;
            rot = prefab.rotation;
        }
        else
        {
            pos = result.position;
            rot = result.rotation;
        }
        _nextSpawn++; 
        if(_nextSpawn >= Spawns.Length)
            _nextSpawn = 0;

    }
}
