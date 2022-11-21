using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(EnemyData))]
[RequireComponent(typeof(MyOutline))]

public class EnemyView : MonoBehaviour
{
    private MyOutline outline; EnemyAI enemyAI;
    private Texture2D spTexture, emptyTexture, hpTexture;  Camera cam; 
    private EnemyData enemyData; EnemyBehavior enemyBehavior; Rigidbody2D rb;
    private bool rottenSet;
    private SpriteRenderer spriteRenderer;
    private GameObject prefabRottenEffect; private GameObject objRottenEffect;
    private Animator animator;
    private ParticleSystem particleSystemDamage; Transform particleSys;
    private void Start() 
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyBehavior = GetComponent<EnemyBehavior>();  

        enemyAI = gameObject.GetComponent<EnemyAI>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();  

        outline = GetComponent<MyOutline>();  
        prefabRottenEffect = Resources.Load<GameObject>("ParticleSystems/RottenEffect/RottenEffect");

        //находим префабы в resourses
        spTexture = Resources.Load<Texture2D>("Textures/SP_texture");
        hpTexture = Resources.Load<Texture2D>("Textures/HP_texture");
        emptyTexture = Resources.Load<Texture2D>("Textures/empty_texture");   
        cam = Camera.main;

        animator = GetComponent<Animator>();    

        particleSys = transform.Find("Particles");//ParticleSystem
        if (particleSys != null)
        {
            particleSystemDamage = particleSys.GetComponent<ParticleSystem>();  
        }

        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update() 
    {        
        if (spriteRenderer.flipX == false && rb.velocity.x >= 0.01f)
            spriteRenderer.flipX = true;
        else if (spriteRenderer.flipX == true && rb.velocity.x <= -0.01f)
            spriteRenderer.flipX = false;

        if (enemyData.inMovement == true)
        {
            animator.SetBool("Walking", true);
        }
        else
        {       
            animator.SetBool("Walking", false);
        }

        //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
        /*if (MousePosition2D.supposedInteractionObject == gameObject)
        {
            //если уже стоит то не будет делать
            outline.SetOutline(MyOutline.Color.red);
        }    
        else
        {
            outline.RemoveOutline();  
        } */

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
            if(enemyData.currentSP < enemyData.data.sp && enemyBehavior.isDead == false)
            {
                Vector2 enemyPositionOnScreen = cam.WorldToScreenPoint(transform.position);

                Vector2 min = spriteRenderer.bounds.min;                Vector2 max = spriteRenderer.bounds.max;
                Vector2 screenMin = cam.WorldToScreenPoint(min);        Vector2 screenMax = cam.WorldToScreenPoint(max);
                float weightObjectOnScreen = screenMax.x - screenMin.x; float heightObjectOnScreen = screenMax.y - screenMin.y;

                GUI.Box(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen, weightObjectOnScreen*1.5f, heightObjectOnScreen/3),"");
                GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, (weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), emptyTexture);
                if (enemyData.currentSP > 0)
                {
                    float currentSPinPercents = enemyData.currentSP/enemyData.data.sp;
                    GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, currentSPinPercents*(weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), spTexture);
                }
            }
        }
        else
        {
            if(enemyData.currentHP < enemyData.data.hp && enemyBehavior.isDead == false)
            {      
                Vector2 enemyPositionOnScreen = cam.WorldToScreenPoint(transform.position);

                Vector2 min = spriteRenderer.bounds.min;                        Vector2 max = spriteRenderer.bounds.max;
                Vector2 screenMin = cam.WorldToScreenPoint(min);        Vector2 screenMax = cam.WorldToScreenPoint(max);
                float weightObjectOnScreen = screenMax.x - screenMin.x; 
                float heightObjectOnScreen = 25f;//screenMax.y - screenMin.y;

                GUI.Box(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen, weightObjectOnScreen*1.5f, heightObjectOnScreen/3),"");
                GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, (weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), emptyTexture);
                if (enemyData.currentHP > 0)
                {
                    float currentHPinPercents = enemyData.currentHP/enemyData.data.hp;
                    GUI.DrawTexture(new Rect(enemyPositionOnScreen.x-weightObjectOnScreen*0.75f+2, Screen.height-enemyPositionOnScreen.y-heightObjectOnScreen+2, currentHPinPercents*(weightObjectOnScreen*1.5f-4), heightObjectOnScreen/3-4), hpTexture);
                }
            } 
        }
    }

    public void DoParticleSystemDamage()//Vector2 impactPosition)
    {
        if (particleSystemDamage != null)
        {
            particleSys.SetParent(transform.parent);

            //float angle = MyMathCalculations.CalculateAngle2D(impactPosition, cam.ScreenToWorldPoint(Input.mousePosition), transform.right);
            //particleSys.localRotation = Quaternion.Euler(angle,90f,0f);

            //if(!particleSystemDamage.isPlaying) 
            particleSystemDamage.Play();
            Destroy(particleSys.gameObject, 1f);
        }
        else
            Debug.Log("have no particel syctem");
    }
}
