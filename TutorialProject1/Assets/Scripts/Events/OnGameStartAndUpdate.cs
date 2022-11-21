using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OnGameStartAndUpdate : MonoBehaviour
{
    AudioManager audioManager;
    InterfaceManager interfaceManager; static TMP_Text statsText, statsText2;
    static Queue<Action> interactionQueue;

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

        interactionQueue = new();
    }
    public void Update() 
    {
        if (MainVariables.inInterface && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInterface();
        }

        //Debug.Log(interactionQueue.Count.ToString());
        if (MainVariables.inInterface == false && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionQueue.Count > 0)
            {
                Action action = interactionQueue.Dequeue();
                action.Invoke();
            }
        }

        if(MainVariables.isPassing || MainVariables.inInterface || MainVariables.inTalking || MainVariables.inImpacting || MainVariables.isDashing || MainVariables.isDashing || MainVariables.isDead)
        {
            MainVariables.allowMovement = false;
            MainVariables.allowAttack = false;
        }
        else
        {
            MainVariables.allowMovement = true;
            MainVariables.allowAttack = true;
        }

        if (interfaceManager.PCPanel.activeSelf)
            MainVariables.allowAttack = false;   

        /*if(MainVariables.isPassing || MainVariables.inInterface || MainVariables.inTalking || MainVariables.inImpacting || MainVariables.isDashing || MainVariables.isDashing)
            MainVariables.allowAttack = false;
        else
            MainVariables.allowAttack = true;*/

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

    public static event Action OnInterfaceClose;
    public static void CloseInterface()
    {
        if (OnInterfaceClose != null) OnInterfaceClose.Invoke();
    }

    public static void AddInteraction(Action newAction)
    {
        interactionQueue.Enqueue(newAction);
    }
    public static void RemoveInteraction(Action newAction)
    {
        interactionQueue = new Queue<Action>(interactionQueue.Where(x => x != newAction));
    }
}

