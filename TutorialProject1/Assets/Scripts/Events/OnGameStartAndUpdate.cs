using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnGameStartAndUpdate : MonoBehaviour
{
    AudioManager audioManager;
    InterfaceManager interfaceManager; static TMP_Text statsText, statsText2;

    private void Awake() 
    {
        HeroMainData.level = 1;
    }
    private void Start() 
    {
        if (MainVariables.autoLoad)
        {
            SaveManager saveManager = FindObjectOfType<SaveManager>();
            saveManager.LoadSave();   
        }
                
        HeroMainData.plainSpeed = 5;
        HeroMainData.plainDamage = 1;
        HeroMainData.plainSpeedAttack = 1;

        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("desertAmbience");

        interfaceManager = FindObjectOfType<InterfaceManager>();
        statsText = interfaceManager.Stats.GetComponent<TMP_Text>();
        statsText2 = interfaceManager.Stats2.GetComponent<TMP_Text>();
    }
    public void Update() 
    {
        if(MainVariables.isPassing || MainVariables.inInterface || MainVariables.inTalking || MainVariables.inImpacting || MainVariables.isDashing)
            MainVariables.allowMovement = false;
        else
            MainVariables.allowMovement = true;

        if(MainVariables.isPassing || MainVariables.inInterface || MainVariables.inTalking || MainVariables.inImpacting || MainVariables.isDashing)
            MainVariables.allowAttack = false;
        else
            MainVariables.allowAttack = true;

        //statsText = StatsUpdate();
    }

    public static void StatsUpdate()
    {
        string text =   "Максимум жизней: " + HeroMainData.plainMaxHP + "(+" + HeroMainData.buffMaxHP + ")" + "\n" +
                        "Количество урона: " + HeroMainData.plainDamage + "(+" + HeroMainData.buffDamage + ")" + "\n" +
                        "Скорость атаки: " + HeroMainData.plainSpeedAttack + "(+" + HeroMainData.buffSpeed + ")" + "\n" +
                        "Скорость передвижения: " + HeroMainData.plainSpeed + "(+" + HeroMainData.buffSpeedAttack + ")"  + "\n" +
                        "Переносимые зелья: " + HeroMainData.maxPotionWithSelf;
        statsText.text = text;
        statsText2.text = text;
    }
}

