using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Number : MonoBehaviour
{
    //damage logic
    [SerializeField] public static int damage = 0;

    // This is our HUD image UI Object
    [SerializeField] public state Image hud;

    //damage num sprites

    [SerializeField] Sprite damage0tex

    [SerializeField] Sprite damage1tex

    [SerializeField] Sprite damage2tex

    [SerializeField] Sprite damage3tex

    [SerializeField] Sprite damage4tex

    [SerializeField] Sprite damage5tex

    [SerializeField] Sprite damage6tex

    [SerializeField] Sprite damage7tex

    [SerializeField] Sprite damage8tex

    [SerializeField] Sprite damage9tex


    // Start is called before the first frame update
    void Start()
    {
        hud = gameObject,GetComponentInChildren<Image>();

        //if u wanna hide hud
        //hud.emabled = false;

        //set initial damagecount value
        damage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (damage == 1)
        {
            hud.sprite = damage1tex
            //hud.enabled = true;
        }
    }
}
