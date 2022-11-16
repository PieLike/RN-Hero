using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabScript : EnemyBehavior
{
    private Vector2 startPosition;
    private GameObject crabArmyPrefab;  Transform spawnPoint;
    HeroHealth heroHealth; HeroMove heroMove;
    public override void Start() 
    {
        base.Start();
        
        startPosition = transform.position;
        spawnPoint = transform.parent;
        
        crabArmyPrefab = Resources.Load<GameObject>("3d_prefabs/Enemies/CrabArmy");
    }

    public override void EnemyDeadRegistration()
    {
        base.EnemyDeadRegistration();
        
        SpawnCrabArmy();
    }

    private void SpawnCrabArmy()
    {
        GameObject crabArmy = Instantiate(crabArmyPrefab, startPosition, Quaternion.identity, spawnPoint);
        EnemyAI enemyAI = crabArmy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.Start();
            enemyAI.ChaseHero();
        }        
    }
    
    public override void OnCollisionStay2D(Collision2D other) 
    {
        base.OnCollisionStay2D(other);

        if (other.gameObject.tag == "Hero" && waitAfterAttack == false)
        {
            if (heroHealth == null)
                heroHealth = other.gameObject.GetComponent<HeroHealth>();

            if (heroMove == null)
                heroMove = other.gameObject.GetComponent<HeroMove>();
                
            if (heroHealth != null && heroMove != null)
            {
                heroHealth.TakeDamage(enemyData.data.damage);
                heroMove.TakeImpact(enemyData.data.damageImpact, transform.position);

                StartCoroutine(WaitAfterAttackCoroutine(enemyData.data.attackSpeed));
            }
        }
    }
    
}
