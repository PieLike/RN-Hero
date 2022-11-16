using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionWheelControl : MonoBehaviour
{
    //float buttonHoldTime = 0.1f, buttomPressTime;   bool readyToShow;
    InterfaceManager interfaceManager; PotionManager potionManager; PickerWheelPotions pickerWheelComponent; Camera cam;
    void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        potionManager = FindObjectOfType<PotionManager>();
        pickerWheelComponent = interfaceManager.PotionsPickerWheel.GetComponent<PickerWheelPotions>();
        pickerWheelComponent.Start();

        cam = Camera.main;
    }

    void Update()
    {
        /*if (readyToShow == false && UIClick.OnMouseDown() && Input.GetMouseButton(1))
        {
            buttomPressTime = Time.time + buttonHoldTime;
            readyToShow = true;
        }
        if (readyToShow == true && Time.time >= buttomPressTime && Input.GetMouseButton(1))
        {
            ShowPickerWheel();
            readyToShow = false;
        }*/
        if (UIClick.OnMouseDown() && Input.GetMouseButton(1) && MainVariables.inInterface == false)
        {
            ShowPickerWheel();
            TimeManipulation.DoSlowmotion(0.05f);
        }
        if (Input.GetMouseButtonUp(1))
        {
            //readyToShow = false;
            HidePickerWheel();
            TimeManipulation.EndSlowmotion();
        }
    }

    void ShowPickerWheel()
    {
        if (interfaceManager.PCPanel.activeSelf == false)
        {
            interfaceManager.PCPanel.SetActive(true);

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            interfaceManager.PotionsPickerWheel.transform.position = new Vector3(mousePos.x, mousePos.y, interfaceManager.PotionsPickerWheel.transform.position.z);

            if (pickerWheelComponent != null)
            {
                List<PotionManager.ActivePotion> potions = potionManager.activePotions;
                if (potions != null && potions.Count >= 1)
                {                    
                    pickerWheelComponent.FillWheelPieces(potions);
                }
            }
            else
                Debug.LogError("Не получается найти pickerWheelComponent");
            
            interfaceManager.PotionsPickerWheel.SetActive(true);
        } 
    }

    void HidePickerWheel()
    {
        if (interfaceManager.PCPanel.activeSelf)
        {
            interfaceManager.PCPanel.SetActive(false);
            interfaceManager.PotionsPickerWheel.SetActive(false);
            pickerWheelComponent.ClearWheel();
        } 
    }    
}
