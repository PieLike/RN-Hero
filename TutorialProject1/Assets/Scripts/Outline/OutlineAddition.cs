using UnityEngine;

public class OutlineAddition : MonoBehaviour
{
    public Material outlineMaterialWhite, defaultSprite, outlineMaterialRed, outlineMaterialBlue, outlineMaterialYellow;
    void Start()
    {
        outlineMaterialWhite = Resources.Load<Material>("Materials/ForOutline");
        outlineMaterialRed = Resources.Load<Material>("Materials/ForOutlineRed");
        outlineMaterialBlue = Resources.Load<Material>("Materials/ForOutlineBlue");
        outlineMaterialYellow = Resources.Load<Material>("Materials/ForOutlineYellow"); 
        defaultSprite = Resources.Load<Material>("Materials/Default");  
    }
}
