using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionAddMana : MonoBehaviour
{
    public int level;

    HeroMana heroMana;
    private void Awake() 
    {
        heroMana = FindObjectOfType<HeroMana>();

        if (Use() == true)
            gameObject.GetComponent<PickerWheelPotions>().potionIsUsed = true;
    }
    public bool Use()
    {
        if (HeroMainData.currentMP < (HeroMainData.plainMaxMP + HeroMainData.buffMaxMP))
        {
            float addPoints = GetManaPoints();
            float mpLack = (HeroMainData.plainMaxMP + HeroMainData.buffMaxMP) - HeroMainData.currentMP;
            if (addPoints > mpLack)
                heroMana.RecoverMp(mpLack);
            else
                heroMana.RecoverMp(addPoints);

            return true;
        }
        else
            return false;
    }    

    private float GetManaPoints()
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
    }
}
