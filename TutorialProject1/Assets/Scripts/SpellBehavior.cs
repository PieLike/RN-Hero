using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellBehavior : MonoBehaviour
{
    public float speed, distance;
    private string behaviorType;
    private double roundAbsStartRelativePointY;
    private GameObject mainHero;

    public void NonPhysicProjectile(float speedNew, float distanceNew, GameObject mainHeroNew)
    {
        speed = speedNew; distance = distanceNew; mainHero = mainHeroNew;
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
            break;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject != mainHero)
        {
            Destroy(gameObject);

        }
    }
}
