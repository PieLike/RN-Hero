using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDistance : MonoBehaviour
{
    Transform  enemyTr, heroTr;
    BoxCollider2D distanceBox; List<GameObject> walls; public bool blocked, haveHero;
    float radiusAttack = 8f;
    private void Start() 
    {
        distanceBox = GetComponent<BoxCollider2D>();
        walls = new List<GameObject>();
        enemyTr = transform.parent.transform;
        heroTr = FindObjectOfType<HeroMove>().transform;
    }
    

    private void FixedUpdate() 
    {
        Vector2 enemyPosition = enemyTr.position, heroPosition = heroTr.position;        

        float distance = Vector2.Distance (enemyPosition, heroPosition);
        distanceBox.size = new Vector2(distanceBox.size.x, (distance < radiusAttack ? distance : radiusAttack));

        distanceBox.transform.position = Vector2.Lerp(enemyPosition, heroPosition, (distance < radiusAttack ? 0.5f : ((radiusAttack/2)/distance)));

        float angle = MyMathCalculations.CalculateAngle2D(enemyPosition, heroPosition, enemyTr.up) * (-1); 
        distanceBox.transform.rotation = Quaternion.Euler(0f,0f,angle);

        if (walls.Count > 0)
            blocked = true;
        else
            blocked = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.tag == "Wall")
        {
            walls.Add(other.gameObject);
        }
        if (other.tag == "Hero")
        {
            haveHero = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Wall")
        {
            walls.Remove(other.gameObject);
        }
        if (other.tag == "Hero")
        {
            haveHero = false;
        }
    }
}
