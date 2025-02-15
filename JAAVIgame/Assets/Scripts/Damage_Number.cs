using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage_Number : MonoBehaviour
{
    //public Knockback knockback;
    [SerializeField] private static int hp;
    //damage logic
    [SerializeField] private int damage = 0;

    // This is our HUD image UI Object
    [SerializeField] public static Image damNum;
    [SerializeField] public static Image damNum2;
    [SerializeField] public static Image damNum3;
    [SerializeField] public Sprite[] damageTex;
    
    // Start is called before the first frame update
    void Start()
    {
        //accesses child elements
        damNum = gameObject.transform.Find("Damage_Num").GetComponent<Image>();
        damNum2 = gameObject.transform.Find("Damage_Num2").GetComponent<Image>();
        damNum3 = gameObject.transform.Find("Damage_Num3").GetComponent<Image>();

        //default health
        damNum3.enabled = false;
        damNum2.enabled = false;
        damNum.sprite = damageTex[0];
        damNum.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        hp = gameObject.transform.root.Find("Player1").Find("KnightPlayer").GetComponent<Knockback>().hitPoints; //obtains current hitPoints for character
        if(damage != hp){ //do not want to reprint the damage counters each frame if the damage does not change
            damage = hp;
            if(damage < 10){
                damNum3.enabled = false;
                damNum2.enabled = false;
                damNum.sprite = damageTex[damage];
                damNum.enabled = true;
            }
            else if(damage >= 10 && damage < 100){
                damNum3.enabled = false;
                damNum2.sprite = damageTex[damage / 10];
                damNum2.enabled = true;
                damNum.sprite = damageTex[damage % 10];
                damNum.enabled = true;
            }
            else if(damage >= 100 && damage < 999){
                damNum3.sprite = damageTex[damage / 100];
                damNum3.enabled = true;
                damNum2.sprite = damageTex[(damage - (damage / 100 * 100)) / 10];
                damNum2.enabled = true;
                damNum.sprite = damageTex[damage % 10];
                damNum.enabled = true;
            }
            else if(damage >= 999){ //damage should not exceed 999 to begin with.  This is just a safeguard
                damNum3.sprite = damageTex[9];
                damNum3.enabled = true;
                damNum2.sprite = damageTex[9];
                damNum2.enabled = true;
                damNum.sprite = damageTex[9];
                damNum.enabled = true;
            }
        }
    }
}