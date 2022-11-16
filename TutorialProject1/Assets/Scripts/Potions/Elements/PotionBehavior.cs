using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{
    //public int level;
    private float timeDashDisable, timeDashEnable;    

    public virtual void Awake() 
    {
                
    }
    public virtual bool Use()
    {
        return false;
    }    
}
