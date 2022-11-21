using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyPotBehavior : MonoBehaviour
{
    InterfaceManager interfaceManager; Alchemy alchemy; PotionsScroll potionsScroll;
    [NonSerialized] public bool canUse; Animator interactionAnimator;
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        alchemy = interfaceManager.Alchemy.GetComponent<Alchemy>();

        potionsScroll = interfaceManager.Alchemy.GetComponent<PotionsScroll>();

        Transform interactButton = transform.Find("InteractButton");
        interactionAnimator = interactButton.GetComponent<Animator>();
    }

    public void UsePot()
    {       
        alchemy.OpenAlcGame();
        
        if (potionsScroll != null)
        {
            potionsScroll.FillScroll();
            if (potionsScroll.potionManager.potions.Count == 0)
            {
                Transform text =  interfaceManager.AlcPotionPanel.transform.Find("Text");
                text.gameObject.SetActive(true);
                text.GetComponent<TMPro.TMP_Text>().text = "Вы пока еще не умеете создавать зелья";

                OnGameStartAndUpdate.OnInterfaceClose += UnActiveText;
            }
        }
        else
            Debug.LogError("potionsScroll = null");

        canUse = false;
    }

    public void UnActiveText()
    {
        Transform text =  interfaceManager.AlcPotionPanel.transform.Find("Text");
        text.gameObject.SetActive(false);
        OnGameStartAndUpdate.OnInterfaceClose -= UnActiveText;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.tag == "HeroUse" && canUse == false)
        {
            interactionAnimator.SetBool("Show", true);
            OnGameStartAndUpdate.AddInteraction(UsePot);
            canUse = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "HeroUse" && canUse)
        {
            interactionAnimator.SetBool("Show", false);
            OnGameStartAndUpdate.RemoveInteraction(UsePot);
            canUse = false;
        }
    }
    /*private void Update() 
    {
        if (canUse && Input.GetKeyDown(KeyCode.E) && MainVariables.inInterface == false)
        {
            UsePot();
        }
    }*/
}
