using System.Collections.Generic;
using UnityEngine;

public class DNumManager : MonoBehaviour
{
    public GameObject DamageNumberPrefab; //assign in inspector
    [SerializeField] private float pos; //space between individual damage counters
    // Start is called before the first frame update
    void Start()
    {
        CreateCounter(); //creates necessary damage number counters
    }

    void CreateCounter(){
        var players = PlayerJoinManager.Instance.joinedPlayers;
        for(int i = 0; i < players.Count; i++){
            Debug.Log("Player " + i + " damage counter added");

            if (players[i].selectedCharacter == null)
            {
                Debug.LogWarning("Player " + i + " has no character selected!");
                continue;
            }

            GameObject DNum = Instantiate(DamageNumberPrefab, new Vector3(0 + pos, 0, 0), Quaternion.identity);
            DNum.transform.parent = transform;
            DNum.GetComponent<Damage_Number>().player = players[i].selectedCharacter;
            pos += 5.0f;
        }
    }
}
