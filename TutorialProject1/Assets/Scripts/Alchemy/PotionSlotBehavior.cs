using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotionSlotBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Alchemy alchemy;
    Animator animator;
    [NonSerialized] public InfoWindow infoWindow;   float infoDelay = 0.3f;
    Transform itemInSlot; PotionData potionData;
        

    private void Start() 
    {
        Transform PotionSlots = transform.parent;
        Transform AlcPotionPanel = PotionSlots.transform.parent;
        Transform Alchemy = AlcPotionPanel.transform.parent;
        alchemy = Alchemy.GetComponent<Alchemy>();

        animator = gameObject.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (itemInSlot == null) itemInSlot = transform.Find("Potion(Clone)"); 
        if (potionData == null) potionData = itemInSlot.GetComponent<PotionData>();

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
        if (itemInSlot == null) itemInSlot = transform.Find("Potion(Clone)"); 
        if (potionData == null) potionData = itemInSlot.GetComponent<PotionData>();
        if (potionData != null)
        {
            Potion potion = potionData.data;

            infoWindow.HideWindow();
            alchemy.SetPotion(potion);
            alchemy.ResetAlcGame();
            alchemy.ChangeWindow(true);
        }
        else
            Debug.LogError("potionData = null");
    }

    private IEnumerator MouseOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(infoDelay); 
        infoWindow.ActivateWindow(potionData.data.potionName, potionData.data.info);      
    }
}
