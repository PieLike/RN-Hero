using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(EnemyData))]
public class EnemyBehavior : MonoBehaviour
{
    //[NonSerialized]public Vector2 finalPoint;
    [NonSerialized] public bool isDead, inImpacting, waitAfterAttack;//beenCollide, 
    [NonSerialized] public GameObject shield;     
    private Rigidbody2D rigidBody; Sound walkingSound;  //AudioManager audioManager; 
    [NonSerialized] public EnemyData enemyData;  public EnemyAI enemyAI;
    [NonSerialized] public bool lootDropIsDone;

    public virtual void Start() 
    {
        enemyData = gameObject.GetComponent<EnemyData>();
        enemyAI = gameObject.GetComponent<EnemyAI>();
        //находим дочерние объекты
        shield = transform.Find("Shield").gameObject;           

        rigidBody = GetComponent<Rigidbody2D>(); 

        //audioManager = FindObjectOfType<AudioManager>();  

        foreach (Sound sound in enemyData.data.sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }    
    
    public virtual void Update()
    {
        if (isDead == false)
        {
            if (enemyData.currentHP <= 0.0f)
            {
                //вызываем функцию дропа лута у активного объекта (врага)
                CallLootDrop();                
                //делаем его компоненты неактивными (потом нужно будет исправить чтобы полностью удалять)
                isDead = true;
            }            
        } 
        else
        {
            if (lootDropIsDone)
            {
                Destroy(gameObject);  Debug.Log("destroy " + gameObject.name); 
                EnemyDeadRegistration();
                Experience.AddExp(MainVariables.expForEnemyKill);
            }
        }   

    if  (enemyData.inMovement == true && (walkingSound == null || walkingSound.playing == false))
        {            
            walkingSound = Play("walking");//audioManager.Play(enemyData.data.walkingSound); 
        }
        else if (walkingSound != null && enemyData.inMovement == false && walkingSound.playing == true)
        {
            Stop(walkingSound);
        }         
    }  

    private void FixedUpdate() 
    {
        //если движение прекращено
        if (inImpacting == true)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.Sleep();
            
            //if (finalPoint != rigidBody.position)
            //    finalPoint = rigidBody.position;
            //if (beenCollide != false)
            //    beenCollide = false;
        }    
    }    

    public virtual void EnemyDeadRegistration()
    {
        //зарегестрировать смерть врага в ивент менеджере
        /*string enemyName;
        if (gameObject.name.Contains('('))
            enemyName = gameObject.name.Split('(')[0];
        else
            enemyName = gameObject.name;

        enemyName = enemyName.Replace(" ", "").ToLower();*/

        QuestEvents.KilledEnemy killedEnemy = new QuestEvents.KilledEnemy { enemy = enemyData.data, spawnPoint = transform.parent};
        QuestEvents.SendEnemyKilled(killedEnemy);
    }

    private void CallLootDrop()
    {
        //вызвать выпадение лута у активного объекта
        LootDroping lootDroping = gameObject.GetComponent<LootDroping>();
        if (lootDroping != null)
            lootDroping.Drop();
    }

    public IEnumerator WaitAfterAttackCoroutine(float duration)
    {
        waitAfterAttack = true;
        yield return new WaitForSeconds(duration);
        waitAfterAttack = false;   
    }

    public virtual void OnCollisionStay2D(Collision2D other) 
    {
        //необходим для override
        //для краба например
    }

    public Sound Play (string soundName)
    {
        //Debug.Log("Play");
        Sound sound = Array.Find(enemyData.data.sounds, sound => sound.name == soundName);
        if (sound != null)
        {
            sound.source.Play();
            StartCoroutine(PlayAudioClip(sound));
            return sound;
        }
        else
        {
            Debug.Log("не найден аудиоклип в объекте " + gameObject.name + ": " + soundName);
            return null;
        }
    }
    public void Stop (Sound _sound)
    {
        _sound.source.Stop();
    }

    private IEnumerator PlayAudioClip(Sound _sound)
    {        
        _sound.playing = true;
        yield return new WaitForSeconds(_sound.clip.length);
        _sound.playing = false; 
    }
    /*public AudioClip FindAnimation (Animator _animator, string name) 
    {
        foreach (AudioClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }*/
    
    /*private IEnumerator DeadCoroutine()
    {
        float duration = 5.0f;
        yield return new WaitForSeconds(duration); 
        Destroy(gameObject);       
    }*/

    /*private void OnCollisionEnter2D(Collision2D other) 
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
    }*/
}
