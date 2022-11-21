using UnityEngine;
using System;

public class MainVariables
{
    [NonSerialized] public static bool inMovement, isPassing, allowMovement, isWalkingByKeys, isDashing, isDead;
    [NonSerialized] public static bool allowAttack;
    [NonSerialized] public static bool inImpacting;
    [NonSerialized] public static bool inSpelling;
    [NonSerialized] public static bool inInteraction;
    [NonSerialized] public static bool inInterface;
    [NonSerialized] public static bool inTalking; 
    [NonSerialized] public static int activeSpellSlot = 1;
    [NonSerialized] public static string[] interactionTags = new string[] {"Word","Chest","Npc","Pass","Pot"};
    [NonSerialized] public static bool autoLoad = false;
    [NonSerialized] public static bool modeWithoutEG = true;

//experience
    [NonSerialized] public static float expForWord = 1f; //expForNewWord = 10f, 
    [NonSerialized] public static float expForEnemyKill = 5f;
    [NonSerialized] public static float expForWordLearn = 50f;
    [NonSerialized] public static float expForPotionCreate = 15f;

//
    public enum atackType {none, ice, fire};

    // artifacts
    [NonSerialized] public static bool canMeleeAttack = false;
}
