using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public PlayerHealth health;
    public PlayerMovement pm;

    private void Start()
    {
        health = transform.GetComponentInParent<PlayerHealth>();
        pm = transform.GetComponentInParent<PlayerMovement>();
    }
}
