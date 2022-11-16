using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [NonSerialized] public EnemyBehavior enemyBehavior; [NonSerialized] public EnemyAI enemyAI;
    [NonSerialized] public List<GameObject> propHero, propWalls, propTrees;
    private Animator animator; //AnimationClip attackClip; 
    private SpriteRenderer spriteRenderer;
    private Transform enemyTransform; Camera cam; 
    [NonSerialized] public Transform heroTransform; HeroHealth heroHealth; HeroMove heroMove;
    private Animator enemyAnimator; 
    private void Start() 
    {
        propHero = new List<GameObject>(); 
        propWalls = new List<GameObject>(); 
        propTrees = new List<GameObject>();
        
        animator = GetComponent<Animator>();

        enemyTransform = transform.parent;

        enemyAnimator = enemyTransform.GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        enemyAI = enemyTransform.GetComponent<EnemyAI>();
        
        UpdateRadius();                
    }
    private void Update() 
    {
        if (enemyBehavior != null && enemyBehavior.waitAfterAttack == false && heroTransform != null)
        {       
            transform.localPosition = ((Vector2)(heroTransform.position - enemyTransform.position)).normalized * 0.5f;
        }
    }
    public void Attack()
    {
        if (heroTransform != null)
        {
            float angle = MyMathCalculations.CalculateAngle2D(enemyTransform.position, heroTransform.position, enemyTransform.right) * (-1);
            transform.localRotation = Quaternion.Euler(0f,0f,angle);              

            StartCoroutine(WaitAfterAttackCoroutine());             
        }
    }

    private void BrokeBreakableObjects()
    {
        List<Breakable> objectsForBroke = new List<Breakable>();
        foreach (var wall in propWalls)
        {
            Breakable breakable = wall.GetComponent<Breakable>();
            if (breakable != null && breakable.enabled)
            {
                objectsForBroke.Add(breakable);                        
            }
        }
        foreach (var objectForBroke in objectsForBroke)
        {
            objectForBroke.DoBroke();
        }
    }

    private void AttackEnemy(GameObject hero)
    {            
        if (heroHealth == null)
            heroHealth = hero.GetComponent<HeroHealth>();

        if (heroMove == null)
            heroMove = hero.GetComponent<HeroMove>();

        if (heroHealth != null && heroMove != null)
        {
            heroHealth.TakeDamage(enemyBehavior.enemyData.data.damage);
            heroMove.TakeImpact(enemyBehavior.enemyData.data.damageImpact, transform.position);
        }
    }
    /*private IEnumerator WaitAfterAttackCoroutine()
    {
        

        StartCoroutine(ActualAttackCoroutine());

        yield return new WaitForSeconds(enemyBehavior.enemyData.data.attackSpeed + 1);

        enemyAnimator.SetBool("Attacking", false);
        enemyBehavior.enemyData.isAttacking = false;
        enemyAI.ChaseHero();
        enemyBehavior.waitAfterAttack = false;          
    }*/

    private IEnumerator WaitAfterAttackCoroutine()
    {
        enemyAI.Stop();
        enemyBehavior.enemyData.isAttacking = true;
        enemyBehavior.waitAfterAttack = true;

        yield return new WaitForSeconds(0.5f);

        if (transform.localPosition.x > 0)
        {
            animator.Play("MelleAtackRight");
        }
        else
        {
            animator.Play("MelleAtackLeft");
        }
        enemyAnimator.SetBool("Attacking", true);

        foreach (var enemy in propHero)
            AttackEnemy(enemy);

        BrokeBreakableObjects();

        yield return new WaitForSeconds(enemyBehavior.enemyData.data.attackSpeed + 0.5f);

        enemyAnimator.SetBool("Attacking", false);
        enemyBehavior.enemyData.isAttacking = false;
        enemyAI.ChaseHero();
        enemyBehavior.waitAfterAttack = false; 
    }

    public void UpdateRadius()
    {
        transform.position.Scale(new Vector2(1.2f, 1.2f) * 1);    //weapon.radiusAttack
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Hero")
        {
            propHero.Add(other.gameObject);
            heroTransform = other.transform;
        }
        else if(other.gameObject.tag == "Wall")
        {
            propWalls.Add(other.gameObject);
        }
        else if(other.gameObject.tag == "Tree")
        {
            propTrees.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Hero")
        {
            propHero.Remove(other.gameObject);
        }
        else if(other.gameObject.tag == "Wall")
        {
            propWalls.Remove(other.gameObject);
        }
        else if(other.gameObject.tag == "Tree")
        {
            propTrees.Remove(other.gameObject);
        }
    }

    public void ClearPropHero()
    {
        propHero.Clear();
    }
}
