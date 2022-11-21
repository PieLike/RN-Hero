using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class PotionsScroll : MonoBehaviour
{
    private GameObject scrollViewContent;
    private GameObject potionPrefab, SlotPrefab; List<Slot> prefabList;
    private InterfaceManager interfaceManager;  [HideInInspector] public PotionManager potionManager;

    public class Slot
    {
        public GameObject potion, slot;
    }

    private void Start() //Start
    {
        interfaceManager = FindObjectOfType<InterfaceManager>(); 
        potionManager = FindObjectOfType<PotionManager>();

        scrollViewContent = interfaceManager.PotionSlots;
        potionPrefab = Resources.Load<GameObject>("2d_prefabs/Potion");  
        SlotPrefab = Resources.Load<GameObject>("2d_prefabs/PotionSlot");    

        prefabList = new List<Slot>();  
    }

    public void FillScroll() 
    {
        if (potionManager.potions != null)
        {
            foreach (var item in potionManager.potions)
            {
                if (item != null)
                    AddScrollElement(item);
            }
        }
    }

    public void AddScrollElement(Potion item)
    {
        Slot newSlot = new Slot();
        newSlot.slot = Instantiate(SlotPrefab, scrollViewContent.transform);
            PotionSlotBehavior potionSlotBehavior = newSlot.slot.GetComponent<PotionSlotBehavior>();   
                potionSlotBehavior.infoWindow = interfaceManager.infoWindowScript;
        newSlot.potion = Instantiate(potionPrefab, newSlot.slot.transform);
            //SVGImage image = newSlot.potion.GetComponent<SVGImage>();
            Image image = newSlot.potion.GetComponent<Image>();
            image.sprite = item.image;
        PotionData potionData = newSlot.potion.GetComponent<PotionData>();
            potionData.data = item;
        
        prefabList.Add(newSlot);
    }

    public void ClearScroll()   //выполнять на выходе
    {
        foreach (var item in prefabList)
        {
            Destroy(item.potion);
            Destroy(item.slot);
        }
        prefabList.Clear();
    }
}
