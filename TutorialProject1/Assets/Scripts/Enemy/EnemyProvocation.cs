using System.Collections.Generic;
using UnityEngine;

public class EnemyProvocation : MonoBehaviour
{
    EnemyAI enemyAI; HeroDistance heroDistance;
    private void Start() 
    {
        Transform enemy = transform.parent;
        if (enemy != null)
        {
            enemyAI = enemy.GetComponent<EnemyAI>();
        }
        else
            Debug.Log("enemy = null");    

        Transform heroDistanceObj = enemy.Find("HeroDistance");
        heroDistance = heroDistanceObj.GetComponent<HeroDistance>();      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.tag == "Hero")
        {
            if (heroDistance.blocked == false)
            {
                enemyAI.ChaseHero();  
                enemyAI.enemyData.data.isAgressive = true;
            }
        }
    }
}
