using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOutline : MonoBehaviour
{
    public enum Color{red, blue, yellow}    public Color col;
    OutlineAddition outlineAddition;
    SpriteRenderer spriteRenderer;
    Transform transformOutline;
    private bool outlineSet;
    public bool outlineParent;  GameObject parent;
    public bool fat;

    void Start()
    {        
        outlineAddition = FindObjectOfType<OutlineAddition>();
        if (outlineParent == false)
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            transformOutline = gameObject.GetComponent<Transform>(); 
        }
        else
        {
            parent = transform.parent.gameObject;
            spriteRenderer = parent.GetComponent<SpriteRenderer>();
            transformOutline = parent.GetComponent<Transform>(); 
        }         
    }

    private void Update() 
    {        
        if (MousePosition2D.supposedInteractionObject == gameObject || (outlineParent && MousePosition2D.supposedInteractionObject == parent))
        {
            //если уже стоит то не будет делать
            SetOutline(col);
        }    
        else
        {
            RemoveOutline();  
        } 
    }

    // Update is called once per frame
    public void SetOutline(Color color)
    {
        if (outlineSet == false)
        {
            switch (color){
            case (Color.red):
                spriteRenderer.material = outlineAddition.outlineMaterialRed;
                break;
            case (Color.blue):
                if (fat)
                    spriteRenderer.material = outlineAddition.outlineMaterialFatBlue;
                else
                    spriteRenderer.material = outlineAddition.outlineMaterialBlue;
                break;
            case (Color.yellow):
                spriteRenderer.material = outlineAddition.outlineMaterialYellow;
                break;
            }
 
            transformOutline.localScale = transformOutline.localScale * 1.0625f;
            outlineSet = true;
        }
    }
    public void RemoveOutline()
    {
        if (outlineSet == true)
        {
            spriteRenderer.material = outlineAddition.defaultSprite;  
            transformOutline.localScale = transformOutline.localScale / 1.0625f;
            outlineSet = false;
        }
    }

    private void OnDisable() 
    {
        RemoveOutline();
    }
}
