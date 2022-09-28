using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EnemyData))]
public class EnemyBehavior : MonoBehaviour
{
    public Vector2 finalPoint;
    [NonSerialized] public bool inMovement, isDead, inWaiting, beenCollide, inImpacting;
    public float speed = 50f;
    [NonSerialized] public GameObject shield;            
    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool isRotate;    
    private EnemyData enemyData;    
    [NonSerialized] public bool lootDropIsDone;

    private void Start() 
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        //находим дочерние объекты
        shield = transform.Find("Shield").gameObject;           

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();  
    }    

    private void MoveTo(GameObject objectToMoveTo)
    {
        finalPoint = objectToMoveTo.transform.position;

        inMovement = true;
        animator.SetBool("Walking", true);
    }

    private void MoveRandom()
    {
        float directionX = UnityEngine.Random.Range(-2.0f, 2.0f);
        float directionY = UnityEngine.Random.Range(-0.5f, 0.5f);

        finalPoint = new Vector2 (transform.position.x + directionX, transform.position.y + directionY); 

        RotateObject(); //поворачивем если необходимо

        inMovement = true;
        animator.SetBool("Walking", true);
    }

    private void RotateObject()
    {
        if(finalPoint.x > transform.position.x)
        {
            if (isRotate == false)
            {
                transform.Rotate(0,180f,0);
                isRotate = true;
            }
        }
        else if(isRotate == true)
            {
                transform.Rotate(0,-180f,0);  
                isRotate = false;  
            }    
    }

    private void WaitRandom()
    {
        inWaiting = true;
        float duration = UnityEngine.Random.Range(1.0f,10.0f);
        StartCoroutine(WaitCoroutine(duration));
    }
    
    void Update()
    {
        if (isDead == false)
        {
            if (enemyData.currentHP <= 0.0f)
            {
                //вызываем функцию дропа лута у активного объекта (врага)
                CallLootDrop();                
                //делаем его компоненты неактивными (потом нужно будет исправить чтобы полностью удалять)
                CallDead();
            }            
        } 
        else
        {
            if (lootDropIsDone)
                Destroy(gameObject);   
        }             
    }  

    private void FixedUpdate() 
    {
        //если движение прекращено
        if (inMovement == false && inImpacting == false)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.Sleep();

            if (animator.GetBool("Walking") == true)
                animator.SetBool("Walking", false);
            if (finalPoint != rigidBody.position)
                finalPoint = rigidBody.position;
            if (beenCollide != false)
                beenCollide = false;
        }
        if (isDead == false && inWaiting == false)
        {
            if (inMovement && finalPoint != null)
            {
                if (MyMathCalculations.CheckReachToPoint(rigidBody.position, finalPoint))
                {
                    inMovement = false;                    
                }
                else
                {
                    Vector2 direction = MyMathCalculations.CalculateDirectionSpeeds(rigidBody.position, finalPoint);
                    if (isRotate == false)
                    {
                        rigidBody.velocity = direction * Time.fixedDeltaTime * speed;
                    }
                    else
                    {
                        rigidBody.velocity = new Vector2(direction.x, direction.y) * Time.fixedDeltaTime * speed;
                    }                               
                }
            }
            else
            {
                //придаём случайное движение если стоит
                //if (Input.GetKeyUp(KeyCode.H))  
                System.Random randomThing = new System.Random();              
                switch(randomThing.Next(2))
                {
                    case(0): 
                        //Debug.Log("going move");
                        MoveRandom(); break;
                    case(1):
                        //Debug.Log("going wait");
                        WaitRandom(); break;
                }                
            }
        }        
    }
    public void CallDead()
    {
        isDead = true; inMovement = true;
        
        /*CircleCollider2D circleCollider2D  = gameObject.GetComponent<CircleCollider2D>();
        if (circleCollider2D) circleCollider2D.enabled = false;

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.enabled = false;
        
        StartCoroutine(DeadCoroutine());*/

        //зарегестрировать смерть врага в ивент менеджере
        EnemyDeadRegistration();
    }

    private void EnemyDeadRegistration()
    {
        //зарегестрировать смерть врага в ивент менеджере
        string enemyName;
        if (gameObject.name.Contains('('))
            enemyName = gameObject.name.Split('(')[0];
        else
            enemyName = gameObject.name;

        enemyName = enemyName.Replace(" ", "").ToLower();

        QuestEvents.SendEnemyKilled(enemyName);
    }

    private void CallLootDrop()
    {
        //вызвать выпадение лута у активного объекта
        LootDroping lootDroping = gameObject.GetComponent<LootDroping>();
        lootDroping.Drop();
    }
    
    private IEnumerator DeadCoroutine()
    {
        float duration = 5.0f;
        yield return new WaitForSeconds(duration); 
        Destroy(gameObject);       
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag != "Floor")// && other.gameObject.tag != "Hero")
        {
            if (beenCollide == false)
            {
                finalPoint = new Vector2(transform.position.x*2 - finalPoint.x, transform.position.y*2 - finalPoint.y);
                RotateObject(); //поворачивем если необходимо 
                beenCollide = true;
            }
            else
            {
                if (inMovement == true)
                    inMovement = false; 
            }  
        }
    }

    private IEnumerator WaitCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        inWaiting = false;   
    }

}
