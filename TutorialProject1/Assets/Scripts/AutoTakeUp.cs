using System;
using UnityEngine;

public class AutoTakeUp : MonoBehaviour
{
    [SerializeField] private GameObject lootItem;
    private GameObject heroObject;
    private Rigidbody2D lootItemRigidBody; 
    private bool inDragging = false;  
    [NonSerialized] public float speed;
    private Vector2 finalPoint;  
    private DictionaryManager dictionaryManager;
    private void Start() 
    {
        dictionaryManager = FindObjectOfType<DictionaryManager>(); 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Hero")
        {
            if (lootItem.tag == "Word")
            {
                
                if (dictionaryManager.CheckExisting(lootItem) == true)
                {
                    TakeUpTo(other.gameObject); 
                }
            }
            else if(lootItem.tag != "Untagged")
            {
                TakeUpTo(other.gameObject);
            }            
        }    
    }

    private void TakeUpTo(GameObject DraggingToObject)
    {
        lootItemRigidBody = lootItem.GetComponent<Rigidbody2D>();
        inDragging = true;
        heroObject = DraggingToObject;
    }

    void FixedUpdate()
    {
        if (inDragging == true)
        {
            //проверяем притянули ли объект к герою
            if (MyMathCalculations.CheckReachToPoint(lootItem.transform.position, finalPoint))
            {
                inDragging = false;
                //TakeUp();
                if (lootItem.tag == "Word")
                    Interaction.TakeUpWord(lootItem);

            }
            //если нет то задаем точку притяжения (каждый раз новую потмоу что герой может двигаться) и двигаем объект
            else if (heroObject != null)
            {
                finalPoint = heroObject.transform.position;
            
                Vector2 direction = MyMathCalculations.CalculateDirectionSpeeds(lootItem.transform.position, finalPoint);
                lootItemRigidBody.MovePosition(lootItemRigidBody.position + direction * Time.deltaTime * speed);      
            }
        }
    }


}
