using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyData))]
[RequireComponent(typeof(Blink))]

public class EnemyInteraction : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private EnemyBehavior enemyBehavior; EnemyData enemyData; EnemyView enemyView;
    private Blink blink; EnemyAI enemyAI;
    private void Start() 
    {
        enemyData = gameObject.GetComponent<EnemyData>();

        rigidBody = GetComponent<Rigidbody2D>(); 
        enemyBehavior = GetComponent<EnemyBehavior>();     

        blink = gameObject.GetComponent<Blink>();

        enemyAI = gameObject.GetComponent<EnemyAI>();

        enemyView = gameObject.GetComponent<EnemyView>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spell")
        {
            SpellBehavior spellBehavior = other.gameObject.GetComponent<SpellBehavior>();
            if(spellBehavior != null && spellBehavior.frendly)
            {
                if(spellBehavior.spell.damage > 0)
                    TakeDamage(spellBehavior.spell.damage, spellBehavior.startPosition * (-1f));

                if(spellBehavior.spell.enemyImpact > 0)
                    TakeImpact(spellBehavior.spell.enemyImpact, spellBehavior.startPosition);
            }
        }
    }

    public void TakeDamage(float damage, Vector2 impactPosition)
    {
        if (enemyData.haveShield)
        {
            if (enemyData.currentSP <= 0)
            {
                //currentHP -= damage;
            }
            else
            {
                enemyData.currentSP -= damage;
                if (enemyData.currentSP > 0)
                    enemyBehavior.shield.GetComponent<Blink>().DoBlink(true);
                else
                {
                    //shield.SetActive(false);
                    //shield.GetComponent<Animator>().SetBool("isDestroying", true);
                    Destroy(enemyBehavior.shield, 5);//anim.clip.length);
                }
            }    
        } 
        else
        {
            enemyData.currentHP -= damage;
            if (enemyData.currentHP > 0)
            {
                if (blink != null)
                    blink.DoBlink(false);
                //enemyView.DoParticleSystemDamage(impactPosition);
            }
            else
            {                
                //Destroy(shield, 5);//anim.clip.length);
                //вызывается в апдейте
            }
        }      
    }

    private void TakeImpact(float impact, Vector2 impactPosition)
    {
        //if (enemyAI != null)
        //    enemyAI.inMovement = false;
        enemyBehavior.inImpacting = true;

        rigidBody.velocity += impactPosition * impact /10;

        StopCoroutine(ImpactStopCoroutine(impact/10));
        StartCoroutine(ImpactStopCoroutine(impact/10));
    }
    private IEnumerator ImpactStopCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        enemyBehavior.inImpacting = false;  
    }
}
