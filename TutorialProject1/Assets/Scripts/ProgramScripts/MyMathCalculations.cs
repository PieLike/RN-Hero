using UnityEngine;
using System;

static class MyMathCalculations
{

    /*static MyMathCalculations() { } */

    public static Vector2 CalculateDirectionSpeeds(Vector2 MoovingObjectPosition, Vector2 finalPoint)   //, float speed
    {
        float directionX, directionY;
        float distanceX, distanceY;
        float speedX, speedY;

        if (finalPoint.x > MoovingObjectPosition.x)
            directionX = 1;
        else if (finalPoint.x < MoovingObjectPosition.x)
            directionX = (-1);
        else
            directionX = MoovingObjectPosition.x;
        

        if (finalPoint.y > MoovingObjectPosition.y)
            directionY = 1;
        else if (finalPoint.y < MoovingObjectPosition.y)
            directionY = (-1);
        else
            directionY = MoovingObjectPosition.y;

        distanceX = Math.Abs(MoovingObjectPosition.x - finalPoint.x);
        distanceY = Math.Abs(MoovingObjectPosition.y - finalPoint.y);

        speedX = distanceX / ((distanceX + distanceY));
        speedY = distanceY / ((distanceX + distanceY)); //speedZ = distanceZ / ((distanceX + distanceZ)/10) * speed * 0.1f;

        //Debug.Log(directionX + "*" + speedX + "," + directionZ + "*" + speedZ);
        
        return new Vector2(directionX*speedX, directionY*speedY);
    } 

    public static bool CheckReachToPoint(Vector2 PointA, Vector2 PointB)
    {
        
        if (Math.Round(PointA.x) == Math.Round(PointB.x)
            && Math.Round(PointA.y) == Math.Round(PointB.y))
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

    public static float CalculateAngle2D(Vector2 pointFrom, Vector2 pointTo, Vector2 vectorOne)
    {
        Vector2 vectorTwo = pointTo - pointFrom; // direction

        float angle = Vector2.SignedAngle(vectorTwo, vectorOne);// * (-1);

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
