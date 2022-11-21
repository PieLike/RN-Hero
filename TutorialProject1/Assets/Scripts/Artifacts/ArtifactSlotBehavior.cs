using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArtifactSlotBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Alchemy alchemy;
    Animator animator;
    [NonSerialized] public InfoWindow infoWindow;   float infoDelay = 0.3f;
    Transform itemInSlot; ArtifactData artifactData;
        

    private void Start() 
    {
        /*Transform PotionSlots = transform.parent;
        Transform AlcPotionPanel = PotionSlots.transform.parent;
        Transform Alchemy = AlcPotionPanel.transform.parent;
        alchemy = Alchemy.GetComponent<Alchemy>();*/

        animator = gameObject.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (itemInSlot == null) itemInSlot = transform.Find("Artifact(Clone)"); 
        if (artifactData == null) artifactData = itemInSlot.GetComponent<ArtifactData>();

        animator.SetBool("bigSize", true);

        StartCoroutine(MouseOverCoroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("bigSize", false);

        StopAllCoroutines();
        infoWindow.HideWindow();
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        
    }

    private IEnumerator MouseOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(infoDelay); 
        infoWindow.ActivateWindow(artifactData.data.artName, artifactData.data.info);      
    }
}
