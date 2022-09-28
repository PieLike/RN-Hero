using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMainData
{
    [SerializeField] private static int _level; public static int level {get{ return _level;} set{ _level = value; wordAbleCount = value *5; }}
    public static int wordAbleCount, wordActualCount; //wordAbleCount = level * 5
    public static float experience;


}
