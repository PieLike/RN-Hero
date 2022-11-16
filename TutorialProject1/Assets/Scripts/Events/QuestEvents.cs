using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public static event Action OnQuestsChange;
    public static void SendQuestsChange()
    {
        if (OnQuestsChange != null) OnQuestsChange.Invoke();
    }


    public static event Action<KilledEnemy> OnEnemyKilled;
    public static KilledEnemy killedEnemy = new KilledEnemy();
    public static void SendEnemyKilled(KilledEnemy enemy)  //когда вызывается где либо SendEnemyKilled() то вызывается отсюда всё методы, записанные в Action OnEnemyKilled
    {
        killedEnemy = enemy;
        if (OnEnemyKilled != null) OnEnemyKilled.Invoke(killedEnemy);

    }
    public struct KilledEnemy
    {
        public Enemy enemy;
        public Transform spawnPoint;
    }

    public static event Action<string> OnAreaReach;
    public static string reachedAreaName = "";
    public static void SendReachArea(string areaName)
    {
        reachedAreaName = areaName;
        if (OnAreaReach != null) OnAreaReach.Invoke(reachedAreaName);

    }

    public static event Action<string> OnInteractionNpc;
    public static string interactionNpcName = "";
    public static void SendInteractionNpc(string npcName)
    {
        interactionNpcName = npcName;
        if (OnInteractionNpc != null) OnInteractionNpc.Invoke(interactionNpcName);

    }

    public static event Action<string> OnAreaEnable;
    public static string enabledAreaName = "";
    public static void SendEnableArea(string areaName)
    {
        enabledAreaName = areaName;
        if (OnAreaEnable != null) 
            OnAreaEnable.Invoke(enabledAreaName);

    }
}
