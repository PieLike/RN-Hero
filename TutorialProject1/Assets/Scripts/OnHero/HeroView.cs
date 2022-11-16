using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : MonoBehaviour
{
    private Animator animator; private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    GameObject reloadSprite; Renderer reloadSpriteRenderer; MaterialPropertyBlock reloadSpritePropBlock;
    float timeReloadEnable, timeReloadDisable; bool reloadUpdate;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        reloadSprite = transform.Find("Reload").gameObject;
        reloadSpritePropBlock = new MaterialPropertyBlock();
        reloadSpriteRenderer = reloadSprite.GetComponent<Renderer>();
        reloadSprite.SetActive(false);
    }
    
    void Update()
    {
        if (spriteRenderer.flipX == false && rigidBody.velocity.x >= 0.01f)
            spriteRenderer.flipX = true;
        else if (spriteRenderer.flipX == true && rigidBody.velocity.x <= -0.01f)
            spriteRenderer.flipX = false;

        if (MainVariables.inMovement == true)
        {
            animator.SetBool("Walking", true);
        }
        else
        {            
            animator.SetBool("Walking", false);
        }

        if (reloadUpdate)
        {
            reloadSpriteRenderer.GetPropertyBlock(reloadSpritePropBlock);
            //_propBlock.SetColor("_Color", Color.Lerp(Color.red, Color.blue, (Mathf.Sin(Time.time * 10) + 1) / 2f));
            float remaining = timeReloadDisable - Time.time;
            if (remaining >= 0) 
            {
                float total = timeReloadDisable - timeReloadEnable;
                if (total > 0)
                {
                    reloadSpritePropBlock.SetInt("_Arc1", (int) (remaining / total * 360));
                    reloadSpriteRenderer.SetPropertyBlock(reloadSpritePropBlock);
                }
            }
            else
            {
                reloadUpdate = false; 
                reloadSprite.SetActive(false);
            }
        }
    }

    public void BeginReload(float timeDashEnable, float timeDashDisable)
    {
        timeReloadEnable = timeDashEnable; timeReloadDisable = timeDashDisable;

        reloadSprite.SetActive(true);
        reloadUpdate = true;
    }
}
