using UnityEngine;
using System;

public class MainVariables : MonoBehaviour
{
    [NonSerialized] public static bool inMovement = false;
    [NonSerialized] public static bool inSpelling = false;
    [NonSerialized] public static bool inInteraction = false;
    [NonSerialized] public static bool inInterface = false;
    [NonSerialized] public static int activeSpellSlot = 1;
}
