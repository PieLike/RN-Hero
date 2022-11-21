using UnityEngine;

public class PigMeleeScript : EnemyBehavior
{    
    GameObject meleeAttackObj; EnemyMeleeAttack meleeAttack;

    public override void Start() 
    {
        base.Start();
        meleeAttackObj = transform.Find("MeleeAttack").gameObject;
        meleeAttack = meleeAttackObj.GetComponent<EnemyMeleeAttack>();
        meleeAttack.enemyBehavior = this;
    }

    public override void Update() 
    {
        base.Update();

        if (enemyAI.allowToMove && enemyData.data.isAgressive && MainVariables.isDead == false)
        {
            if (meleeAttack.propHero.Count > 0 && enemyData.isAttacking == false)
            {
                meleeAttack.Attack();
            }
        }
    }
}
