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
    Vector3 contactPoint, centerPoint; Vector2 direction; float speed = 300f;
    Rigidbody2D rigidBody;

    private void Start() 
    {
        //находим префабы в ресурсах        
        rigidBody = FindObjectOfType<HeroMove>().gameObject.GetComponent<Rigidbody2D>();
        objFinalPoint = GameObject.Find("FinalPoint2D");

        boxSize = GetComponent<BoxCollider2D>(); 
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {     
        if (other.gameObject.tag == "Hero")
        {
            FindContactPoint(other);
            //if (contactPointIsFound == false) return;

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
        {
            if (MainVariables.isPassing)
            {
                MainVariables.isPassing = false;
                MainVariables.inMovement = false;
            }
            contactPointIsFound = false;
        }
    }

    private void FindContactPoint(Collider2D other)
    {
        contactPoint = other.ClosestPoint(transform.position);
        centerPoint = other.bounds.center;

        right = contactPoint.x > centerPoint.x;
        top = contactPoint.y > centerPoint.y; 

        contactPointIsFound = true;         
    }
    public void FindContactPoint(Vector2 hero)
    {
        right = hero.x < transform.position.x;
        top = hero.y < transform.position.y; 

        contactPointIsFound = true;         
    }

    public void DoPass()
    {
        if (contactPointIsFound == false) return;

            if (passType == PassType.vertical)
            {
                if (top)
                {                    
                    //finalPointTransform.position = new Vector3(contactPoint.x, contactPoint.y + boxSize.size.y, 0f); 
                    direction = new Vector2 (0, 1f);
                }
                else
                    //finalPointTransform.position = new Vector3(contactPoint.x, contactPoint.y - boxSize.size.y, 0f); 
                    direction = new Vector2 (0, -1f);
            }
            else
            {
                if (right)
                {                    
                    //finalPointTransform.position = new Vector3(contactPoint.x + boxSize.size.x, contactPoint.y, 0f); 
                    direction = new Vector2 (1f, 0);
                }
                else
                    //finalPointTransform.position = new Vector3(contactPoint.x - boxSize.size.x, contactPoint.y, 0f); 
                    direction = new Vector2 (-1f, 0);
            }
               
            MainVariables.inMovement = true; 
            MainVariables.isPassing = true;  
            MainVariables.allowMovement = false;
    }

    private void FixedUpdate() 
    {
        if (MainVariables.isPassing && direction != Vector2.zero)
        {         
            rigidBody.velocity = direction * speed * Time.fixedDeltaTime;
        }  
    }

}
