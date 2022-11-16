using System.Collections;
using UnityEngine;

public class PigDistanceScript : EnemyBehavior
{
    Transform heroTransform; HeroHealth heroHealth; HeroMove heroMove;
    HeroDistance heroDistance;
    
    Animator enemyAnimator;
    public ScriptableObjSpell activeSpell; GameObject projectile;
    public override void Start() 
    {
        base.Start();

        enemyAnimator = GetComponent<Animator>();

        Transform heroDistanceObj = transform.Find("HeroDistance");
        heroDistance = heroDistanceObj.GetComponent<HeroDistance>(); 

        heroTransform = FindObjectOfType<HeroMove>().transform;       
    }

    public override void Update() 
    {
        base.Update();
        
        if (enemyAI.allowToMove && enemyData.data.isAgressive)
        {
            if(heroDistance.blocked || heroDistance.haveHero == false)
            {
                if (enemyAI.targetObject != EnemyAI.TargetObject.hero || enemyData.inMovement == false)
                    enemyAI.ChaseHero();
            }
            else
            {
                enemyAI.Stop();
                if (waitAfterAttack == false)
                {
                    DistanceAttack();
                }
            }
        }
    }

    private void DistanceAttack()
    {
        if (activeSpell != null && heroTransform != null)
        {
            if (projectile == null) projectile = Resources.Load<GameObject>("Spells/" + activeSpell.name.ToString() + "/" + activeSpell.name.ToString());
            if (projectile != null)
            {
                StartCoroutine(WaitAfterAttackCoroutine());                
            }
        }
    }

    public IEnumerator WaitAfterAttackCoroutine()
    {        
        enemyData.isAttacking = true;
        waitAfterAttack = true;

        yield return new WaitForSeconds(0.5f);

        enemyAnimator.SetBool("Attacking", true);

        float angle = MyMathCalculations.CalculateAngle2D(transform.position, heroTransform.position, transform.up);
        GameObject newSpellObject = Instantiate(projectile, transform.position, transform.rotation * Quaternion.Euler(0f,0f,angle*(-1)));//Quaternion.Euler(Math.Abs(angle),angle,-90)

        SpellBehavior newSpellBehavior = newSpellObject.GetComponent<SpellBehavior>(); 
        Vector2 startPosition = new Vector2(heroTransform.position.x - gameObject.transform.position.x, heroTransform.position.y - gameObject.transform.position.y);
        newSpellBehavior.NonPhysicProjectile(activeSpell, true, startPosition); 

        //if(activeSpell.selfImpact > 0)
        //    gameObject.GetComponent<HeroMove>().TakeImpact(activeSpell.selfImpact, heroTransform.position);

        yield return new WaitForSeconds(enemyData.data.attackSpeed + 0.5f);

        enemyAnimator.SetBool("Attacking", false);
        enemyData.isAttacking = false;
        waitAfterAttack = false;          
    }
}
