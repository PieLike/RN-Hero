using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulation
{
    public static void DoSlowmotion(float factor)
    {
        Time.timeScale = factor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public static void EndSlowmotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
