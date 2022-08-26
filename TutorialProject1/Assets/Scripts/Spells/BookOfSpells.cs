//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;

public class BookOfSpells : MonoBehaviour
{
    [NonSerialized] public static spell slot1data, slot2data, slot3data, slot4data, slot5data, slot6data, slot7data, slot8data, slot9data;
    private static string generalSpellDataBaseName = "spellBook.bytes";
    [SerializeField] private GameObject pickSpellUI; //нижняя панель выбора заклинания

    public struct spell
    {
        public string name, fullname, type;
        public float speed, distance, damage; 
        public int bullets;
    }

    public static void FillSlot(int slotNumber, string spellName)
    {
        spell activeSlot = new spell();
        
        string query = ($"SELECT * FROM spells WHERE name = '{spellName}';");
        DataTable spellFromBook = WorkWithDataBase.GetTable(query, generalSpellDataBaseName); 

        foreach (DataRow row in spellFromBook.Rows)
        {
            activeSlot.name = row["name"].ToString();
            activeSlot.speed = Convert.ToSingle(row["speed"]);
            activeSlot.distance = Convert.ToSingle(row["distance"]);
            activeSlot.fullname = row["fullname"].ToString();
            activeSlot.type = row["type"].ToString();
            activeSlot.bullets = Convert.ToInt32(row["bullets"]); 
            activeSlot.damage = Convert.ToSingle(row["damage"]);   
        } 

        switch (slotNumber)
        {
            case(1): slot1data = activeSlot; 
            break;
            case(2): slot2data = activeSlot; 
            break; 
            case(3): slot3data = activeSlot; 
            break; 
            case(4): slot4data = activeSlot; 
            break; 
            case(5): slot5data = activeSlot; 
            break; 
            case(6): slot6data = activeSlot; 
            break; 
            case(7): slot7data = activeSlot; 
            break;
            case(8): slot8data = activeSlot; 
            break; 
            case(9): slot9data = activeSlot; 
            break;     

            default: slot1data = activeSlot;
            break;
        }
        //НЕ ЗАБЫТЬ ЗАПОЛНИТЬ PickSpellUI.FillSpellSlotIcon(slotNumber); ПОСЛЕ ВЫЗОВЫ ЭТОГО МЕТОДА
    }


        
}
