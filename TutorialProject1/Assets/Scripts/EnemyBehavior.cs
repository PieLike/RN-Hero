using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Outline))]

public class EnemyBehavior : MonoBehaviour
{
    private Vector3 finalPoint;
    private bool inMovement = false, isDead = false, inWaiting = false;
    public float speed = 50f;
    private GameObject shield;
    [NonSerialized] public float currentHP, currentSP; 
    public float generalHP = 1f, generalSP = 5f;
    private Texture2D spTexture, emptyTexture;  
    private Outline outline; 
    private Animator animator;
    private Rigidbody rigidBody;
    private bool isRotate = false;

    private void Start() 
    {
        //находим дочерние объекты
        shield = transform.Find("Shield").gameObject;
        //находим префабы в resourses
        spTexture = Resources.Load<Texture2D>("Textures/SP_texture");
        emptyTexture = Resources.Load<Texture2D>("Textures/empty_texture");

        currentSP = generalSP;
        currentHP = generalHP;  

        outline = GetComponent<Outline>(); 
        outline.OutlineWidth = 0;

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();  
    } 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spell")
        {
            if(other.gameObject.GetComponent<SpellBehavior>().damage > 0)
                TakeDamage(other.gameObject.GetComponent<SpellBehavior>().damage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentSP <= 0)
        {
            //currentHP -= damage;
        }
        else
        {
            currentSP -= damage;
            if (currentSP > 0)
                shield.GetComponent<Blink>().DoBlink();
            else
            {
                //shield.SetActive(false);
                //shield.GetComponent<Animator>().SetBool("isDestroying", true);
                Destroy(shield, 5);//anim.clip.length);
            }
        }           
    }

    private void MoveTo(GameObject objectToMoveTo)
    {
        finalPoint.x = objectToMoveTo.transform.position.x;
        finalPoint.y = transform.position.y;
        finalPoint.z = objectToMoveTo.transform.position.z;

        inMovement = true;
        animator.SetBool("Walking", true);
    }

    private void MoveRandom()
    {
        float directionX = UnityEngine.Random.Range(-2.0f, 2.0f);
        float directionZ = UnityEngine.Random.Range(-0.5f, 0.5f);

        finalPoint.x = transform.position.x + directionX;
        finalPoint.y = transform.position.y;
        finalPoint.z = transform.position.z + directionZ; 

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
            if (currentHP <= 0.0f)
            {
                //вызываем функцию дропа лута у активного объекта (врага)
                CallLootDrop();                
                //делаем его компоненты неактивными (потом нужно будет исправить чтобы полностью удалять)
                CallDead();

            }            
        } 
        //если движение прекращено
        if (inMovement == false)
        {
            if (animator.GetBool("Walking") == true)
                animator.SetBool("Walking", false);
            if (finalPoint != transform.position)
                finalPoint = transform.position;

        }
        //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
        if (Interaction.supposedInteractionObject == gameObject)
        {
            outline.OutlineWidth = 3;
        }    
        else if (outline.OutlineWidth != 0)
            outline.OutlineWidth = 0;        
    }  

    private void FixedUpdate() 
    {
        if (isDead == false && inWaiting == false)
        {
            if (inMovement && finalPoint != null)
            {
                if (MyMathCalculations.CheckReachToPoint(transform.position, finalPoint))
                {
                    inMovement = false;
                    //animator.SetBool("Walking", false);
                    //finalPoint = null;
                }
                else
                {
                    Vector3 direction = MyMathCalculations.CalculateDirectionSpeeds(transform.position, finalPoint, speed);
                    if (isRotate == false)
                    {
                        rigidBody.velocity = transform.TransformDirection(direction * Time.fixedDeltaTime);
                    }
                    else
                    {
                        rigidBody.velocity = transform.TransformDirection(direction * Time.fixedDeltaTime * (-1));
                    }
                    //transform.Translate(direction * Time.deltaTime);                                
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
                    MoveRandom(); break;
                    case(1):
                    WaitRandom(); break;

                }                
            }
        }        
    }
    public void CallDead()
    {
        isDead = true;
        if (gameObject.GetComponent<MeshRenderer>() != null)
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        else
            gameObject.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(DeadCoroutine());
    }

    private void CallLootDrop()
    {
        //вызвать выпадение лута у активного объекта
        LootDroping lootDroping = gameObject.GetComponent<LootDroping>();
        lootDroping.Drop();
    }

    private void OnGUI() 
    {
        if(currentSP < generalSP && isDead == false)
        {
            Camera cam = Camera.main;
            MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();

            Vector2 enemyPositionOnScreen = cam.WorldToScreenPoint(transform.position);

            Vector3 min = mesh.bounds.min;                          Vector3 max = mesh.bounds.max;
            Vector3 screenMin = cam.WorldToScreenPoint(min);        Vector3 screenMax = cam.WorldToScreenPoint(max);
            float weightObjectOnScreen = screenMax.x - screenMin.x; float heightObjectOnScreen = screenMax.y - screenMin.y;

            GUI.Box(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen, weightObjectOnScreen*1.5f, heightObjectOnScreen/3),"");
            GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, (weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), emptyTexture);
            float currentSPinPercents = currentSP/generalSP;
            GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, currentSPinPercents*(weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), spTexture);
        }
    }
    private IEnumerator DeadCoroutine()
    {
        float duration = 5.0f;
        yield return new WaitForSeconds(duration); 
        Destroy(gameObject);       
    }

    private void OnCollisionEnter(Collision other) 
    {
        //inMovement = false; 
        finalPoint = transform.position - finalPoint;
        RotateObject(); //поворачивем если необходимо   
    }

    private IEnumerator WaitCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        inWaiting = false;   
    }

}
