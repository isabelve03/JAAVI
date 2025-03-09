using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float fdamage = 20;

    public LayerMask layerMask;

    private void OriggerEnter2D(Collider2D collision)
    {
        if(layerMask == (layerMask | (1 << collision.transform.gameObject.layer))){
            Hurtbox h = collision.GetComponent<Hurtbox>();

            if(h != null){
                h.health.fhealth += fdamage;
            }
            
        }

    }
}
