using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public bool enable, waitAfterAttack;
    //public float damage, radius = 1f, speedAttack = 1f;
    //private CircleCollider2D circleCollider;
    public List<GameObject> propEnemy, propWalls, propTrees;
    private Animator animator; //AnimationClip attackClip; 
    private SpriteRenderer spriteRenderer;
    private Transform heroTransform; Camera cam;
    private Transform weapontransform; Animator weaponAnimator; public Item weapon;
    private void Start() 
    {
        //circleCollider = gameObject.GetComponent<CircleCollider2D>(); 
        propEnemy = new List<GameObject>(); 
        propWalls = new List<GameObject>(); 
        propTrees = new List<GameObject>();
        
        animator = GetComponent<Animator>();
        //    attackClip = FindAnimation(animator, "MelleAtackRight");

        weapontransform = transform.Find("Weapon");
        if (weapontransform != null)
            weaponAnimator = weapontransform.GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        heroTransform = transform.parent;
        UpdateRadius();
    }
    private void Update() 
    {
        if (MainVariables.canMeleeAttack && MainVariables.allowAttack && MousePosition2D.supposedInteractionObject == null && waitAfterAttack == false)
        {         
            //UpdateRadius(); //убрать потом   
            transform.localPosition = ((Vector2)(cam.ScreenToWorldPoint(Input.mousePosition) - heroTransform.position)).normalized;// * 0.5f;            

            if (Input.GetMouseButtonDown(0) && UIClick.OnMouseDown())
            {
                if (MainVariables.inSpelling == false || propEnemy.Count > 0 || propWalls.Count > 0)
                {
                    float angle = MyMathCalculations.CalculateAngle2D(heroTransform.position, cam.ScreenToWorldPoint(Input.mousePosition), heroTransform.right) * (-1);
                    transform.localRotation = Quaternion.Euler(0f,0f,angle);

                    MainVariables.inMovement = false;
                    if (transform.localPosition.x > 0)
                    {
                        animator.Play("MelleAtackRight");
                        weaponAnimator.Play("MeleeWeaponAttackRight");
                    }
                    else
                    {
                        animator.Play("MelleAtackLeft");
                        weaponAnimator.Play("MeleeWeaponAttackLeft");
                    }
                    StartCoroutine(WaitAfterAttackCoroutine());

                    foreach (var enemy in propEnemy)
                        AttackEnemy(enemy);

                    BrokeBreakableObjects(); 
                }   
            }
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

    private void AttackEnemy(GameObject enemy)
    {            
        var enemyInteraction = enemy.GetComponent<EnemyInteraction>();
        if (enemyInteraction != null)
        {    
            enemyInteraction.TakeDamage(weapon.damage, heroTransform.position);
        }
    }

    public void UpdateRadius()
    {
        transform.localScale = new Vector2(1.2f, 1.2f) * weapon.radiusAttack;
    }

    public void UpdateWeaponSprite(Sprite weapon)
    {
        weapontransform.GetComponent<SpriteRenderer>().sprite = weapon;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Enemy")
        {
            propEnemy.Add(other.gameObject);
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
        if (other.gameObject.tag == "Enemy")
        {
            propEnemy.Remove(other.gameObject);
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

    public void ClearPropEnemy()
    {
        propEnemy.Clear();
    }

    /*private IEnumerator PlayAnimationCoroutine()
    {        
        isAttacking = true;
        yield return new WaitForSeconds(attackClip.length);
        isAttacking = false; 
    }*/
    /*public AnimationClip FindAnimation (Animator _animator, string name) 
    {
        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }*/
    public IEnumerator WaitAfterAttackCoroutine()
    {
        waitAfterAttack = true;
        yield return new WaitForSeconds(weapon.speedAttack);
        waitAfterAttack = false;   
    }
}
