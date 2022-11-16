using UnityEngine;
using System;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class HeroMove : MonoBehaviour
{
    public enum TargetObject { interactionObject, finalPoint, none }; public TargetObject targetObject = TargetObject.none;
    public bool isReached = false;
    public float nextWaypointDistance = 2f;  
    private Rigidbody2D rigidBody;
    private GameObject objFinalPoint;        
        private Seeker seeker; Path path; Vector2 prevFP, prevIO, prevH; public GameObject interactionObject;
        private int currentWaypoint;// bool reachedEndOfPath;
        //float lastUpdatePath, timeBetweenUpdatePath = 0.05f;

        private Action<GameObject> DoAfterReach; TakeUp takeUp;

    private void Start() 
    {
        rigidBody = GetComponent<Rigidbody2D>();
        objFinalPoint = GameObject.Find("FinalPoint2D");        

        seeker = GetComponent<Seeker>();
        prevFP = transform.position; prevIO = transform.position;

        takeUp = GetComponent<TakeUp>();

        InvokeRepeating("UpdatePath", 0f, 0.3f);
    }

    private void FixedUpdate() 
    {
        if (MainVariables.allowMovement == true)
        { 
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                //Vector2 force = direction * HeroMainData.speed * Time.fixedDeltaTime;
                //rigidBody.AddForce(force);
                rigidBody.velocity = direction * (HeroMainData.plainSpeed + HeroMainData.buffSpeed);

                if (objFinalPoint!= null && objFinalPoint.activeSelf == true) 
                    objFinalPoint.SetActive(false);
                
                MainVariables.inInteraction = false;
                interactionObject = null;
                targetObject = TargetObject.none;
                prevIO = Vector2.zero; 

                MainVariables.inMovement = true;
            }
            else if(targetObject == TargetObject.none && MainVariables.inMovement == true)
            {
                //rigidBody.velocity = Vector2.zero;
                //rigidBody.Sleep();
                MainVariables.inMovement = false;   
            }
        }
        if(targetObject == TargetObject.none && MainVariables.inImpacting == false && MainVariables.inMovement == false && MainVariables.isPassing == false && MainVariables.isDashing == false)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.Sleep();
        }

        if (path != null && MainVariables.inMovement == true && targetObject != TargetObject.none)
        {
            if (currentWaypoint >= path.vectorPath.Count && Input.GetMouseButton(1) == false)
            {               
                //reachedEndOfPath = true;

                Stop();
            }
            else if (currentWaypoint < path.vectorPath.Count)
            {
                //reachedEndOfPath = false;            

                Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rigidBody.position).normalized; //Debug.Log("direction = " + direction.ToString());
                //Vector2 force = direction * HeroMainData.speed * Time.fixedDeltaTime;
                //rigidBody.AddForce(force);
                rigidBody.velocity = direction * (HeroMainData.plainSpeed + HeroMainData.buffSpeed); //Debug.Log("force = " + (direction * (HeroMainData.plainSpeed + HeroMainData.buffSpeed)).ToString());
                
                float distance = Vector2.Distance(rigidBody.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }  
            }
        }
    }
    private void Stop()
    {
        //rigidBody.velocity = Vector2.zero;
        //rigidBody.Sleep();

        if (targetObject == TargetObject.interactionObject)
            isReached = true;

        path = null;
        MainVariables.inMovement = false;
        targetObject = TargetObject.none;
        prevIO = Vector2.zero;

        if (MainVariables.inInteraction == false && objFinalPoint != null && objFinalPoint.activeSelf == true) 
            objFinalPoint.SetActive(false);
    }

    public void Update() 
    {    
        if(MainVariables.inInteraction && isReached)
        {
            MainVariables.inMovement = false;
            FillDoAfterReach();
            InteractionWith();
            DoAfterReach = null;
        }
        //при клике мыши Не на интерфейс, не во время заклинания, не во время интерфейса и не на объект
        //задаём точку движения "к чему" и включаем движение
        
            /*if (Input.GetMouseButtonDown(1) && MainVariables.allowMovement == true)
            {
                if(Time.time - lastUpdatePath > timeBetweenUpdatePath)
                {          
                    lastUpdatePath = Time.time;

                    targetObject = TargetObject.finalPoint;
                    UpdatePath();
                    MainVariables.inMovement = true;
                }
            }*/
        if (UIClick.OnMouseDown() && Input.GetMouseButtonUp(0) && MousePosition2D.supposedInteractionObject != null && MainVariables.inInteraction == false && MainVariables.inInterface == false)   //Interaction
        {
            interactionObject = MousePosition2D.supposedInteractionObject;  
            MainVariables.inInteraction = true;     
            isReached = false;         
            ReachThenInteract();
        }
    }  
    public void ReachThenInteract()
    {
        if (isReached == false)
        {
            //ChaseInterationObject();            
            prevIO = Vector2.zero;
            targetObject = TargetObject.interactionObject;
            UpdatePath();
            MainVariables.inMovement = true;
        }
        /*else
        {
            FillDoAfterReach();
            InteractionWith();
        }*/
    }
    private void InteractionWith()
    {       
        MainVariables.inInteraction = false;        
        
        if (interactionObject != null && DoAfterReach != null)
        {
            if (interactionObject.name == "AdditionalCollider")
                interactionObject = interactionObject.transform.parent.gameObject;

            DoAfterReach.Invoke(interactionObject);
            interactionObject = null;             
        }                       
    }
    private void FillDoAfterReach()
    {
        //ищем тип объекта активного взаимодействия                    
        if (DoAfterReach == null)
        {
            switch(interactionObject.tag)
            {
                //case "Enemy":
                    //if(interactionObject.GetComponent<EnemyData>() != null && interactionObject.GetComponent<EnemyData>().currentSP <= 0)
                        //DoAfterReach += StartEG; 
                //    break; 
                case "Word":                                        
                        DoAfterReach += TakeWord;
                    break;
                case "Chest":
                    ChestBehavior chestBehavior = interactionObject.GetComponent<ChestBehavior>();
                    if (chestBehavior != null && chestBehavior.looted == false)
                        DoAfterReach += OpenChest;
                    break; 
                case "Npc":
                        DoAfterReach += InteractWithNpc;
                    break;
                case "Pass":
                        DoAfterReach += InteractWithPass;
                    break;
                case "Pot":
                        DoAfterReach += InteractWithPot;
                    break;       
            }
        }
    }
    private void TakeWord(GameObject capsuleChild)
    {
        try
        {
            GameObject wordObj = capsuleChild.transform.parent.gameObject;
            takeUp.TakeUpWord(wordObj);
        }
        catch (System.Exception e)
        {
            Debug.LogError("TakeWord(), " + e);
        }        
    }
    private void OpenChest(GameObject chest)
    {        
        Debug.Log("open chest");
        try
        {
            chest.GetComponent<ChestBehavior>().Open();
        }
        catch (System.Exception e)
        {
            Debug.LogError("OpenChest(), " + e);
        }        
    }
    private void InteractWithNpc(GameObject npc)
    {
        try
        {
            npc.GetComponent<NpcBehavior>().Interact();
        }
        catch (System.Exception e)
        {
            Debug.LogError("InteractWithNpc(), " + e);
        }
    }
    private void InteractWithPass(GameObject pass)
    {
        try
        {
            Pass passComponent = pass.GetComponent<Pass>();
            if (passComponent != null)
            {
                //passComponent.speed = speed;
                passComponent.FindContactPoint(transform.position);
                passComponent.DoPass();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("InteractWithPass(), " + e);
        }
    }    
    void InteractWithPot(GameObject pot)
    {
        //Debug.Log("InteractWithPot");
        try
        {
            AlchemyPotBehavior alchemyPotBehavior = pot.GetComponent<AlchemyPotBehavior>();
            if (alchemyPotBehavior != null)
                alchemyPotBehavior.UsePot();
            else
                Debug.LogError("alchemyPotBehavior = null");
        }
        catch (System.Exception e)
        {
            Debug.LogError("InteractWithPot(), " + e);
        }
    }

    private void OnCollisionStay2D(Collision2D other)  //OnTriggerStay2D
    {
        if (interactionObject != null)
        {
            if (other.transform == interactionObject.transform && targetObject == TargetObject.interactionObject)
            {
                Stop();
            }
        } 
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (interactionObject != null)
        {
            if (other.transform == interactionObject.transform && targetObject == TargetObject.interactionObject)
            {
                Stop();
            }
        } 
    }

    private void OnCollisionExit2D(Collision2D other)  //OnTriggerExit2D 
    {
        if (interactionObject != null)
        {
            if (other.transform == interactionObject.transform)
            {
                isReached = false;
            }
        } 
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (interactionObject != null)
        {
            if (other.transform == interactionObject.transform)
            {
                isReached = false;
            }
        } 
    }

    public void TakeImpact(float impact, Vector2 impactPosition)
    {
        MainVariables.inMovement = false;
        MainVariables.inImpacting = true;
        MainVariables.allowMovement = false;

        rigidBody.velocity -= new Vector2(impactPosition.x - gameObject.transform.position.x, impactPosition.y - gameObject.transform.position.y) * impact /10;
        
        StopCoroutine(ImpactStopCoroutine(impact /10));
        StartCoroutine(ImpactStopCoroutine(impact /10));
    }
    private IEnumerator ImpactStopCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        MainVariables.inImpacting = false;  
    }
    
    private void UpdatePath()
    {
        if (targetObject != TargetObject.none && seeker.IsDone())
        {
            switch (targetObject)
            {
                case TargetObject.none:
                    break;
                case TargetObject.interactionObject:
                    if (interactionObject != null)
                    {
                        Vector2 interactionObjectPosition = (Vector2) interactionObject.transform.position;
                        if (prevIO != interactionObjectPosition)
                        {
                            seeker.StartPath(rigidBody.position, interactionObjectPosition, OnPathComplete);
                            prevIO = interactionObjectPosition;
                        }
                    }
                    break;
                case TargetObject.finalPoint:
                    if (objFinalPoint != null)
                    {
                        Vector2 finalPoint = (Vector2) objFinalPoint.transform.position;
                        if (prevFP != finalPoint)
                        {
                            seeker.StartPath(rigidBody.position, finalPoint, OnPathComplete);
                            prevFP = finalPoint;
                        }
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
}
