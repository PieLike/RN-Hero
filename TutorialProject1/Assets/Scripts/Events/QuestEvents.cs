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


    public static event Action<string> OnEnemyKilled;
    public static string killedEnemyName = "";
    public static void SendEnemyKilled(string enemyName)  //когда вызывается где либо SendPigKilled() то вызывается отсюда всё методы, записанные в Action OnEnemyKilled
    {
        killedEnemyName = enemyName;
        if (OnEnemyKilled != null) OnEnemyKilled.Invoke(killedEnemyName);

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
}
