using UnityEngine;
using System;
using TMPro;

public class Spells : MonoBehaviour
{
    private GameObject mouseTarget; 
    private GameObject objEnglishGame, prefabDirectionPlane, directionPlane;    
    private GameObject USInterface, UsingSpellPanel, USName, USCount;
    //[NonSerialized] public BookOfSpells.spell activeSpell;
    [NonSerialized] public ScriptableObjSpell activeSpell;
    //private int usesRemain = 0;
    private bool isWaitingEnemy = false;
    private GameObject interactionObject;
    float lastSpell;
    private InterfaceManager interfaceManager; MeleeAttack meleeAttack;
    public bool spellEnabled; private float timeSpellDisable;

    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        //находим объекты на сцене
        USInterface = interfaceManager.UsingSpell;
        objEnglishGame = interfaceManager.EnglishGame;

        mouseTarget = GameObject.Find("MouseTarget");
        //находим дочерние объекты
        UsingSpellPanel = interfaceManager.UsingSpellPanel;
        USName = interfaceManager.USName;
        USCount = interfaceManager.USCount;

        //находим ресурсы (и создаем на сцене)
        prefabDirectionPlane = Resources.Load<GameObject>("3d_prefabs/DirectionPlane");
            directionPlane = Instantiate(prefabDirectionPlane);
            directionPlane.SetActive(false); 

        Transform melleAttackObj = transform.Find("MeleeAttack");
            meleeAttack = melleAttackObj.GetComponent<MeleeAttack>();

        //находим активный спелл по номеру активного слота заклинания 
        //SetActiveSpell(MainVariables.activeSpellSlot);       
    }    
    private void Update() 
    {
        if (interactionObject != MousePosition2D.supposedInteractionObject)
            interactionObject = MousePosition2D.supposedInteractionObject;
        
        //если спрайт направления активен то задаем ему направление и положение
        if (directionPlane.activeSelf == true) 
        {
            float angle = MyMathCalculations.CalculateAngle2D(transform.position, mouseTarget.transform.position, transform.right) * (-1);
            directionPlane.transform.rotation = Quaternion.Slerp(directionPlane.transform.rotation, Quaternion.Euler(0f,0f,angle), 1f);

            directionPlane.transform.position = new Vector2(transform.position.x, transform.position.y);
        }  

        if (spellEnabled)
        {
            if (Time.time > timeSpellDisable) 
            {
                DisableSpell();
            }
        }      
    }
    public void DisableSpell()
    {
        UsingSpellPanel.SetActive(false);
        MainVariables.inSpelling = false;
        directionPlane.SetActive(false);

        spellEnabled = false;
        timeSpellDisable = 0f;
    }
    
    public void LateUpdate() 
    {
        if (MainVariables.inInterface == false)
        {
            //если режим заклинания активен
            if (MainVariables.inSpelling)
            {
                //используем заклинание по клику мыши
                if (Input.GetMouseButton(0) && UIClick.OnMouseDown() && meleeAttack.propEnemy.Count == 0 && meleeAttack.propWalls.Count == 0)
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
                    //usesRemain = 1;
                    //SpellEnding();  
                    DisableSpell(); 
                }                           
            }
        }
        //очищаем слот наведенного объекта
        interactionObject = null;
    }

    public void SpellBegining(float duration)
    {
        //если активное заклинание есть
        if(String.IsNullOrEmpty(activeSpell.name) == false)
        {
            //то что будет выполнятся в начале активного использорвания заклинаний
            //показываем интерфейс режим заклинания, записываем количество оставшихся использований, включаем режим заклинаний, отображаем спрайт указателя
            ShowSpellInterface(activeSpell.fullname, activeSpell.bullets);
            //usesRemain = activeSpell.bullets;
            MainVariables.inSpelling = true;
            directionPlane.SetActive(true); 

            if (activeSpell.type == ScriptableObjSpell.SpellType.DirectedOnEnemy) //|| activeSpell.type == "DH")
                isWaitingEnemy = true;
            else
                isWaitingEnemy = false;

            spellEnabled = true;
            timeSpellDisable = timeSpellDisable == 0f ? Time.time + duration : timeSpellDisable + duration;
        }
    }

    /*private void SpellEnding()
    {
        //то что будет выполнятся в конце каждого использорвания заклинания
        //если использований больше не осталось, то выполнится всё, что заканчивает режим активного использования заклинаний
        //usesRemain -= 1;
        //if (usesRemain < 1)
        //{
            UsingSpellPanel.SetActive(false);
            MainVariables.inSpelling = false;
            directionPlane.SetActive(false); 
        //} 
        //else
        //    ChangeUsesCountMinus(activeSpell.bullets);             
    }*/

    public void UseSpell(ScriptableObjSpell activeSpell)    //BookOfSpells.spell
    {
        float angle;
        //находим тип заклинания и зупаскаем соответсвующий класс
        switch(activeSpell.type)
        {
            case(ScriptableObjSpell.SpellType.NonPhysicProjectile): //Non Physic Projectile
                //находим угол между вектором +X и вектором от героя до курсора                
                angle = MyMathCalculations.CalculateAngle2D(transform.position, mouseTarget.transform.position, transform.up);
                UseNonPhysicProjectileSpell(activeSpell, angle, mouseTarget.transform.position);
            break;

            case(ScriptableObjSpell.SpellType.DirectedOnEnemy): //Directed on Enemy
                UseDirectedOnEnemySpell();
            break;
        }
        //запускаем класс действий после использования заклинания
        //SpellEnding();  
    } 

    private void UseNonPhysicProjectileSpell(ScriptableObjSpell spell, float angle, Vector2 position)   //BookOfSpells.spell
    {
        GameObject projectile = Resources.Load<GameObject>("Spells/" + spell.name.ToString() + "/" + spell.name.ToString());
        if (projectile != null)
        {
            GameObject newSpellObject = Instantiate(projectile, transform.position, transform.rotation * Quaternion.Euler(0f,0f,angle*(-1)));//Quaternion.Euler(Math.Abs(angle),angle,-90)

            SpellBehavior newSpellBehavior = newSpellObject.GetComponent<SpellBehavior>(); 
                newSpellBehavior.frendly = true;
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
        //USCount.GetComponent<TMP_Text>().text = spellBullets.ToString() + "/" + spellBullets.ToString(); 
    }  

   /* private void ChangeUsesCountMinus(int spellBullets)
    {
        USCount.GetComponent<TMP_Text>().text = usesRemain + "/" + spellBullets.ToString();
    }*/
}
