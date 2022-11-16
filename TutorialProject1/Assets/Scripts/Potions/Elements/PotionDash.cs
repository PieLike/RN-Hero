using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDash : PotionBehavior
{   
    HeroDash heroDash;
    public override void Awake() 
    {
        heroDash = FindObjectOfType<HeroDash>();

        if (Use() == true)
            gameObject.GetComponent<PickerWheelPotions>().potionIsUsed = true;
    }
    public override bool Use()
    {
        //if (heroDash.dashEnabled == false)
        //{
            heroDash.EnableDash(10f);  //GetDashDuration()
            return true;
        //}
        //else
        //    return false;
    }    

    /*private float GetDashDuration()
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
