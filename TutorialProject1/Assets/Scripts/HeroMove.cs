using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroMove : MonoBehaviour
{

    public Vector3 finalPoint;
    public float speed;      
    public GameObject mousePosition;

    public void Update() 
    {       

        if (MainVariables.inMovement)
        { 
            if (MyMathCalculations.CheckReachToPoint(transform.position, finalPoint))
                MainVariables.inMovement = false;
            else
            {
                Vector3 direction = MyMathCalculations.CalculateDirectionSpeeds(transform.position, finalPoint, speed);   
                transform.Translate(direction * Time.deltaTime);        
            }
        }

        if (Input.GetMouseButtonDown(0) && UIClick.OnMouseDown() && MainVariables.inSpelling == false && MainVariables.inInterface == false)
        {
            MainVariables.inMovement = true;

            finalPoint.x = mousePosition.transform.position.x;
            finalPoint.y = transform.position.y;
            finalPoint.z = mousePosition.transform.position.z;
            
        }
        
        


        if (Input.GetMouseButtonDown(1) && UIClick.OnMouseDown())
        {
            //Debug.Log("ПКМ");
        }
        if (Input.GetMouseButtonDown(2) && UIClick.OnMouseDown())
        {
            //Debug.Log("СКМ");
        }    
        
    }    
}
