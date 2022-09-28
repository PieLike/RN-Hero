using UnityEngine;

public class CursorRenderer : MonoBehaviour
{
    SpriteRenderer sprite;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();    
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        /*if(other.gameObject.tag == "Floor")
        {
            if (sprite.enabled == false)
                sprite.enabled = true; 
        }
        else*/
        //{
            if(other.gameObject.tag == "Hero")
            {
                if (sprite.enabled == true)
                    sprite.enabled = false;
                return;
            } 

            foreach(string tag in MainVariables.interactionTags)
            {
                if(other.gameObject.tag == tag)
                {
                    if (sprite.enabled == true)
                        sprite.enabled = false;
                    return;
                } 
            }  
        //}
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        /*if(other.gameObject.tag == "Floor")
        {
            if (sprite.enabled == true)
                sprite.enabled = false; 
        }*/
        //else 
        if (sprite.enabled == false)
        {
            sprite.enabled = true;  
        }    
    }
}
