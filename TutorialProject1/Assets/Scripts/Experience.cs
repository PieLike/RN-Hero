using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    private static InterfaceManager interfaceManager;
    private static Image expLine; private float smoothTime = 0.25f, Velocity = 0.0f;
    private static float prevExp, expUpdate, expUpdateOver;    
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        expLine = interfaceManager.ExpLine.GetComponent<Image>();
        prevExp = -1f;  //для первого заполнения
        UpdateExp();
    }
    public static void AddExp(float exp)
    {
        //interfaceManager.messageScript.NewMessage("add exp ", exp.ToString());
        HeroMainData.experience += exp;
        UpdateExp();
    }

    private static void UpdateExp()
    {
        if (HeroMainData.experience != prevExp)
        {
            float levelExp = FindLevelExp(HeroMainData.level);
            //float levelExpPrev = FindLevelExp(HeroMainData.level-1);

            if (HeroMainData.experience < levelExp)
            {
                expUpdate = HeroMainData.experience;
            }
            else
            {
                expUpdate = levelExp;
                expUpdateOver = HeroMainData.experience - levelExp;
            }            

            //expUpdate = (HeroMainData.experience % (levelExp-levelExpPrev)) / (levelExp-levelExpPrev); 
            prevExp = HeroMainData.experience;
        } 
    }
    void Update()
    {
        float update = expUpdate / FindLevelExp(HeroMainData.level);
        if (expLine.fillAmount != update)
        {
            float smoothExpUpdate = Mathf.SmoothDamp(expLine.fillAmount, update, ref Velocity, smoothTime);                
            expLine.fillAmount = smoothExpUpdate;
        }
        if (expLine.fillAmount > 0.95f && expUpdateOver > 0)
        {          
            HeroMainData.level ++;
            HeroMainData.skillPoints ++;

            interfaceManager.messageScript.NewMessage("Вы повысили свой уровень", "Теперь ваш уровень равен " + HeroMainData.level.ToString());

            expLine.fillAmount = 0;

            expUpdate = expUpdateOver;
            expUpdateOver -= FindLevelExp(HeroMainData.level);
        }
    }

    private static float FindLevelExp(int _level = -1)
    {
        if (_level == -1)    _level = HeroMainData.level;
        switch (_level)
        {
            case 1: return 10f;
            case 2: return 40f;
            case 3: return 120f;
            default: return (HeroMainData.level - 2) * 120f;
        }
        
    }
}
