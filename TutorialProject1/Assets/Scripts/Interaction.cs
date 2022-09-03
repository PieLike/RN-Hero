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
        if (Input.GetMouseButtonDown(0) && UIClick.OnMouseDown() && MainVariables.inSpelling == false && MainVariables.inInterface == false)            
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
                    if(interactionObject.GetComponent<EnemyBehavior>().currentSP <= 0)
                        DoAfterReach += StartEG; 
                    break; 
                case "Word":
                        DoAfterReach += TakeUpWord;
                    break;
                case "Chest":
                    if (interactionObject.GetComponent<ChestBehavior>().looted == false)
                        DoAfterReach += OpenChest;
                    break; 
                case "Npc":
                        DoAfterReach += InteractWithNpc;
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
    public static void TakeUpWord(GameObject takingWord)
    { 
        if (takingWord == null)
        {
            Debug.Log("попытка поднять пустой объект слово");
            return;
        }
        //сохранить предмет в активный словарь и уничтожить
        if (CheckExisting(takingWord) == false)
            AddInDataBase(takingWord);       
        Destroy(takingWord);   
    }    


    private bool ReachInteractionObject()
    {
        FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)
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
            } else 
            {                    
                //заканчиваем взаимодействие и очищаем слот активного объект
                MainVariables.inInteraction = false;            
                objFinalPoint.SetActive(false);
                return true;
            }   
        }
        else
            return false; 
    }
   
    private void OnTriggerStay(Collider other) {
        //заполняем слот наведенного объекта тем, что попал под курсор
        foreach(string tag in MainVariables.interactionTags)
        {
            if(other.gameObject.tag == tag)
            {
                supposedInteractionObject = other.gameObject;
                //Debug.Log(supposedInteractionObject.ToString());   
                return;
            } 
        }      
    }
    
    private void FixedUpdate() {
        //очищаем слот наведенного объекта
        supposedInteractionObject = null;    
    }

    public static void AddInDataBase(GameObject interactionObject)
    {
        //ищем в общей дб словаря слово и записываем его в активный словарь
        string actualDataBaseName = "vocabularyActual.bytes", generalDataBaseName = "vocabularyGeneral.bytes";
        string itemName = interactionObject.GetComponent<TMP_Text>().text.ToLower();

        //делаем запрос на строку с нужным словом в общем словаре
        DataTable generalVocabulary = WorkWithDataBase.GetTable($"SELECT * FROM words WHERE eng = '{itemName}';", generalDataBaseName);

        //записываем слово из общего словаря в активный
        WorkWithDataBase.InsertOneRow(actualDataBaseName, generalVocabulary, "words");      
    }

    public static bool CheckExisting(GameObject interactionObject)
    {
        //проверяем есть ли такой объект у героя (в словаре)
        string actualDataBaseName = "vocabularyActual.bytes";
        string itemName = interactionObject.GetComponent<TMP_Text>().text.ToLower();

        //делаем запрос на строку с нужным словом в актуальном словаре
        DataTable actualVocabulary = WorkWithDataBase.GetTable($"SELECT * FROM words WHERE eng = '{itemName}';", actualDataBaseName);

        foreach(DataRow row in actualVocabulary.Rows)
        {
            return true;
        }
        return false;
    }

    private void FindFinalPoint()
    {
        if (objFinalPoint == null)
        {
            objFinalPoint = GameObject.Find("FinalPoint(Clone)");            
        }
    }    
    
}
