using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionHeal : PotionBehavior
{   
    HeroHealth heroHealth;
    public override void Awake() 
    {
        heroHealth = FindObjectOfType<HeroHealth>();

        if (Use() == true)
            gameObject.GetComponent<PickerWheelPotions>().potionIsUsed = true;
    }
    public override bool Use()
    {
        if (HeroMainData.currentHP < (HeroMainData.plainMaxHP + HeroMainData.buffMaxHP))
        {
            float healPoints = 10f; //GetHealPoints()
            /*float hpLack = HeroMainData.maxHP - HeroMainData.currentHP;
            if (healPoints > hpLack)
                heroHealth.HealHP(hpLack);
            else
                heroHealth.HealHP(healPoints);*/

            heroHealth.EnableSlowHeal(healPoints);

            return true;
        }
        else
            return false;
    }    

    /*private float GetHealPoints()
    {
        switch (level)
        {
            case 1:
                return 10f;
            case 2:
                return 20f;
            case 3:
                return 30f;
            default:
                return 10f;
        }        
    }*/
}
