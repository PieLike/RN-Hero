//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Outline))]

public class ChestBehavior : MonoBehaviour
{
    private Outline outline; 
    [NonSerialized] public bool looted = false;

    private void Start() 
    {
        outline = GetComponent<Outline>(); 
        outline.OutlineWidth = 0; 
    } 

    void Update()
    {      
        if (looted == false)
        {
            //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
            if (Interaction.supposedInteractionObject == gameObject)
            {
                outline.OutlineWidth = 3;
            }    
            else if (outline.OutlineWidth != 0)
                outline.OutlineWidth = 0;
        }  
    }  

    public void Open() 
    {  
        //запускаем анимацию открывания сундука
        gameObject.GetComponent<Animator>().SetBool("isOpen", true);
        //выбрасываем из него лут
        gameObject.GetComponent<LootDroping>().Drop();
        //меняем статус на облутанный и на всякий случай убираем обводку
        looted = true;
        outline.OutlineWidth = 0;
    }
}
