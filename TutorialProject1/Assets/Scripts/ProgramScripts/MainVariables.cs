using UnityEngine;
using System;

public class MainVariables
{
    [NonSerialized] public static bool inMovement = false;
    [NonSerialized] public static bool inSpelling = false;
    [NonSerialized] public static bool inInteraction = false;
    [NonSerialized] public static bool inInterface = false;
    [NonSerialized] public static int activeSpellSlot = 1;
    [NonSerialized] public static string[] interactionTags = new string[] {"Enemy","Word","Chest","Npc"};
    [NonSerialized] public static bool autoLoad = true;
    [NonSerialized] public static bool modeWithoutEG = true;


}
