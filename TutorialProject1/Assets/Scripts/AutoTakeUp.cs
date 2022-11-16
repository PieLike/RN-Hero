using System;
using UnityEngine;

public class AutoTakeUp : MonoBehaviour
{
    private Transform lootItem;
    private GameObject heroObject;
    private Rigidbody2D lootItemRigidBody; 
    private bool inDragging = false;  float smoothTime = 0.06f, Velocity = 0.0f;
    [NonSerialized] public float speed;
    private Vector2 finalPoint;  
    private DictionaryManager dictionaryManager;
    private void Start() 
    {
        lootItem = transform.parent;
        dictionaryManager = FindObjectOfType<DictionaryManager>(); 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Hero")
        {            
            if (lootItem.tag == "Word")
            {   
                //Debug.Log("OnTriggerEnter2D Hero Word");             
                LootBehavior lootBehavior = lootItem.GetComponent<LootBehavior>();
                //Debug.Log("dictionaryManager.CheckExisting(lootBehavior.lootData.word) != null " + (dictionaryManager.CheckExisting(lootBehavior.lootData.word) != null).ToString());
                if (lootBehavior != null && dictionaryManager.CheckExisting(lootBehavior.lootData.word) != (-1))
                {
                    TakeUpTo(other.gameObject); 
                }
            }    
        }    
    }

    private void TakeUpTo(GameObject DraggingToObject)
    {
        //Debug.Log("TakeUpTo");
        lootItemRigidBody = lootItem.GetComponent<Rigidbody2D>();
        inDragging = true;
        heroObject = DraggingToObject;
    }

    void FixedUpdate()
    {
        if (inDragging == true)
        {
            Debug.Log("inDragging");
            //проверяем притянули ли объект к герою
            if (MyMathCalculations.CheckReachToPoint(lootItem.transform.position, finalPoint))
            {
                inDragging = false;
                //TakeUp();
                if (lootItem.tag == "Word")
                {
                    TakeUp takeUp = heroObject.GetComponent<TakeUp>();
                    takeUp.TakeUpWord(lootItem.gameObject);
                }

            }
            //если нет то задаем точку притяжения (каждый раз новую потмоу что герой может двигаться) и двигаем объект
            else if (heroObject != null)
            {
                finalPoint = heroObject.transform.position;
            
                Vector2 direction = new Vector2(
                    Mathf.SmoothDamp(lootItem.transform.position.x, finalPoint.x, ref Velocity, smoothTime),
                    Mathf.SmoothDamp(lootItem.transform.position.y, finalPoint.y, ref Velocity, smoothTime));

                lootItemRigidBody.transform.position = direction;
            }
        }
    }


}
