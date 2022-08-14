using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Data;

public class Interaction : MonoBehaviour
{    
    public GameObject supposedInteractionObject;
    private GameObject interactionObject;
    public bool interactionIsActive = false;
    public GameObject heroObject;      
    
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
                            
        }
        //если герой не в процессе взаимодействия то очищаем слот активного объекта
        if (interactionIsActive == false)
            interactionObject = null;
            
    }


    private void InteractionWith()
    {
        //ищем тип объекта активного взаимодействия
        switch(interactionObject.tag)
        {
            case "Enemy":
                InteractionWithEnemy(); 
                break; 
            case "Loot":
                InteractionWithLoot(); 
                break;   
        }            
    }

    private void InteractionWithEnemy()
    {
        HeroMove heroMove;
        //взаимодействие с объектом типа Enemy
        string enemyName = interactionObject.name;
        
        heroMove = heroObject.GetComponent<HeroMove>();
        
        //заставим героя идти к объекту пока не подойдет
        if (MyMathCalculations.CheckReachToPoint(interactionObject.transform.position, heroObject.transform.position) != true)
        {
            if (heroMove.finalPoint != interactionObject.transform.position)
            ComeTo(heroMove);
            //начинаем взаимодействие
            interactionIsActive  = true;
        } else {
            //вызываем функцию дропа лута у активного объекта (врага)
            CallLootDrop();
            //делаем его компоненты неактивными (потом нужно будет исправить чтобы полностью удалять)
            interactionObject.GetComponent<MeshCollider>().enabled = false;
            interactionObject.GetComponent<MeshRenderer>().enabled = false;            
            //заканчиваем взаимодействие и очищаем слот активного объект
            interactionIsActive = false;
            interactionObject = null;
        }       

    }

    private void InteractionWithLoot()
    {
        HeroMove heroMove;
        //взаимодействие с объектом типа Loot
        string enemyName = interactionObject.name;
        
        heroMove = heroObject.GetComponent<HeroMove>();
        
        //заставим героя идти к объекту пока не подойдет
        if (MyMathCalculations.CheckReachToPoint(interactionObject.transform.position, heroObject.transform.position) != true)
        {
            if (heroMove.finalPoint != interactionObject.transform.position)
            ComeTo(heroMove);
            //начинаем взаимодействие
            interactionIsActive  = true;
        } else {            
            //подбираем лут
            TakeUp(); 
            //заканчиваем взаимодействие и очищаем слот активного объект
            interactionIsActive = false;
            interactionObject = null;
        }    
    }

    private void ComeTo(HeroMove heroMove)
    {        
        //задать точку подхода героя и активировать движение к ней
        MainVariables.inMovement = true;
        heroMove.finalPoint = interactionObject.transform.position;  
    }

    private void TakeUp()
    {
        //сохранить предмет в активный словарь и уничтожить
        if (CheckExisting(interactionObject) == false)
            AddInDataBase(interactionObject);       
        Destroy(interactionObject);   
    }

    private void CallLootDrop()
    {
        //вызвать выпадение лута у активного объекта
        LootDroping lootDroping;
        lootDroping = interactionObject.GetComponent<LootDroping>();
        lootDroping.Drop();
    }

    private void OnTriggerStay(Collider other) {
        //заполняем слот наведенного объекта тем, что попал под курсор
        supposedInteractionObject = other.gameObject;          
    }
    
    private void FixedUpdate() {
        //очищаем слот наведенного объекта
        supposedInteractionObject = null;    
    }

    private void AddInDataBase(GameObject interactionObject)
    {
        //ищем в общей дб словаря слово и записываем его в активный словарь
        string actualDataBaseName = "vocabularyActual.bytes", generalDataBaseName = "vocabularyGeneral.bytes";
        string itemName = interactionObject.GetComponent<TMP_Text>().text.ToLower();

        //делаем запрос на строку с нужным словом в общем словаре
        DataTable generalVocabulary = WorkWithDataBase.GetTable($"SELECT * FROM words WHERE eng = '{itemName}';", generalDataBaseName);

        //записываем слово из общего словаря в активный
        WorkWithDataBase.InsertOneRow(actualDataBaseName, generalVocabulary);      
    }

    private bool CheckExisting(GameObject interactionObject)
    {
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

    

    
}
