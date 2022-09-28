using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    //public GameObject parent;
    private float downBoard = 0.0f;
    new Transform transform;
    new Rigidbody2D rigidbody2D;
    public float parentpositionY = 0.0f, parentlocalScaleY = 0.0f;
    
    private void Start() 
    {
        transform = gameObject.GetComponent<Transform>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        if (rigidbody2D.gravityScale != 0 || rigidbody2D.velocity != Vector2.zero)
        {
            if (downBoard == 0.0f && parentpositionY != 0.0f && parentlocalScaleY != 0.0f)
            {
                //Transform parentTransform = parent.GetComponent<Transform>();
                var range = Random.Range(-1f,1f);
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
}
