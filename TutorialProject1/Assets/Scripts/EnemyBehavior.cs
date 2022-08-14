using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBehavior : MonoBehaviour
{
    public Vector3 finalPoint;
    public bool inMovement = false;
    public float speed, speedX, speedZ;
    
    void Start()
    {
        //StartCoroutine(DropCoroutine());
        
    }

    private void moveTo(GameObject objectToMoveTo)
    {
        inMovement = true;

        finalPoint.x = objectToMoveTo.transform.position.x;
        finalPoint.y = transform.position.y;
        finalPoint.z = objectToMoveTo.transform.position.z;

    }

    private void move(Vector3 direction, float seconds)
    {
        

    }
    
    void Update()
    {
        if (inMovement == true)
        { 
            if (CheckReachToPoint(finalPoint) == true)
                inMovement = false;
        }
        
        if (inMovement == true && CheckReachToPoint(finalPoint) == false)
        {
            float directionX, directionZ;

            float distanceX, distanceZ;

            if (finalPoint.x > transform.position.x)
                directionX = 1 * Time.deltaTime;
            else if (finalPoint.x < transform.position.x)
                directionX = (-1) * Time.deltaTime;
            else
                directionX = transform.position.x;
            

            if (finalPoint.z > transform.position.z)
                directionZ = 1 * Time.deltaTime;
            else if (finalPoint.z < transform.position.z)
                directionZ = (-1) * Time.deltaTime;
            else
                directionZ = transform.position.z;

            distanceX = Math.Abs(transform.position.x - finalPoint.x);
            distanceZ = Math.Abs(transform.position.z - finalPoint.z);

            speedX = distanceX / ((distanceX + distanceZ)/10) * speed;
            speedZ = distanceZ / ((distanceX + distanceZ)/10) * speed;    

            transform.Translate(new Vector3(directionX*speedX, directionZ*speedZ, 0));    
        }
        
    }

    public bool CheckReachToPoint(Vector3 Point)
    {        
        if (Math.Round(transform.position.x) == Math.Round(Point.x)
            && Math.Round(transform.position.z) == Math.Round(Point.z))
            return true;
        else
            return false;   
    }    

    private IEnumerator DropCoroutine()
    {
        float dropSpeed = 0.2f;
        yield return new WaitForSeconds(dropSpeed);        
    }
}
