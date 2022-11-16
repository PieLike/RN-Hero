using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillScroll : MonoBehaviour
{
    private GameObject scrollViewContent;
    private GameObject skillPrefab, SlotPrefab; List<Slot> prefabList;
    private InterfaceManager interfaceManager;  [NonSerialized] public SkillManager skillManager;
    Sprite lockIcon, unlockIcon;

    public class Slot
    {
        public GameObject skill, slot;
    }

    private void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>(); 
        skillManager = FindObjectOfType<SkillManager>();

        scrollViewContent = interfaceManager.SkillSlots;
        skillPrefab = Resources.Load<GameObject>("2d_prefabs/Skill");  
        SlotPrefab = Resources.Load<GameObject>("2d_prefabs/SkillSlot");    

        prefabList = new List<Slot>();  

        unlockIcon = Resources.Load<Sprite>("Sprites/UnlockIcon");
        lockIcon = Resources.Load<Sprite>("Sprites/LockIcon");
    }

    public void FillScroll() 
    {
        if (skillManager.skills == null) return;
        foreach (SkillManager.SkillState item in skillManager.skills)
        {
            if (item != null)
                AddScrollElement(item);
        }
    }

    public void AddScrollElement(SkillManager.SkillState item)
    {
        Slot newSlot = new Slot();
        newSlot.slot = Instantiate(SlotPrefab, scrollViewContent.transform);
            SkillSlotBehavior skillSlotBehavior = newSlot.slot.GetComponent<SkillSlotBehavior>();   
                skillSlotBehavior.infoWindow = interfaceManager.infoWindowScript;
        newSlot.skill = Instantiate(skillPrefab, newSlot.slot.transform);
            newSlot.skill.transform.SetSiblingIndex(0);
            //SVGImage image = newSlot.potion.GetComponent<SVGImage>();
            Image image = newSlot.skill.GetComponent<Image>();
            image.sprite = item.skill.image;
        SkillData skillData = newSlot.skill.GetComponent<SkillData>();
            skillData.data = item.skill;
            skillData.state = item.state;

        Transform lockObj; Image lockImage;
        switch (item.state)
        {
            case SkillData.State.locked:
                lockObj = newSlot.slot.transform.Find("Lock");
                lockImage = lockObj.GetComponent<Image>();
                lockImage.enabled = true;
                lockImage.sprite = lockIcon;
                break;
            case SkillData.State.unlocked:
                lockObj = newSlot.slot.transform.Find("Lock");
                lockImage = lockObj.GetComponent<Image>();
                lockImage.enabled = true;
                lockImage.sprite = unlockIcon;
                break;
            default:
                break;
        }
        
        
        prefabList.Add(newSlot);
    }

    public void ClearScroll()   //выполнять на выходе
    {
        foreach (var item in prefabList)
        {
            Destroy(item.skill);
            Destroy(item.slot);
        }
        prefabList.Clear();
    }    
}
