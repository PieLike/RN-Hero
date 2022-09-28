using UnityEngine;
using System;

public class MainVariables
{
    [NonSerialized] public static bool inMovement = false, forceNewMovementLockWhenMove = false, allowMovement = false, isWalkingByKeys = false;
    [NonSerialized] public static bool inImpacting;
    [NonSerialized] public static bool inSpelling = false;
    [NonSerialized] public static bool inInteraction = false;
    [NonSerialized] public static bool inInterface = false;
    [NonSerialized] public static bool inTalking = false; 
    [NonSerialized] public static int activeSpellSlot = 1;
    [NonSerialized] public static string[] interactionTags = new string[] {"Enemy","Word","Chest","Npc","Pass","Branch"};
    [NonSerialized] public static bool autoLoad = true;
    [NonSerialized] public static bool modeWithoutEG = true;
}
