using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class NewItemMessageScript : MonoBehaviour
{
    private InterfaceManager interfaceManager;
    private Inventory inventory;
    private GameObject prefabSlot; List<GameObject> prefabList;
    private GameObject NISlots, TrashButton, PackButton;

    void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>(); 
            NISlots = interfaceManager.NISlots;
                TrashButton = interfaceManager.TrashButton;
                    TrashButton.GetComponent<Button>().onClick.AddListener(delegate{    CloseMessege(true); });
                PackButton = interfaceManager.PackButton;
                    PackButton.GetComponent<Button>().onClick.AddListener(PackAll);
            inventory = interfaceManager.Inventory.GetComponent<Inventory>();

        prefabList = new List<GameObject>();
        
        prefabSlot = Resources.Load<GameObject>("2d_prefabs/ItemSlot");
    }

    private void PackAll()
    {
        foreach (var item in prefabList)
        {
            int slotIndex = inventory.FindEmptySlot();
            if (slotIndex != (-1))
            {
                inventory.inventorySlots[slotIndex].item = item;
                inventory.inventorySlots[slotIndex].item.transform.position = inventory.inventorySlots[slotIndex].slotObj.transform.position;
                prefabList.Remove(item); 
                SlotChange();                
            }    
            else
            {
                Debug.LogError("Больше нет места в инвентаре");
                break;
            }        
            
        }
    }

    public void ShowItemsMessege(List<Item> items)
    {
        FillItemView(items); 

        interfaceManager.NewItemsPanel.SetActive(true);
        interfaceManager.EquipmentPanel.SetActive(true);
        interfaceManager.InventoryPanel.SetActive(true);
        MainVariables.inInterface = true;
    }

    private void FillItemView(List<Item> items) 
    {
        foreach (Item item in items)
        {
            if (item != null)
            {
                GameObject newItem = Instantiate(prefabSlot, NISlots.transform);
                var gameObjectItem = newItem.transform.Find("Item").gameObject;
                    var itemData = gameObjectItem.GetComponent<ItemData>();
                        itemData.data = item;
                    var image = gameObjectItem.GetComponent<SVGImage>();
                        image.sprite = item.image;                

                prefabList.Add(newItem);
            }
        }
    }

    public void SlotChange()
    {
        if (prefabList.Count == 0)
            CloseMessege(false); 
    }

    private void CloseMessege(bool clear = false)
    {
        if (clear)
            prefabList.Clear(); 

        interfaceManager.NewItemsPanel.SetActive(false);
        interfaceManager.EquipmentPanel.SetActive(false);
        interfaceManager.InventoryPanel.SetActive(false);
        MainVariables.inInterface = false;
    }
}
