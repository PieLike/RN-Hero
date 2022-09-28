using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOutline : MonoBehaviour
{
    public enum Color{red, blue, yellow}
    OutlineAddition outlineAddition;
    SpriteRenderer spriteRenderer;
    Transform transformOutline;
    public bool outlineSet;

    void Start()
    {        
        outlineAddition = FindObjectOfType<OutlineAddition>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); 
        transformOutline = gameObject.GetComponent<Transform>(); 
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
                spriteRenderer.material = outlineAddition.outlineMaterialBlue;
                break;
            case (Color.yellow):
                spriteRenderer.material = outlineAddition.outlineMaterialYellow;
                break;
            }
 
            transformOutline.localScale = transform.localScale * 1.0625f;
            outlineSet = true;
        }
    }
    public void RemoveOutline()
    {
        if (outlineSet == true)
        {
            spriteRenderer.material = outlineAddition.defaultSprite;  
            transformOutline.localScale = transform.localScale / 1.0625f;
            outlineSet = false;
        }
    }
}
