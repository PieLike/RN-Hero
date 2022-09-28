using UnityEngine;
using System;
using TMPro;
using System.Data;

public class Interaction : MonoBehaviour
{    
    public static GameObject supposedInteractionObject;
    private GameObject interactionObject;
    //public bool interactionIsActive = false;
    private GameObject heroObject, objFinalPoint, objEnglishGame;  
    private HeroMove heroMove;
    private Action<GameObject> DoAfterReach;

    private void Start() 
    {
        //находим объекты на сцене
        objEnglishGame = GameObject.Find("Interface/EnglishGame");
        heroObject = GameObject.Find("Hero");
        objFinalPoint = GameObject.Find("FinalPoint2D");

        heroMove = heroObject.GetComponent<HeroMove>();
    } 
    
    private void LateUpdate()
    {       
        //если существует активный объект, то побуждаем героя взаимодейстовать с ним
        if (interactionObject != null)
        {
            InteractionWith();
        }

        //обрабатываем нажатие мыши (если это не нажатие на интерфейс)        
        if (Input.GetMouseButtonDown(0) && UIClick.OnMouseDown() && MainVariables.allowMovement == true)            
        {
            //если наведенный объект существует то делаем его объектом активного взаимодействия
            //и приступаем к взаимодействию
            if (supposedInteractionObject != null)
            {
                interactionObject = supposedInteractionObject;                
                InteractionWith();
            }
            else
            {
                if (interactionObject != null)
                    interactionObject = null;
                if (MainVariables.inInteraction != false)
                    MainVariables.inInteraction = false;    
            }                           
        }
        //если герой не в процессе взаимодействия то очищаем слот активного объекта
        if (MainVariables.inInteraction == false && interactionObject != null)
            interactionObject = null;                           
    }

    private void InteractionWith()
    {       
        if (interactionObject == null)
            return; 
        //ищем тип объекта активного взаимодействия                    
        if (DoAfterReach == null)
        {
            switch(interactionObject.tag)
            {
                case "Enemy":
                    if(interactionObject.GetComponent<EnemyData>() != null && interactionObject.GetComponent<EnemyData>().currentSP <= 0)
                        DoAfterReach += StartEG; 
                    break; 
                case "Word":
                        DoAfterReach += TakeUpWord;
                    break;
                case "Chest":
                    if (interactionObject.GetComponent<ChestBehavior>() != null && interactionObject.GetComponent<ChestBehavior>().looted == false)
                        DoAfterReach += OpenChest;
                    break; 
                case "Npc":
                        DoAfterReach += InteractWithNpc;
                    break;
                case "Pass":
                        DoAfterReach += InteractWithPass;
                    break;
                case "Branch":
                        //DoAfterReach += InteractWithPass;
                    break;       
            }
        }
        if (DoAfterReach != null)
        {
            if (ReachInteractionObject() == true)
            {         
                //Debug.Log(DoAfterReach.ToString() + ", " + interactionObject.name);
                DoAfterReach.Invoke(interactionObject);
                DoAfterReach = null;
                interactionObject = null;
            } 
        }                   
    }

    private void StartEG(GameObject objForEG)
    {
        objEnglishGame.GetComponent<EnglishGame>().StartGame(objForEG, true);
    }
    private void OpenChest(GameObject chest)
    {
        chest.GetComponent<ChestBehavior>().Open();
    }
    private void InteractWithNpc(GameObject npc)
    {
        npc.GetComponent<NpcBehavior>().Interact();
    }
    private void InteractWithPass(GameObject pass)
    {
        pass.GetComponent<Pass>().DoPass();
    }
    public static void TakeUpWord(GameObject takingWord)
    { 
        if (takingWord == null)
        {
            Debug.Log("попытка поднять пустой объект слово");
            return;
        }
        //сохранить предмет в активный словарь и уничтожить
        Voculabrary.AddWordInDataBase(takingWord);  
        Destroy(takingWord);   
    }    


    private bool ReachInteractionObject()
    {
        //FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)
        if (objFinalPoint != null)
        {
            //заставим героя идти к объекту пока не подойдет
            if (HeroMove.isReached == false)
            {            
                if (MainVariables.inMovement == false)
                    MainVariables.inMovement = true;            
                //начинаем взаимодействие
                if (MainVariables.inInteraction == false)
                    MainVariables.inInteraction = true;
                return false;
            } 
            else 
            {                    
                //заканчиваем взаимодействие и очищаем слот активного объект
                MainVariables.inInteraction = false;            
                objFinalPoint.SetActive(false);
                if (MainVariables.inMovement == true)
                    MainVariables.inMovement = false;   
                return true;
            }   
        }
        else
            return false; 
    }
   
    private void OnTriggerStay2D(Collider2D other) {
        //заполняем слот наведенного объекта тем, что попал под курсор
        foreach(string tag in MainVariables.interactionTags)
        {
            if(other.gameObject.tag == tag)
            {
                if (supposedInteractionObject != other.gameObject)
                    supposedInteractionObject = other.gameObject;
                return;
            } 
        }      
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        //очищаем слот наведенного объекта
        if (supposedInteractionObject != null)
            supposedInteractionObject = null; 
    }    
    
    /*private void FindFinalPoint()
    {
        if (objFinalPoint == null)
        {
            objFinalPoint = GameObject.Find("FinalPoint2D(Clone)");            
        }
    }  */  
    
}
