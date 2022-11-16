using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyPotBehavior : MonoBehaviour
{
    InterfaceManager interfaceManager; Alchemy alchemy; PotionsScroll potionsScroll;
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        alchemy = interfaceManager.Alchemy.GetComponent<Alchemy>();

        potionsScroll = interfaceManager.Alchemy.GetComponent<PotionsScroll>();
    }

    public void UsePot()
    {       
        alchemy.OpenAlcGame();
        
        if (potionsScroll != null)
            potionsScroll.FillScroll();
        else
            Debug.LogError("potionsScroll = null");
    }
}
