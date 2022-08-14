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
    
    private void Update() 
    {
        SetActiveSpell();
    }
    public void LateUpdate() 
    {
        if (MainVariables.inInterface == false)
        {
            BookOfSpells.FillSlot(1, "frostspell");
            if (Input.GetKeyUp(KeyCode.Alpha1) && String.IsNullOrEmpty(activeSpell.name) == false)
            {
                ShowSpellInterface(activeSpell.fullname, activeSpell.bullets);
                usesRemain = activeSpell.bullets;
                MainVariables.inSpelling = true;    
            }
        
            if (Input.GetMouseButtonDown(0) && UIClick.OnMouseDown() && MainVariables.inSpelling)
            {
                UseSpell(activeSpell);                            
            }
        }
    }

    public void UseSpell(BookOfSpells.spell activeSpell)
    {
        float angle = MyMathCalculations.CalculateAngle(transform.position, mousePosition.transform.position, transform.right); 

        switch(activeSpell.type)
        {
            case("NPP"): UseNonPhysicProjectileSpell(activeSpell.speed, activeSpell.distance, activeSpell.prefab, angle);
            break;
        }

        usesRemain -= 1;
        if (usesRemain < 1)
        {
            CloseSpellInterface();
            MainVariables.inSpelling = false; 
        } 
        else
            ChangeUsesCountMinus(activeSpell.bullets);      

    } 

    private void UseNonPhysicProjectileSpell(float speed, float distance, string stringProjectile, float angle)
    {
        GameObject projectile = Resources.Load<GameObject>("Spells/" + stringProjectile.ToString());
 
        Vector3 placeInstantiate = transform.position;

        GameObject newSpellObject = Instantiate(projectile, placeInstantiate, Quaternion.Euler(0,angle,-90));
        
        SpellBehavior newSpellBehavior = newSpellObject.GetComponent<SpellBehavior>(); 
        newSpellBehavior.NonPhysicProjectile(speed, distance, gameObject);        
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

    private void SetActiveSpell()
    {
        switch (activeSpellSlot)
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
