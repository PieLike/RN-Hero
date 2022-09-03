using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public static Action OnQuestsChange;
    public static void SendQuestsChange()
    {
        if (OnQuestsChange != null) OnQuestsChange.Invoke();
    }


    public static Action OnEnemyKilled;
    public static string killedEnemyName = "";
    public static void SendEnemyKilled(string enemyName)  //когда вызывается где либо SendPigKilled() то вызывается отсюда всё методы, записанные в Action OnEnemyKilled
    {
        killedEnemyName = enemyName;
        if (OnEnemyKilled != null) OnEnemyKilled.Invoke();

    }

    public static Action OnAreaReach;
    public static string reachedAreaName = "";
    public static void SendReachArea(string areaName)
    {
        reachedAreaName = areaName;
        if (OnAreaReach != null) OnAreaReach.Invoke();

    }

    public static Action OnInteractionNpc;
    public static string interactionNpcName = "";
    public static void SendInteractionNpc(string npcName)
    {
        interactionNpcName = npcName;
        if (OnInteractionNpc != null) OnInteractionNpc.Invoke();

    }
}
