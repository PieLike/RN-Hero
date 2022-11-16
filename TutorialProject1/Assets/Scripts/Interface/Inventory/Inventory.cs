using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [NonSerialized] public InterfaceManager interfaceManager; GameObject InvSlots;
    private GameObject prefabSlot, prefabItem;
    public List<Slot> inventorySlots; public Slot meleeSlot, armorSlot;
    [SerializeField]private MeleeAttack meleeAttack;

    public class Slot
    {
        public GameObject slotObj, item;
    }

    private void Start() 
    {
        prefabSlot = Resources.Load<GameObject>("2d_prefabs/InvSlot");
        prefabItem = Resources.Load<GameObject>("2d_prefabs/Item");

        interfaceManager = FindObjectOfType<InterfaceManager>();    
            InvSlots = interfaceManager.InvSlots;

        meleeAttack = FindObjectOfType<MeleeAttack>();//.GetComponent<MelleAttack>();    

        inventorySlots  = new List<Slot>(HeroMainData.inventorySlotCount);
        meleeSlot       = new Slot{ slotObj = interfaceManager.WeaponSlot};
        armorSlot       = new Slot{ slotObj = interfaceManager.ArmorSlot};

        AddSlots();

            meleeSlot.item = Instantiate(prefabItem, meleeSlot.slotObj.transform);   //для прототипа или сейва            
            ItemData itemData = meleeSlot.item.GetComponent<ItemData>();
            if (itemData != null)
            {
                itemData.data = Resources.Load<Item>("WeaponsMelle/Paddle");
                meleeSlot.item.GetComponent<SVGImage>().sprite = itemData.data.image;
            }
        MeleeUpdate();
    }

    public void AddSlots() 
    {        
        foreach (Transform child in InvSlots.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var slot in inventorySlots)
        {
            slot.slotObj = Instantiate(prefabSlot, InvSlots.transform);
        }
    }
    public void InventoryButton()
    {
        if(interfaceManager.InventoryPanel.activeSelf == false)
        {
            interfaceManager.InventoryPanel.SetActive(true);    
            interfaceManager.EquipmentPanel.SetActive(true); 
            MainVariables.inInterface = true;       
        }
        else
        {
            interfaceManager.InventoryPanel.SetActive(false); 
            interfaceManager.EquipmentPanel.SetActive(false); 
            MainVariables.inInterface = false;
        }
    }

    public void MeleeChange(GameObject item = null)
    {
        meleeSlot.item = item;
    }
    public void MeleeUpdate()
    {

        if (meleeSlot.item != null)//не пустой слот
        {
            ItemData weapon = meleeSlot.item.GetComponent<ItemData>();
            if (weapon != null && weapon.data.itemType == ItemData.ItemType.weapon)
            {
                //meleeAttack.damage      = weapon.data.damage;
                //meleeAttack.radius      = weapon.data.radiusAttack;
                //meleeAttack.speedAttack = weapon.data.speedAttack;
                meleeAttack.weapon = weapon.data;
                meleeAttack.UpdateRadius();
                meleeAttack.UpdateWeaponSprite(weapon.data.sprite);
                meleeAttack.enable = true;
            }
        }
        else
            meleeAttack.enable = false;        
    }

    public int FindEmptySlot()
    {
        for (var i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].slotObj != null && inventorySlots[i].item == null)
            {
                return i;
            }
        }

        return -1;
    }
}
