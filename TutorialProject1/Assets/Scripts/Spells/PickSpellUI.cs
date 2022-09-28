using UnityEngine;
using UnityEngine.UI;
using System;
[RequireComponent(typeof(Spells))]


public class PickSpellUI : MonoBehaviour
{
    private GameObject SpellSlot1, SpellSlot2, SpellSlot3, SpellSlot4, SpellSlot5, SpellSlot6, SpellSlot7, SpellSlot8, SpellSlot9;
    private GameObject PickSpell, SpellPickSquere;
    private static BookOfSpells.spell activeSpell;
    private RectTransform rectTransform; 
    private int previousActiveSpellSlot = 1;  

    private ScriptableObjSpell FindSpellSO(string spellName)
    {
        ScriptableObjSpell spell = Resources.Load<ScriptableObjSpell>("Spells/" + spellName + "/" + spellName); 
        return spell;  
    }  

    private void Start() 
    {
        PickSpellUI pickSpellUI = gameObject.GetComponent<PickSpellUI>();


        BookOfSpells.FillSlotBySO (1, FindSpellSO("frostspell")); //потом убрать
        //pickSpellUI.FillSpellSlotIcon(1);
        BookOfSpells.FillSlotBySO(2, FindSpellSO("recognizespell")); //потом убрать
        //pickSpellUI.FillSpellSlotIcon(2);
        BookOfSpells.FillSlotBySO(3, FindSpellSO("fireballspell")); //потом убрать
        //pickSpellUI.FillSpellSlotIcon(3);

        //находим объекты на сцене
        PickSpell = GameObject.Find("Interface/MainInterface/PickSpell");
        //находим дочерние объекты
        SpellPickSquere = PickSpell.transform.Find("SpellPickSquere").gameObject;
        SpellSlot1 = PickSpell.transform.Find("SpellSlot1").gameObject;
        SpellSlot2 = PickSpell.transform.Find("SpellSlot2").gameObject;
        SpellSlot3 = PickSpell.transform.Find("SpellSlot3").gameObject;
        SpellSlot4 = PickSpell.transform.Find("SpellSlot4").gameObject;
        SpellSlot5 = PickSpell.transform.Find("SpellSlot5").gameObject;
        SpellSlot6 = PickSpell.transform.Find("SpellSlot6").gameObject;
        SpellSlot7 = PickSpell.transform.Find("SpellSlot7").gameObject;
        SpellSlot8 = PickSpell.transform.Find("SpellSlot8").gameObject;
        SpellSlot9 = PickSpell.transform.Find("SpellSlot9").gameObject;

        //заполняем все слоты заклинаний
        for (int i = 1; i <= 9; i++)
        {
            FillSpellSlotIcon(i);
        }

        //получаем трансофрм квадратика выбора зкалинания чтобы потом двигать
        //и один раз меняем
        rectTransform = SpellPickSquere.GetComponent<RectTransform>();
        ChangeSquereSlotPick();            
    }

    private void Update() 
    {       
        //если активный слот заклинания изменился то двигаем квадратик
        if (previousActiveSpellSlot != MainVariables.activeSpellSlot)
        {
            ChangeSquereSlotPick();
            previousActiveSpellSlot = MainVariables.activeSpellSlot;
        }            
    }

    private void ChangeSquereSlotPick()
    {        
        //двигаем квадратик выбора заклинания в нижней панели в соотвествии с активным слотом игрока        
        switch(MainVariables.activeSpellSlot)
        {   
            case(1): rectTransform.position = (new Vector3(SpellSlot1.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(2): rectTransform.position = (new Vector3(SpellSlot2.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(3): rectTransform.position = (new Vector3(SpellSlot3.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(4): rectTransform.position = (new Vector3(SpellSlot4.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(5): rectTransform.position = (new Vector3(SpellSlot5.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(6): rectTransform.position = (new Vector3(SpellSlot6.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(7): rectTransform.position = (new Vector3(SpellSlot7.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(8): rectTransform.position = (new Vector3(SpellSlot8.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
            case(9): rectTransform.position = (new Vector3(SpellSlot9.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;

            default: rectTransform.position = (new Vector3(SpellSlot1.GetComponent<RectTransform>().position.x,rectTransform.position.y,rectTransform.position.z));
            break;
        }
    }

    public void FillSpellSlotIcon(int slotNumber)
    {
        SetActiveSpell(slotNumber); 
        
        //если заклинание есть
        if(String.IsNullOrEmpty(activeSpell.name) == false)
        {
            Sprite icon;

            //Debug.Log("icon" + activeSpell.name.ToString());
            icon = Resources.Load<Sprite>("Spells/icon" + activeSpell.name.ToString() + "/" + activeSpell.name.ToString());
            if (icon == null)
            {
                icon = Resources.Load<Sprite>("Spells/iconerror");
                if (icon == null)
                {
                    Debug.Log("программа не может найти иконки для заклинаний");
                }
            }            
            
            Image image;

            switch (slotNumber)
            {
                case(1): image = SpellSlot1.GetComponent<Image>();
                    image.sprite = icon; 
                break;
                case(2): image = SpellSlot2.GetComponent<Image>();
                    image.sprite = icon;   
                break; 
                case(3): image = SpellSlot3.GetComponent<Image>();
                    image.sprite = icon;   
                break; 
                case(4): image = SpellSlot4.GetComponent<Image>();
                    image.sprite = icon; 
                break; 
                case(5): image = SpellSlot5.GetComponent<Image>();
                    image.sprite = icon;  
                break; 
                case(6): image = SpellSlot6.GetComponent<Image>();
                    image.sprite = icon; 
                break; 
                case(7): image = SpellSlot7.GetComponent<Image>();
                    image.sprite = icon;  
                break;
                case(8): image = SpellSlot8.GetComponent<Image>();
                    image.sprite = icon; 
                break; 
                case(9): image = SpellSlot9.GetComponent<Image>();
                    image.sprite = icon; 
                break;    
                
                default: image = SpellSlot1.GetComponent<Image>();
                    image.sprite = icon; 
                break;
            }

            //если прозрачность была включена то  выключаем
            if (image.color.a == 0f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);   
            }
        }
        else
            ClearSpellSlotIcon(slotNumber);
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

    private void ClearSpellSlotIcon(int slotNumber)
    {
        Image image;
        switch (slotNumber)
        {
            case(1): image = SpellSlot1.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
            case(2): image = SpellSlot2.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);  
            break;
            case(3): image = SpellSlot3.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
            case(4): image = SpellSlot4.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
            case(5): image = SpellSlot5.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
            case(6): image = SpellSlot6.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
            case(7): image = SpellSlot7.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
            case(8): image = SpellSlot8.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
            case(9): image = SpellSlot9.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;    
            
            default: image = SpellSlot1.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); 
            break;
        }
    }
    



}
