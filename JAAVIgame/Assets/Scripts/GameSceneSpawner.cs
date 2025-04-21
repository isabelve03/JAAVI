using UnityEngine;
using System.Collections.Generic;

public class GameSceneSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    void Start()
    {
        var players = PlayerJoinManager.Instance.joinedPlayers;

        for (int i = 0; i < players.Count; i++)
        {
            var data = players[i];
            if (data.selectedCharacter == null)
            {
                Debug.LogWarning("Player " + i + " has no character selected!");
                continue;
            }

            GameObject obj = Instantiate(data.selectedCharacter, spawnPoints[i].position, Quaternion.identity);
            Debug.Log("Spawning player " + i + " at " + spawnPoints[i].position);
            var move = obj.GetComponent<PlayerMovement>();
            if (move != null) move.SetControllerID(data.controllerID);
        }
    }
}
