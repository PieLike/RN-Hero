using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForCheckmark : MonoBehaviour
{
    [SerializeField] private Sprite red, green;
    private void Start() 
    {
        //находим объекты в resourses
        red = Resources.Load<Sprite>("Sprites/crossmark");
        green = Resources.Load<Sprite>("Sprites/checkmark");        
    }

    public void GoGreen()
    {
        gameObject.GetComponent<Image>().sprite = green;
    }
    public void GoRed()
    {
        gameObject.GetComponent<Image>().sprite = red;   
    }
}
