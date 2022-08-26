using UnityEngine;
//using System.Data;
//using System.IO;
//using UnityEngine.EventSystems;
using System;

static class MyMathCalculations
{

    /*static MyMathCalculations() { } */

    public static Vector3 CalculateDirectionSpeeds(Vector3 MoovingObjectPosition, Vector3 finalPoint, float speed)
    {
        float directionX, directionZ;
        float distanceX, distanceZ;
        float speedX, speedZ;

        if (finalPoint.x > MoovingObjectPosition.x)
            directionX = 1;
        else if (finalPoint.x < MoovingObjectPosition.x)
            directionX = (-1);
        else
            directionX = MoovingObjectPosition.x;
        

        if (finalPoint.z > MoovingObjectPosition.z)
            directionZ = 1;
        else if (finalPoint.z < MoovingObjectPosition.z)
            directionZ = (-1);
        else
            directionZ = MoovingObjectPosition.z;

        distanceX = Math.Abs(MoovingObjectPosition.x - finalPoint.x);
        distanceZ = Math.Abs(MoovingObjectPosition.z - finalPoint.z);

        speedX = distanceX / ((distanceX + distanceZ)) * speed;
        speedZ = distanceZ / ((distanceX + distanceZ)) * speed; //speedZ = distanceZ / ((distanceX + distanceZ)/10) * speed * 0.1f;

        //Debug.Log(directionX + "*" + speedX + "," + directionZ + "*" + speedZ);
        
        return new Vector3(directionX*speedX, 0, directionZ*speedZ);
    } 

    public static bool CheckReachToPoint(Vector3 PointA, Vector3 PointB)
    {
        
        if (Math.Round(PointA.x) == Math.Round(PointB.x)
            && Math.Round(PointA.z) == Math.Round(PointB.z))
            return true;
        else
            return false;   
    }

    public static float CalculateAngle(Vector3 pointFrom, Vector3 pointTo, Vector3 vectorOne)
    {
        Vector3 vectorTwo = new Vector3(pointTo.x, pointFrom.y, pointTo.z) - pointFrom; // direction

        float angle = Vector3.SignedAngle(vectorTwo, vectorOne, Vector3.up) * (-1);

        return angle;  
    }

    public static double RoundAbs(float number)
    {
        return Math.Round(Math.Abs(number));
    }

    public static Vector3 ClaculateDirectionPoint(Vector3 pointFrom, Vector3 pointTo)
    {
        Vector3 distance = new Vector3(pointTo.x, pointFrom.y, pointTo.z) - pointFrom;

        Vector3 direction = new Vector3(Math.Abs(distance.x)/(Math.Abs(distance.x) + Math.Abs(distance.z)),0,Math.Abs(distance.z)/(Math.Abs(distance.x) + Math.Abs(distance.z)));

        if (pointTo.x > pointFrom.x)
            direction.x *= 1;
        else if (pointTo.x < pointFrom.x)
            direction.x *= (-1);
        else
            direction.x *= 1;
        

        if (pointTo.z > pointFrom.z)
            direction.z *= 1;
        else if (pointTo.z < pointFrom.z)
            direction.z *= (-1);
        else
            direction.z *= 1;

        return direction;
    }

}
