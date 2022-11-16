using System;
using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    public bool inWaiting;
    public enum TargetObject { hero, finalPoint, none }; public TargetObject targetObject = TargetObject.none; public bool infiniteChase;
    public bool allowToMove;
    private Transform hero; public EnemyData enemyData;    
    public float nextWaypointDistance = 0.5f;
    private Path path; private int currentWaypoint; //private bool reachedEndOfPath;
    private Seeker seeker; [NonSerialized] public Rigidbody2D rb;
    private Vector2 finalPoint, prevFP;

    private void Awake() 
    {
        InvokeRepeating("UpdatePath", 0f, 0.3f);

        if (enemyData == null) enemyData = gameObject.GetComponent<EnemyData>();
        if (enemyData.data.isAgressive)
            ChaseHero();
    }
    public void Start()
    {
        if (seeker != null) return;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();        

        if (enemyData == null) enemyData = gameObject.GetComponent<EnemyData>();
            
        hero = GameObject.Find("Hero").transform;

        finalPoint  = transform.position;
        prevFP      = transform.position;           
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
        rb.Sleep();

        //reachedEndOfPath = true;
        path = null;
        
        targetObject = TargetObject.none;
        enemyData.inMovement = false;
    }

    private void UpdatePath()
    {
        if (seeker != null && seeker.IsDone() && inWaiting == false && allowToMove)
        {
            switch (targetObject)
            {
                case TargetObject.none:
                    break;
                case TargetObject.hero:
                    seeker.StartPath(rb.position, new Vector2(hero.position.x, hero.position.y -0.25f), OnPathComplete);
                    break;
                case TargetObject.finalPoint:
                    if (prevFP != finalPoint)
                    {
                        seeker.StartPath(rb.position, finalPoint, OnPathComplete);
                        prevFP = finalPoint;
                    }
                    break;
                default:
                    break;
            }            
        }            
    }

    private void OnPathComplete(Path _path)
    {
        if (!_path.error)
        {
            path = _path;
            currentWaypoint = 0;
        }
    }    

    private void FixedUpdate()
    {
        if (allowToMove)
        {
            if (enemyData.inMovement == false && inWaiting == false && targetObject == TargetObject.none && enemyData.data.isAgressive == false)
            {
                //придаём случайное движение если стоит
                System.Random randomThing = new System.Random();              
                switch(randomThing.Next(2))
                {
                    case(0): 
                        MoveRandom(); break;
                    case(1):
                        WaitRandom(); break;
                }
            }

            if (path != null && enemyData.inMovement == true)
            {
                if (currentWaypoint >= path.vectorPath.Count)
                {
                    rb.velocity = Vector2.zero;
                    rb.Sleep();

                    //reachedEndOfPath = true;
                    path = null;
                    
                    if (infiniteChase == false)
                    {
                        targetObject = TargetObject.none;
                        enemyData.inMovement = false;
                    }
                    
                    return;
                }
                else
                {        
                    Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;  //Debug.Log("direction = " + direction.ToString());
                    Vector2 force = direction * enemyData.data.speed * Time.fixedDeltaTime; //Debug.Log("force = " + force.ToString());

                    rb.AddForce(force);
                    
                    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                    if (distance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }  
                }
            }       
        }
        else
        {
            if (rb.velocity != Vector2.zero)
            {
                //Debug.Log("block velocity");
                rb.velocity = Vector2.zero;
                rb.Sleep();                
            }
            if (enemyData.inMovement)
            {
                //path = null;
                targetObject = TargetObject.none;
                enemyData.inMovement = false;
            }
        } 
    }
    public void ChaseHero()
    {
        inWaiting = false;
        targetObject = TargetObject.hero;
        UpdatePath();
        enemyData.inMovement = true;
        infiniteChase = true;
    }

    private void MoveRandom()
    {
        CreateRandomFP();
        if (finalPoint != null)
        {
            targetObject = TargetObject.finalPoint;
            UpdatePath();
            enemyData.inMovement = true;
        }        
    }
    private void CreateRandomFP()
    {
        float directionX = UnityEngine.Random.Range(-1.0f, 1.0f) * 6f;
        float directionY = UnityEngine.Random.Range(-1.0f, 1.0f) * 2f;

        finalPoint = new Vector2 (transform.position.x + directionX, transform.position.y + directionY); 
    }

    private void WaitRandom()
    {
        enemyData.inMovement = false;
        inWaiting = true;
        path = null;
        
        float duration = UnityEngine.Random.Range(1.0f,10.0f);
        StartCoroutine(WaitCoroutine(duration));
    }
    private IEnumerator WaitCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        inWaiting = false;   
    }
}
