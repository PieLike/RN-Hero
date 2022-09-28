//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
[RequireComponent(typeof(PickSpellUI))]

public class Spells : MonoBehaviour
{
    private GameObject MouseTarget; 
    private GameObject objEnglishGame, prefabDirectionPlane, directionPlane;    
    private GameObject USInterface, UsingSpellPanel, USName, USCount;
    private BookOfSpells.spell activeSpell;
    private int usesRemain = 0;
    private bool isWaitingEnemy = false;
    private GameObject interactionObject;
    float lastSpell;

    private void Start() 
    {
        //находим объекты на сцене
        USInterface = GameObject.Find("Interface/UsingSpell");
        objEnglishGame = GameObject.Find("Interface/EnglishGame");
        MouseTarget = GameObject.Find("MouseTarget");
        //находим дочерние объекты
        UsingSpellPanel = USInterface.transform.Find("UsingSpellPanel").gameObject;
        USName = USInterface.transform.Find("UsingSpellPanel/USName").gameObject;
        USCount = USInterface.transform.Find("UsingSpellPanel/USCount").gameObject;
        //находим ресурсы (и создаем на сцене)
        prefabDirectionPlane = Resources.Load<GameObject>("3d_prefabs/DirectionPlane");
            directionPlane = Instantiate(prefabDirectionPlane);
            directionPlane.SetActive(false); 

        //находим активный спелл по номеру активного слота заклинания 
        SetActiveSpell(MainVariables.activeSpellSlot);       
    }    
    private void Update() 
    {
        if (interactionObject != Interaction.supposedInteractionObject)
            interactionObject = Interaction.supposedInteractionObject;
        
        //если спрайт направления активен то задаем ему направление и положение
        if (directionPlane.activeSelf == true) 
        {
            float angle = MyMathCalculations.CalculateAngle2D(transform.position, MouseTarget.transform.position, transform.right) * (-1);
            directionPlane.transform.rotation = Quaternion.Slerp(directionPlane.transform.rotation, Quaternion.Euler(0f,0f,angle), 1f);

            directionPlane.transform.position = new Vector2(transform.position.x, transform.position.y);
        }        
    }
    
    public void LateUpdate() 
    {
        if (MainVariables.inInterface == false)
        {
            bool buttonIsPressed = false;
            //обрабатываем нажатия клавиш цифр, находим соответсвующее заклинания, и если оно есть, то записываем его активным и активируем режим заклинания
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                MainVariables.activeSpellSlot = 1;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                MainVariables.activeSpellSlot = 2;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                MainVariables.activeSpellSlot = 3;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                MainVariables.activeSpellSlot = 4;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                MainVariables.activeSpellSlot = 5;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha6))
            {
                MainVariables.activeSpellSlot = 6;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha7))
            {
                MainVariables.activeSpellSlot = 7;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha8))
            {
                MainVariables.activeSpellSlot = 8;
                buttonIsPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Alpha9))
            {
                MainVariables.activeSpellSlot = 9;
                buttonIsPressed = true;
            }

            if (buttonIsPressed == true)
            {
                SetActiveSpell(MainVariables.activeSpellSlot);  
                SpellBegining();

                objEnglishGame.GetComponent<EnglishGame>().StartGame(activeSpell.name);
            }


            //если режим заклинания активен
            if (MainVariables.inSpelling)
            {
                //используем заклинание по клику мыши
                if (Input.GetMouseButton(0) && UIClick.OnMouseDown())
                {
                    if(Time.time - lastSpell > activeSpell.usingInterval)
                    {
                        lastSpell = Time.time;
                
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

            if (activeSpell.type == ScriptableObjSpell.SpellType.DirectedOnEnemy) //|| activeSpell.type == "DH")
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
        float angle;
        //находим тип заклинания и зупаскаем соответсвующий класс
        switch(activeSpell.type)
        {
            case(ScriptableObjSpell.SpellType.NonPhysicProjectile): //Non Physic Projectile
                //находим угол между вектором +X и вектором от героя до курсора                
                angle = MyMathCalculations.CalculateAngle2D(transform.position, MouseTarget.transform.position, transform.up);
                UseNonPhysicProjectileSpell(activeSpell, angle, MouseTarget.transform.position);
            break;

            case(ScriptableObjSpell.SpellType.DirectedOnEnemy): //Directed on Enemy
                UseDirectedOnEnemySpell();
            break;
        }
        //запускаем класс действий после использования заклинания
        SpellEnding();  
    } 

    private void UseNonPhysicProjectileSpell(BookOfSpells.spell spell, float angle, Vector2 position)
    {
        GameObject projectile = Resources.Load<GameObject>("Spells/" + spell.name.ToString() + "/" + spell.name.ToString());
        if (projectile != null)
        {
            Vector3 placeInstantiate = transform.position;

            GameObject newSpellObject = Instantiate(projectile, placeInstantiate, transform.rotation * Quaternion.Euler(0f,0f,angle*(-1)));//Quaternion.Euler(Math.Abs(angle),angle,-90)

            SpellBehavior newSpellBehavior = newSpellObject.GetComponent<SpellBehavior>(); 
            Vector2 startPosition = new Vector2(position.x - gameObject.transform.position.x, position.y - gameObject.transform.position.y);
            newSpellBehavior.NonPhysicProjectile(spell, true, startPosition); 

            if(spell.selfImpact > 0)
                gameObject.GetComponent<HeroMove>().TakeImpact(spell.selfImpact, position);
        }       
    } 

    private void UseDirectedOnEnemySpell()
    {
        LootDroping lootDroping = interactionObject.GetComponent<LootDroping>();
        lootDroping.Drop();
    }

    private void ShowSpellInterface(string spellFullName, int spellBullets)
    {
        UsingSpellPanel.SetActive(true);  
        USName.GetComponent<TMP_Text>().text = spellFullName;
        USCount.GetComponent<TMP_Text>().text = spellBullets.ToString() + "/" + spellBullets.ToString(); 
    }  

    private void CloseSpellInterface()
    {
        UsingSpellPanel.SetActive(false);    
    }

    private void ChangeUsesCountMinus(int spellBullets)
    {
        USCount.GetComponent<TMP_Text>().text = usesRemain + "/" + spellBullets.ToString();
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
    /*private Vector3 SetMousePositionToFinalPoint()
    {
        Vector3 finalPoint;

        finalPoint.x = MouseTarget.transform.position.x;
        finalPoint.y = transform.position.y;
        finalPoint.z = MouseTarget.transform.position.z;

        return finalPoint;
    }   */ 

}
