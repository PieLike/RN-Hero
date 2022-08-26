//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroMove : MonoBehaviour
{
    [NonSerialized] public static bool isReached = false;
    public float speed = 700f;   
    private Rigidbody rigidBody;
    private GameObject objFinalPoint;

    private void Start() 
    {
        rigidBody = GetComponent<Rigidbody>();  
    }

    private void FixedUpdate() 
    {
        //если герой в движении
        if (MainVariables.inMovement)
        { 
            FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)
            //проверяем добрался ли до точки, если да то заканчиваем статус "движение"
            //иначе двигаем героя в точке
            if (objFinalPoint != null)
            {
                if (MyMathCalculations.CheckReachToPoint(transform.position, objFinalPoint.transform.position) == false && (objFinalPoint.activeSelf == true))
                {
                    Vector3 direction = MyMathCalculations.CalculateDirectionSpeeds(transform.position, objFinalPoint.transform.position, speed); 
                    rigidBody.velocity = transform.TransformDirection(direction * Time.fixedDeltaTime); 
                }
                else
                {
                    MainVariables.inMovement = false;
                    if (MainVariables.inInteraction == false)
                        objFinalPoint.SetActive(false);                    
                }
            }
        }
        if (isReached == true)
            isReached = false;   
    }

    public void LateUpdate() 
    {    
        
        //при клике мыши Не на интерфейс, не во время заклинания, не во время интерфейса и не на объект
        //задаём точку движения "к чему" и включаем движение
        if (Input.GetMouseButtonDown(0) && UIClick.OnMouseDown() && MainVariables.inSpelling == false && MainVariables.inInterface == false && Interaction.supposedInteractionObject == null)
        {
            FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)                        
            if (objFinalPoint != null)
            {
                if (objFinalPoint.activeSelf == true)
                {
                    MainVariables.inMovement = true;    
                }
            }
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

    private void OnTriggerStay(Collider other) 
    {
        FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)
        if (objFinalPoint != null)
        {
            if (other.gameObject == objFinalPoint)
            {
                isReached = true;

                if (MainVariables.inMovement == true)
                {
                    MainVariables.inMovement = false;
                    if (MainVariables.inInteraction == false)
                        objFinalPoint.SetActive(false);     
                }   
            }
        }     
    }
    private void FindFinalPoint()
    {
        if (objFinalPoint == null)
            objFinalPoint = GameObject.Find("FinalPoint(Clone)");
    }  
}
