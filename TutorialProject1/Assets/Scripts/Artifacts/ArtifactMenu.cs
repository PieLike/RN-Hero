using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactMenu : MonoBehaviour
{
    private GameObject ArPanel, ArExit;
    InterfaceManager interfaceManager;
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();

        //находим дочерние объекты        
        ArPanel = interfaceManager.ArtifactsPanel;         
        ArExit = interfaceManager.ArExit;
            ArExit.GetComponent<Button>().onClick.AddListener(Close); 
    }
    public void ArtifactButton()
    {
        if(interfaceManager.ArtifactsPanel.activeSelf == false)
        {
            gameObject.GetComponent<ArtifactScroll>().FillScroll();
            interfaceManager.ArtifactsPanel.SetActive(true);    
            MainVariables.inInterface = true; 
            OnGameStartAndUpdate.StatsUpdate();      
        }
        else
        {
            gameObject.GetComponent<ArtifactScroll>().ClearScroll();
            interfaceManager.ArtifactsPanel.SetActive(false); 
            MainVariables.inInterface = false;
        }
    }

    public void Close()
    {
        gameObject.GetComponent<ArtifactScroll>().ClearScroll();
        ArPanel.SetActive(false);    
    }   
}
