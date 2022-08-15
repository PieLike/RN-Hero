using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellBehavior : MonoBehaviour
{
    public float speed, distance, damage = 0;
    private string behaviorType;
    private double roundAbsStartRelativePointY;

    public void NonPhysicProjectile(float speedNew, float distanceNew, float damageNew)
    {
        speed = speedNew; distance = distanceNew; damage = damageNew;
        behaviorType = "NPP";
        roundAbsStartRelativePointY = MyMathCalculations.RoundAbs(transform.InverseTransformPoint(0, 0, 0).y);
    }

    private void Update() 
    {
        switch (behaviorType)
        {
            case "NPP" :
            //nonPhysicProjectile
            if (MyMathCalculations.RoundAbs(transform.InverseTransformPoint(0, 0, 0).y) != roundAbsStartRelativePointY + distance)
                transform.Translate(Vector3.up * Time.deltaTime * speed);
            else
                Destroy(gameObject);
            break;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag != "Hero" && other.gameObject.tag != "Loot")
        {
            Destroy(gameObject);

        }
    }
}
