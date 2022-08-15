using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Spells : MonoBehaviour
{
    public GameObject mousePosition;
    public int activeSpellSlot = 1;
    private BookOfSpells.spell activeSpell;
    public GameObject UsingSpell_Interface, UsingSpellName, UsingSpellCount;
    private int usesRemain = 0;
    public GameObject directionPlane;
    private bool isWaitingEnemy = false;
    private GameObject interactionObject;

    
    private void Update() 
    {
        interactionObject = Interaction.supposedInteractionObject;
        //находим активный спелл по номеру активного слота заклинания 
        SetActiveSpell(activeSpellSlot);
        //если спрайт направления активен то задаем ему направление и положение
        if (directionPlane.activeSelf == true) 
        {
            float angle = MyMathCalculations.CalculateAngle(transform.position, mousePosition.transform.position, transform.right) * (-1); 
            directionPlane.transform.rotation = Quaternion.Slerp(directionPlane.transform.rotation, Quaternion.Euler(90,0,angle), 1f);

            directionPlane.transform.position = new Vector3(transform.position.x,directionPlane.transform.position.y,transform.position.z);
        }        
    }

    private void Start() 
    {
        BookOfSpells.FillSlot(1, "frostspell"); //потом убрать
        BookOfSpells.FillSlot(2, "recognizespell"); //потом убрать
    }
    public void LateUpdate() 
    {
        if (MainVariables.inInterface == false)
        {
            //обрабатываем нажатия клавиш цифр, находим соответсвующее заклинания, и если оно есть, то записываем его активным и активируем режим заклинания
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                activeSpellSlot = 1;
                SetActiveSpell(activeSpellSlot);  
                SpellBegining();
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                activeSpellSlot = 2;
                SetActiveSpell(activeSpellSlot);  
                SpellBegining();
            }


            //если режим заклинания активен
            if (MainVariables.inSpelling)
            {
                //используем заклинание по клику мыши
                if (Input.GetMouseButtonDown(0) && UIClick.OnMouseDown())
                {
                    //если ожидается указание объекта то проверяем есть ли такой
                    if(isWaitingEnemy == false)
                        UseSpell(activeSpell);
                    else if (interactionObject != null)
                    {
                        if (interactionObject.tag == "Enemy")
                        {
                            UseSpell(activeSpell);
                        }
                        else
                            Debug.Log("Не враг");
                        
                    }
                }
                //сбрасываем заклинание по кнопке
                if (Input.GetKeyUp(KeyCode.R))
                {
                    usesRemain = 1;
                    SpellEnding();    
                }                           
            }
        }
        //очищаем слот наведенного объекта
        interactionObject = null;
    }

    private void SpellBegining()
    {
        //если активное заклинание есть
        if(String.IsNullOrEmpty(activeSpell.name) == false)
        {
            //то что будет выполнятся в начале активного использорвания заклинаний
            //показываем интерфейс режим заклинания, записываем количество оставшихся использований, включаем режим заклинаний, отображаем спрайт указателя
            ShowSpellInterface(activeSpell.fullname, activeSpell.bullets);
            usesRemain = activeSpell.bullets;
            MainVariables.inSpelling = true;
            directionPlane.SetActive(true); 

            if (activeSpell.type == "DE") //|| activeSpell.type == "DH")
                isWaitingEnemy = true;
            else
                isWaitingEnemy = false;
        }
    }

    private void SpellEnding()
    {
        //то что будет выполнятся в конце каждого использорвания заклинания
        //если использований больше не осталось, то выполнится всё, что заканчивает режим активного использования заклинаний
        usesRemain -= 1;
        if (usesRemain < 1)
        {
            CloseSpellInterface();
            MainVariables.inSpelling = false;
            directionPlane.SetActive(false); 
        } 
        else
            ChangeUsesCountMinus(activeSpell.bullets);             
    }

    public void UseSpell(BookOfSpells.spell activeSpell)
    {
        //Debug.Log(activeSpell.name);
        //находим тип заклинания и зупаскаем соответсвующий класс
        switch(activeSpell.type)
        {
            case("NPP"): //Non Physic Projectile
                //находим угол между вектором +X и вектором от героя до курсора
                float angle = MyMathCalculations.CalculateAngle(transform.position, mousePosition.transform.position, transform.right); 
                UseNonPhysicProjectileSpell(activeSpell.speed, activeSpell.distance, activeSpell.prefab, angle, activeSpell.damage);
            break;

            case("DE"): //Directed on Enemy
                UseDirectedOnEnemySpell();
            break;
        }
        //запускаем класс действий после использования заклинания
        SpellEnding();  
    } 

    private void UseNonPhysicProjectileSpell(float speed, float distance, string stringProjectile, float angle, float damage)
    {
        GameObject projectile = Resources.Load<GameObject>("Spells/" + stringProjectile.ToString());
 
        Vector3 placeInstantiate = transform.position;

        GameObject newSpellObject = Instantiate(projectile, placeInstantiate, transform.rotation * Quaternion.Euler(Math.Abs(angle),angle,-90));
        
        SpellBehavior newSpellBehavior = newSpellObject.GetComponent<SpellBehavior>(); 
        newSpellBehavior.NonPhysicProjectile(speed, distance, damage);        
    } 

    private void UseDirectedOnEnemySpell()
    {
        LootDroping lootDroping = interactionObject.GetComponent<LootDroping>();
        lootDroping.Drop();
    }

    private void ShowSpellInterface(string spellFullName, int spellBullets)
    {
        UsingSpell_Interface.SetActive(true);  
        UsingSpellName.GetComponent<TMP_Text>().text = spellFullName;
        UsingSpellCount.GetComponent<TMP_Text>().text = spellBullets.ToString() + "/" + spellBullets.ToString(); 
    }  

    private void CloseSpellInterface()
    {
        UsingSpell_Interface.SetActive(false);    
    }

    private void ChangeUsesCountMinus(int spellBullets)
    {
        UsingSpellCount.GetComponent<TMP_Text>().text = usesRemain + "/" + spellBullets.ToString();
    }

    private void SetActiveSpell(int spellSlot)
    {
        switch (spellSlot)
        {
            case(1): activeSpell = BookOfSpells.slot1data; 
            break;
            case(2): activeSpell = BookOfSpells.slot2data; 
            break; 
            case(3): activeSpell = BookOfSpells.slot3data; 
            break; 
            case(4): activeSpell = BookOfSpells.slot4data; 
            break; 
            case(5): activeSpell = BookOfSpells.slot5data; 
            break; 
            case(6): activeSpell = BookOfSpells.slot6data; 
            break; 
            case(7): activeSpell = BookOfSpells.slot7data; 
            break;
            case(8): activeSpell = BookOfSpells.slot8data; 
            break; 
            case(9): activeSpell = BookOfSpells.slot9data; 
            break;     

            default: activeSpell = BookOfSpells.slot1data;
            break;
        }
    }
    private Vector3 SetMousePositionToFinalPoint()
    {
        Vector3 finalPoint;

        finalPoint.x = mousePosition.transform.position.x;
        finalPoint.y = transform.position.y;
        finalPoint.z = mousePosition.transform.position.z;

        return finalPoint;
    }    

}
