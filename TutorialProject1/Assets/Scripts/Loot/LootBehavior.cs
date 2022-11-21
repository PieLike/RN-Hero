using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    //public GameObject parent;
    private float downBoard = 0.0f;
    GameObject objectOutline; private MyOutline outline; SpriteRenderer spriteRenderer; Color originalColor;
    new Rigidbody2D rigidbody2D; new Transform transform;
    [NonSerialized] public float parentpositionY = 0.0f, parentlocalScaleY = 0.0f;
    public Loot lootData;   private Transform hero;
    private bool canTakeUp;
    
    private void Start() 
    {
        transform = gameObject.GetComponent<Transform>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        Transform textChildren = transform.Find("Text");
        textChildren.GetComponent<TMPro.TMP_Text>().text = lootData.word.word;

        objectOutline = transform.Find("Capsule").gameObject; 
        if (objectOutline != null)
        {
            //outline = objectOutline.GetComponent<MyOutline>();
            spriteRenderer = objectOutline.GetComponent<SpriteRenderer>();
        }
        else
            Debug.LogError("Cant find capsule child object for word");        
    }

    private void FixedUpdate() 
    {
        if (rigidbody2D.gravityScale != 0 || rigidbody2D.velocity != Vector2.zero)
        {
            if (downBoard == 0.0f && parentpositionY != 0.0f && parentlocalScaleY != 0.0f)
            {
                //Transform parentTransform = parent.GetComponent<Transform>();
                var range = UnityEngine.Random.Range(-1f,1f);
                downBoard = parentpositionY- (parentlocalScaleY / 2) + range;   
            }
            else if (downBoard != 0.0f && transform.position.y < downBoard)
            {
                rigidbody2D.gravityScale = 0;
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.Sleep();
            }
        }
    }

    private void Update() 
    {
        if (MousePosition2D.supposedInteractionObject == objectOutline)
        {
            if (spriteRenderer.color != Color.blue)
            {
                originalColor = spriteRenderer.color;
                spriteRenderer.color = Color.blue;
            }
        }    
        else if (spriteRenderer.color != originalColor && originalColor != new Color(0f,0f,0f,0f))
        {
            spriteRenderer.color = originalColor;
        } 

        /*if (canTakeUp && Input.GetKeyDown(KeyCode.E) && MainVariables.inInterface == false)
        {
            var takeUp = hero.GetComponent<TakeUp>();
            takeUp.TakeUpWord(this.gameObject);
        }*/
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.tag == "HeroUse" && canTakeUp == false)
        {
            hero = other.transform.parent;
            //interactionAnimator.SetBool("Show", true);
            canTakeUp = true;
            OnGameStartAndUpdate.AddInteraction(TakeUp);
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "HeroUse" && canTakeUp)
        {
            //interactionAnimator.SetBool("Show", false);
            canTakeUp = false;
            OnGameStartAndUpdate.RemoveInteraction(TakeUp);
        }
    }

    public void TakeUp()
    {
        var takeUp = hero.GetComponent<TakeUp>();
        takeUp.TakeUpWord(this.gameObject);
        canTakeUp = false;
    }
}
