//using System.Collections;
//using System.Collections.Generic;
using System;
using UnityEngine;
//using System.Data;

public class AutoTakeUp : MonoBehaviour
{
    [SerializeField] private GameObject lootItem;
    private GameObject heroObject;
    private Rigidbody lootItemRigidBody; 
    private bool inDragging = false;  
    [NonSerialized] public float speed;
    private Vector3 finalPoint;  

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Hero")
        {
            if (lootItem.tag == "Word")
            {
                if (Interaction.CheckExisting(lootItem) == true)
                {
                    TakeUpTo(other.gameObject); 
                }
            }
            else if(lootItem.tag != "Untagged")
            {
                TakeUpTo(other.gameObject);
            }            
        }    
    }

    private void TakeUpTo(GameObject DraggingToObject)
    {
        lootItemRigidBody = lootItem.GetComponent<Rigidbody>();
        inDragging = true;
        heroObject = DraggingToObject;
    }

    void FixedUpdate()
    {
        if (inDragging == true)
        {
            //проверяем притянули ли объект к герою
            if (MyMathCalculations.CheckReachToPoint(lootItem.transform.position, finalPoint))
            {
                inDragging = false;
                //TakeUp();
                if (lootItem.tag == "Word")
                    Interaction.TakeUpWord(lootItem);

            }
            //если нет то задаем точку притяжения (каждый раз новую потмоу что герой может двигаться) и двигаем объект
            else if (heroObject != null)
            {
                finalPoint = heroObject.transform.position;
            
                Vector3 direction = MyMathCalculations.CalculateDirectionSpeeds(lootItem.transform.position, finalPoint, speed);
                lootItemRigidBody.MovePosition(lootItem.transform.position + direction * Time.deltaTime);      
            }
        }
    }


}
