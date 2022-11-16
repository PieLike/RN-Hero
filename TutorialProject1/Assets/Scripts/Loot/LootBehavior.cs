using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    //public GameObject parent;
    private float downBoard = 0.0f;
    new Transform transform;    GameObject objectOutline; private MyOutline outline;    SpriteRenderer spriteRenderer; Color originalColor;
    new Rigidbody2D rigidbody2D;
    [NonSerialized] public float parentpositionY = 0.0f, parentlocalScaleY = 0.0f;
    public Loot lootData;
    
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
            if (spriteRenderer.color != Color.yellow)
            {
                originalColor = spriteRenderer.color;
                spriteRenderer.color = Color.yellow;
            }
        }    
        else if (spriteRenderer.color != originalColor && originalColor != new Color(0f,0f,0f,0f))
        {
            spriteRenderer.color = originalColor;
        } 
    }
}
