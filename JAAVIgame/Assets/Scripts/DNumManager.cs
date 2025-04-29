using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DNumManager : MonoBehaviour
{
    public GameObject DamageNumberPrefab; //assign in inspector
    [SerializeField] private float pos = 0.0f; //space between individual damage counters


    public void CreateCounter(GameObject player){
        GameObject DNum = Instantiate(DamageNumberPrefab, new Vector3(360 + pos, 540, 0), Quaternion.identity); //weird UI settings that I will probably fix later
        DNum.transform.parent = transform;
        DNum.GetComponent<Damage_Number>().player = player;
        pos += 400.0f;
    }
}
