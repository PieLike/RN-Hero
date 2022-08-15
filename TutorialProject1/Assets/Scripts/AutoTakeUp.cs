using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTakeUp : MonoBehaviour
{
    public GameObject lootItem;
    private GameObject heroObject;
    private Rigidbody lootItemRigidBody; 
    private bool inDragging = false;  
    public float speed;
    private Vector3 finalPoint; 

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Hero")
        {
            TakeUpTo(other.gameObject);            
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
            if (MyMathCalculations.CheckReachToPoint(lootItem.transform.position, finalPoint))
            {
                inDragging = false;
                TakeUp();

            }
            else if (heroObject != null)
            {
                finalPoint = heroObject.transform.position;//heroObject.GetComponent<Transform>().position;
            
                Vector3 direction = MyMathCalculations.CalculateDirectionSpeeds(lootItem.transform.position, finalPoint, speed);
                lootItemRigidBody.MovePosition(lootItem.transform.position + direction * Time.deltaTime);      
            }
        }
    }

    private void TakeUp()
    {
        Destroy(lootItem);
    }
}
