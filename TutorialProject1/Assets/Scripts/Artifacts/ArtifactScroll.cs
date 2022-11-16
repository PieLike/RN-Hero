using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactScroll : MonoBehaviour
{
    private GameObject scrollViewContent;
    private GameObject artifactPrefab, SlotPrefab; List<Slot> prefabList;
    private InterfaceManager interfaceManager;  ArtifactManager artifactManager;

    public class Slot
    {
        public GameObject artifact, slot;
    }

    private void Start() //Start
    {
        interfaceManager = FindObjectOfType<InterfaceManager>(); 
        artifactManager = FindObjectOfType<ArtifactManager>();

        scrollViewContent = interfaceManager.ArtifactSlots;
        artifactPrefab = Resources.Load<GameObject>("2d_prefabs/Artifact"); 
        SlotPrefab = Resources.Load<GameObject>("2d_prefabs/ArtifactSlot");    

        prefabList = new List<Slot>();  
    }

    public void FillScroll() 
    {
        if (artifactManager.artifacts == null) return;
        
        foreach (var artifact in artifactManager.artifacts)
        {
            if (artifact != null && artifact.item != null)
            {
                AddScrollElement(artifact);
            }
        }
    }

    public void AddScrollElement(ArtifactManager.ArtifactInManager artifact)
    {
        Slot newSlot = new Slot();
        newSlot.slot = Instantiate(SlotPrefab, scrollViewContent.transform);
            ArtifactSlotBehavior artifactSlotBehavior = newSlot.slot.GetComponent<ArtifactSlotBehavior>();   
                artifactSlotBehavior.infoWindow = interfaceManager.infoWindowScript;
        newSlot.artifact = Instantiate(artifactPrefab, newSlot.slot.transform);
            //SVGImage image = newSlot.potion.GetComponent<SVGImage>();
            Image image = newSlot.artifact.GetComponent<Image>();
            image.sprite = artifact.item.image;
        ArtifactData artifactData = newSlot.artifact.GetComponent<ArtifactData>();
            artifactData.data = artifact.item;
        
        prefabList.Add(newSlot);
    }

    public void ClearScroll()   //выполнять на выходе
    {
        foreach (var item in prefabList)
        {
            Destroy(item.artifact);
            Destroy(item.slot);
        }
        prefabList.Clear();
    }
}
