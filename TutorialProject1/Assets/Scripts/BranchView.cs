using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MyOutline))]
public class BranchView : MonoBehaviour
{
    private MyOutline outline;
    private void Start() 
    {
        outline = GetComponent<MyOutline>();         
    }
    private void Update() 
    {
        //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
        if (Interaction.supposedInteractionObject == gameObject)
        {
            //если уже стоит то не будет делать
            outline.SetOutline(MyOutline.Color.blue);
        }    
        else
        {
            outline.RemoveOutline();  
        } 
    }
}
