using UnityEngine;
using System;
using System.Collections;

public class HeroMove : MonoBehaviour
{
    [NonSerialized] public static bool isReached = false;
    public float speed = 500f;   
    private Rigidbody2D rigidBody;
    private GameObject objFinalPoint;
    private bool isRotate;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Start() 
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        objFinalPoint = GameObject.Find("FinalPoint2D");
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() 
    {
        if (MainVariables.forceNewMovementLockWhenMove == false)
        { 
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                rigidBody.velocity = direction * speed * Time.fixedDeltaTime;

                if(Input.GetAxis("Horizontal") > 0 && (isRotate == false))
                {
                    //transform.Rotate(0,180f,0);
                    sprite.flipX = true;
                    isRotate = true;
                }
                else if(Input.GetAxis("Horizontal") < 0 && isRotate == true)
                {
                    //transform.Rotate(0,-180f,0); 
                    sprite.flipX = false; 
                    isRotate = false;  
                }    
                
                if (animator.GetBool("Walking") == false)
                    animator.SetBool("Walking", true);
                if (objFinalPoint!= null && objFinalPoint.activeSelf == true) 
                    objFinalPoint.SetActive(false);
                if (MainVariables.inMovement == true)
                    MainVariables.inMovement = false;
            }
            else if(MainVariables.inMovement == false && MainVariables.inImpacting == false)
            {
                DisableMoving();    
            }
        }
        //если герой в движении
        if (MainVariables.inMovement)
        { 
            //FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)
            //проверяем добрался ли до точки, если да то заканчиваем статус "движение"
            //иначе двигаем героя в точке
            if (objFinalPoint != null)
            {
                if (MyMathCalculations.CheckReachToPoint(transform.position, objFinalPoint.transform.position) == false && (objFinalPoint.activeSelf == true))
                {
                    Vector2 direction = MyMathCalculations.CalculateDirectionSpeeds(transform.position, objFinalPoint.transform.position); 
                    rigidBody.velocity = direction * Time.fixedDeltaTime * speed; //transform.TransformDirection(
                }
                else
                    //до сюда скорее всего не дойдет тк выклчютися раньше но на всякий случай
                    DisableMoving();             
            }
        }
        if (isReached == true)
            isReached = false; 
    }

    private void DisableMoving()
    {
        if (animator.GetBool("Walking") == true)
            animator.SetBool("Walking", false);
        rigidBody.velocity = Vector2.zero;
        rigidBody.Sleep();
        if (MainVariables.inMovement == true)
            MainVariables.inMovement = false;
        if (MainVariables.inInteraction == false && objFinalPoint != null && objFinalPoint.activeSelf == true) 
            objFinalPoint.SetActive(false);
    }

    public void LateUpdate() 
    {    
        
        //при клике мыши Не на интерфейс, не во время заклинания, не во время интерфейса и не на объект
        //задаём точку движения "к чему" и включаем движение
        if (Input.GetMouseButton(0) && UIClick.OnMouseDown() && MainVariables.allowMovement == true && Interaction.supposedInteractionObject == null)
        {
            //FindFinalPoint();   //на всякий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)                        
            if (objFinalPoint != null)
            {
                if (objFinalPoint.activeSelf == true)
                {
                    if (animator.GetBool("Walking") == false)
                        animator.SetBool("Walking", true);
                    RotateObject();
                    MainVariables.inMovement = true;    
                }
            }
        } 
        


        if (Input.GetMouseButtonDown(1) && UIClick.OnMouseDown())
        {
            //Debug.Log("ПКМ");
        }
        if (Input.GetMouseButtonDown(2) && UIClick.OnMouseDown())
        {
            //Debug.Log("СКМ");
        }    
        
    }  

    private void OnTriggerStay2D(Collider2D other) 
    {
        //FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)
        if (objFinalPoint != null)
        {
            if (other.gameObject == objFinalPoint)
            {
                isReached = true;
                MainVariables.forceNewMovementLockWhenMove = false;

                if (MainVariables.inMovement == true)
                {
                    DisableMoving();    
                }   
            }
        }     
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Wall" && MainVariables.inMovement == true)
        {
            MainVariables.inMovement = false; 
        }    
    }

    public void TakeImpact(float impact, Vector2 impactPosition)
    {
        MainVariables.inMovement = false;
        MainVariables.inImpacting = true;

        rigidBody.velocity -= new Vector2(impactPosition.x - gameObject.transform.position.x, impactPosition.y - gameObject.transform.position.y) * impact /10;
        
        StopCoroutine(ImpactStopCoroutine(impact /10));
        StartCoroutine(ImpactStopCoroutine(impact /10));
    }
    private IEnumerator ImpactStopCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        MainVariables.inImpacting = false;  
    }

    private void RotateObject()
    {
        if(objFinalPoint.transform.position.x > transform.position.x && (isRotate == false))
        {            
            //transform.Rotate(0,180f,0);
            sprite.flipX = true;
            isRotate = true;
        }
        else if(objFinalPoint.transform.position.x < transform.position.x && isRotate == true)
        {
            //transform.Rotate(0,-180f,0);  
            sprite.flipX = false;
            isRotate = false;  
        }    
    }
}
