//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Data;

public class Interaction : MonoBehaviour
{    
    [NonSerialized] public static GameObject supposedInteractionObject;
    private GameObject interactionObject;
    //public bool interactionIsActive = false;
    private GameObject heroObject, objFinalPoint, objEnglishGame;  
    private HeroMove heroMove;

    private void Start() 
    {
        //находим объекты на сцене
        objEnglishGame = GameObject.Find("Interface/EnglishGame");
        heroObject = GameObject.Find("Hero");

        heroMove = heroObject.GetComponent<HeroMove>();
    } 
    
    private void Update()
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
        FindFinalPoint();   //на всйкий случай находим FinalPoint (еси она уже найдена то он не будет заного искать)
        //ищем тип объекта активного взаимодействия
        if (objFinalPoint != null)
        {
            switch(interactionObject.tag)
            {
                case "Enemy":
                    InteractionWithEnemy(); 
                    break; 
                case "Word":
                    InteractionWithWord(); 
                    break;
                case "Chest":
                    InteractionWithChest(); 
                    break;      
            } 
        }           
    }

    private void InteractionWithEnemy()
    {
        //взаимодействие с объектом типа Enemy        
        
        //разрешаем взаимодейтсвие с врагом когда у него нет щита
        if(interactionObject.GetComponent<EnemyBehavior>().currentSP <= 0)
        {
            //заставим героя идти к объекту пока не подойдет
            //if (MyMathCalculations.CheckReachToPoint(interactionObject.transform.position, heroObject.transform.position) != true)
            if (HeroMove.isReached == false)
            {
                ComeTo(heroMove);
                //начинаем взаимодействие
                if (MainVariables.inInteraction == false)
                    MainVariables.inInteraction = true;
            } else {

                objEnglishGame.GetComponent<EnglishGame>().StartGame(interactionObject, true);

                //заканчиваем взаимодействие и очищаем слот активного объект
                MainVariables.inInteraction = false;
                interactionObject = null;

                objFinalPoint.SetActive(false);
            }     
        }
    }

    private void InteractionWithWord()
    {
        //взаимодействие с объектом типа Word 
        
        //заставим героя идти к объекту пока не подойдет
        //if (MyMathCalculations.CheckReachToPoint(interactionObject.transform.position, heroObject.transform.position) != true)
        if (HeroMove.isReached == false)
        {
            ComeTo(heroMove);
            //начинаем взаимодействие
            if (MainVariables.inInteraction == false)
                MainVariables.inInteraction = true;
        } else {                  
            //подбираем лут
            Interaction.TakeUpWord(interactionObject); 
            //заканчиваем взаимодействие и очищаем слот активного объект
            MainVariables.inInteraction = false;
            interactionObject = null;

            objFinalPoint.SetActive(false);
        }    
    }

    private void InteractionWithChest()
    {
        if (interactionObject.GetComponent<ChestBehavior>().looted == false)
        {
            //взаимодействие с объектом типа Chest            
            
            //заставим героя идти к объекту пока не подойдет
            //if (MyMathCalculations.CheckReachToPoint(interactionObject.transform.position, heroObject.transform.position) != true)
            if (HeroMove.isReached == false)
            {
                ComeTo(heroMove);
                //начинаем взаимодействие
                if (MainVariables.inInteraction == false)
                    MainVariables.inInteraction = true;
            } else {   
                //открываем сундук
                OpenChest(); 
                //заканчиваем взаимодействие и очищаем слот активного объект
                MainVariables.inInteraction = false;
                interactionObject = null;

                objFinalPoint.SetActive(false);
            } 
        }      
    }

    private void OpenChest()
    {
        interactionObject.GetComponent<ChestBehavior>().Open();
    }

    private void ComeTo(HeroMove heroMove)
    {        
        //задать точку подхода героя и активировать движение к ней
        if (MainVariables.inMovement == false)
            MainVariables.inMovement = true;
        //heroMove.finalPoint = interactionObject.transform.position;  
    }

    public static void TakeUpWord(GameObject takingWord)
    {
        //сохранить предмет в активный словарь и уничтожить
        if (CheckExisting(takingWord) == false)
            AddInDataBase(takingWord);       
        Destroy(takingWord);   
    }    

    private void OnTriggerStay(Collider other) {
        //заполняем слот наведенного объекта тем, что попал под курсор
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Word" || other.gameObject.tag == "Chest")
            supposedInteractionObject = other.gameObject;          
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
        WorkWithDataBase.InsertOneRow(actualDataBaseName, generalVocabulary);      
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
            objFinalPoint = GameObject.Find("FinalPoint(Clone)");
    }    

    
}
