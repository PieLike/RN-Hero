using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyData))]
[RequireComponent(typeof(MyOutline))]

public class EnemyView : MonoBehaviour
{
    private MyOutline outline;
    private Texture2D spTexture, emptyTexture, hpTexture;   
    private EnemyData enemyData; 
    private EnemyBehavior enemyBehavior;
    private bool rottenSet;
    private SpriteRenderer spriteRenderer;
    private GameObject prefabRottenEffect; private GameObject objRottenEffect;
    private void Start() 
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyBehavior = GetComponent<EnemyBehavior>();  

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();  

        outline = GetComponent<MyOutline>();  
        prefabRottenEffect = Resources.Load<GameObject>("ParticleSystems/RottenEffect/RottenEffect");

        //находим префабы в resourses
        spTexture = Resources.Load<Texture2D>("Textures/SP_texture");
        hpTexture = Resources.Load<Texture2D>("Textures/HP_texture");
        emptyTexture = Resources.Load<Texture2D>("Textures/empty_texture");           
    }

    private void Update() 
    {
        //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
        if (Interaction.supposedInteractionObject == gameObject)
        {
            //если уже стоит то не будет делать
            outline.SetOutline(MyOutline.Color.red);
        }    
        else
        {
            outline.RemoveOutline();  
        } 

        if (enemyData.isRotten == true && rottenSet == false)
        {
            spriteRenderer.color = new Color(0.72f, 0.05f, 0.85f);
            if (objRottenEffect == null)
                objRottenEffect = Instantiate(prefabRottenEffect, gameObject.transform);
            else
                objRottenEffect.SetActive(true);    
            rottenSet = true; 
        }
        else if (enemyData.isRotten == false && rottenSet == true)
        {
            spriteRenderer.color = Color.white;
            if (objRottenEffect != null)
                objRottenEffect.SetActive(false); 
            rottenSet = false; 
        }
    }

    private void OnGUI() 
    {
        if (enemyData.haveShield)
        {
            if(enemyData.currentSP < enemyData.generalSP && enemyBehavior.isDead == false)
            {
                Vector2 enemyPositionOnScreen = Camera.main.WorldToScreenPoint(transform.position);

                Vector2 min = spriteRenderer.bounds.min;                        Vector2 max = spriteRenderer.bounds.max;
                Vector2 screenMin = Camera.main.WorldToScreenPoint(min);        Vector2 screenMax = Camera.main.WorldToScreenPoint(max);
                float weightObjectOnScreen = screenMax.x - screenMin.x; float heightObjectOnScreen = screenMax.y - screenMin.y;

                GUI.Box(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen, weightObjectOnScreen*1.5f, heightObjectOnScreen/3),"");
                GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, (weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), emptyTexture);
                float currentSPinPercents = enemyData.currentSP/enemyData.generalSP;
                GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, currentSPinPercents*(weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), spTexture);
            }
        }
        else
        {
            if(enemyData.currentHP < enemyData.generalHP && enemyBehavior.isDead == false)
            {      
                Vector2 enemyPositionOnScreen = Camera.main.WorldToScreenPoint(transform.position);

                Vector2 min = spriteRenderer.bounds.min;                        Vector2 max = spriteRenderer.bounds.max;
                Vector2 screenMin = Camera.main.WorldToScreenPoint(min);        Vector2 screenMax = Camera.main.WorldToScreenPoint(max);
                float weightObjectOnScreen = screenMax.x - screenMin.x; float heightObjectOnScreen = screenMax.y - screenMin.y;

                GUI.Box(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen, weightObjectOnScreen*1.5f, heightObjectOnScreen/3),"");
                GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, (weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), emptyTexture);
                float currentHPinPercents = enemyData.currentHP/enemyData.generalHP;
                GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, currentHPinPercents*(weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), hpTexture);
            } 
        }
    }
}
