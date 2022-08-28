//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using System;

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

        //выпрямляем вращение дочерних элементов в 0,0,0 по мировым координатам
        //чтобы доп спрайты не поворачивались при использовании заклинания
        UnRotateChildrens();
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

    private void UnRotateChildrens()
    {
        Transform[] childTransform = GetComponentsInChildren<Transform>(false);
        foreach (var item in childTransform)
            if (item != null && item != transform)
            {
                item.eulerAngles = new Vector3(0,0,0);
            }
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag != "Hero" && other.gameObject.tag != "Loot" && other.gameObject.tag != "Spell")
        {
            if(other.gameObject.GetComponent<MeshRenderer>() != null)
                Destroy(gameObject);

        }
    }
}
