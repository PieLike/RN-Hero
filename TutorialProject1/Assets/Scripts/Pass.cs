using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass : MonoBehaviour
{
    public enum PassType {vertical, horizontal}
    public PassType passType;
    private GameObject objFinalPoint;//, prefabFinalPoint;
    private bool contactPointIsFound, top, right;
    BoxCollider2D boxSize;
    Vector3 contactPoint, centerPoint;

    private void Start() 
    {
        //находим префабы в ресурсах
        //prefabFinalPoint = Resources.Load<GameObject>("3d_prefabs/FinalPoint2D");
        objFinalPoint = GameObject.Find("FinalPoint2D");

        boxSize = GetComponent<BoxCollider2D>(); 
    }

    /*private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Hero")
        { 
            FindContactPoint(other);
            if (contactPointIsFound)
                DoPass();  
        }        
    }*/
    private void OnTriggerStay2D(Collider2D other) 
    {
        FindContactPoint(other);

        if (other.gameObject.tag == "Hero" && contactPointIsFound == true)
        {
            if (passType == PassType.vertical)
            {
                if (top)
                {                    
                    if(Input.GetAxis("Vertical") > 0)
                    {
                        DoPass();
                    } 
                }
                else
                {
                    if(Input.GetAxis("Vertical") < 0)
                    {
                        DoPass();
                    } 
                } 
            }
            else
            {
                if (right)
                {                    
                    if(Input.GetAxis("Horizontal") > 0)
                    {
                        DoPass();
                    }   
                }
                else
                {
                    if(Input.GetAxis("Horizontal") < 0)
                    {
                        DoPass();
                    }
                }                     
            }
        }              
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
            contactPointIsFound = false;
    }

    private void FindContactPoint(Collider2D other)
    {
        contactPoint = other.ClosestPoint(transform.position);
        centerPoint = other.bounds.center;

        right = contactPoint.x > centerPoint.x;
        top = contactPoint.y > centerPoint.y; 

        contactPointIsFound = true;         
    }

    public void DoPass()
    {
        //Debug.Log("pass");
        if (contactPointIsFound == false) return;
        //FindFinalPoint();
        
        if (objFinalPoint != null)
        {           
            if (objFinalPoint.activeSelf != true)
                objFinalPoint.SetActive(true);

            var finalPointTransform = objFinalPoint.GetComponent<Transform>();

            if (passType == PassType.vertical)
            {
                if (top)
                {                    
                    finalPointTransform.position = new Vector3(contactPoint.x, contactPoint.y + boxSize.size.y, 0f); 
                }
                else
                    finalPointTransform.position = new Vector3(contactPoint.x, contactPoint.y - boxSize.size.y, 0f); 
            }
            else
            {
                if (right)
                {                    
                    finalPointTransform.position = new Vector3(contactPoint.x + boxSize.size.x, contactPoint.y, 0f); 
                }
                else
                    finalPointTransform.position = new Vector3(contactPoint.x - boxSize.size.x, contactPoint.y, 0f); 
            }
               
            MainVariables.inMovement = true; 
            MainVariables.forceNewMovementLockWhenMove = true;  
        }  
    }

    /*private void FindFinalPoint()
    {
        if (objFinalPoint == null)
            objFinalPoint = GameObject.Find("FinalPoint2D(Clone)");
        //if (objFinalPoint == null)
        //    objFinalPoint = Instantiate(prefabFinalPoint);
    } */

}
