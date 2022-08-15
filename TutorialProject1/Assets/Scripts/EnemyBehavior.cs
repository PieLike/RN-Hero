using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//[RequireComponent(typeof(Outline))]

public class EnemyBehavior : MonoBehaviour
{
    private Vector3 finalPoint;
    private bool inMovement = false, isDead = false;
    public float speed;
    public GameObject shield;
    private float currentHP, currentSP; 
    public float generalHP, generalSP;
    public Texture2D spTexture, emptyTexture;  
    private Outline outline; 

    private void Start() 
    {
        currentSP = generalSP;
        currentHP = generalHP;  

        outline = GetComponent<Outline>(); 
        outline.OutlineWidth = 0; 
    } 
    
    private void OnCollisionEnter(Collision other) 
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
            currentHP -= damage;
        else
        {
            currentSP -= damage;
            if (currentSP > 0)
                shield.GetComponent<Blink>().DoBlink();
            else
            {
                //shield.SetActive(false);
                shield.GetComponent<Animator>().SetBool("isDestroying", true);
                Destroy(shield, 5);//anim.clip.length);
            }
        }           
    }

    private void moveTo(GameObject objectToMoveTo)
    {
        inMovement = true;

        finalPoint.x = objectToMoveTo.transform.position.x;
        finalPoint.y = transform.position.y;
        finalPoint.z = objectToMoveTo.transform.position.z;
    }

    private void move(Vector3 direction, float seconds)    {   }
    
    void Update()
    {
        if (isDead == false){
            if (currentHP <= 0.0f){
                //вызываем функцию дропа лута у активного объекта (врага)
                CallLootDrop();                
                //делаем его компоненты неактивными (потом нужно будет исправить чтобы полностью удалять)
                CallDead();

                isDead = true;                 
            }
            if (inMovement)
            { 
                if (MyMathCalculations.CheckReachToPoint(transform.position, finalPoint))
                    inMovement = false;
                else
                {
                    Vector3 direction = MyMathCalculations.CalculateDirectionSpeeds(transform.position, finalPoint, speed);   
                    transform.Translate(direction * Time.deltaTime);        
                }
            }
        } 
        //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
        if (Interaction.supposedInteractionObject == gameObject)
        {
            outline.OutlineWidth = 3;
        }    
        else if (outline.OutlineWidth != 0)
            outline.OutlineWidth = 0;  
    }  
    private void CallDead()
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
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

}
