using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineDamageNumber : MonoBehaviour
{
    [SerializeField] private static int hp;
    [SerializeField] private int damage = 0;


    [SerializeField] public Image damNum;
    [SerializeField] public Image damNum2;
    [SerializeField] public Image damNum3;
    [SerializeField] public Sprite[] damageTex; //[0,1,2,3,4,5,6,7,8,9]
    [SerializeField] public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        damNum = transform.Find("Damage_Num").gameObject.GetComponent<Image>();
        damNum2 = transform.Find("Damage_Num2").gameObject.GetComponent<Image>();
        damNum3 = transform.Find("Damage_Num3").gameObject.GetComponent<Image>();

        // default health
        damNum3.enabled = false;
        damNum2.enabled = false;
        damNum.sprite = damageTex[0];
        damNum.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        hp = player.GetComponent<Damage_Calculations>().currentHealth; //obtains current hitPoints for character
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
}
