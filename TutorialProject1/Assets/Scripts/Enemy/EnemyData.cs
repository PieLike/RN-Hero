using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public float currentHP, currentSP; 
    public float generalHP = 5f, generalSP = 5f;
    public bool haveShield, isRotten;

    private void Start() 
    {
        currentSP = generalSP;
        currentHP = generalHP;  
    }
}
