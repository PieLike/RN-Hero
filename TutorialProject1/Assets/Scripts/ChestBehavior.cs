using UnityEngine;
using System;
[RequireComponent(typeof(MyOutline))]

public class ChestBehavior : MonoBehaviour
{
    private MyOutline outline; 
    [NonSerialized] public bool looted = false;

    private void Start() 
    {
        outline = GetComponent<MyOutline>();
    }

    void Update()
    {      
        if (looted == false)
        {
            //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
            if (Interaction.supposedInteractionObject == gameObject)
            {
                outline.SetOutline(MyOutline.Color.yellow);
            }    
            else
            {
                outline.RemoveOutline();  
            }
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
        outline.RemoveOutline(); 
    }
}
