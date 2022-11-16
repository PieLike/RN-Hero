using UnityEngine;

public class CursorRenderer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void Update()
    {
        if (MainVariables.inInterface == false)
        {
            if (spriteRenderer.enabled != true)
                spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Hero")
        {
            if (spriteRenderer.enabled == true)
                spriteRenderer.enabled = false;
            return;
        } 

        foreach(string tag in MainVariables.interactionTags)
        {
            if(other.gameObject.tag == tag)
            {
                if (spriteRenderer.enabled == true)
                    spriteRenderer.enabled = false;
                return;
            } 
        } 
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (spriteRenderer.enabled == false)
        {
            spriteRenderer.enabled = true;  
        }    
    }
}
