using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactMenu : MonoBehaviour
{
    private GameObject ArExit;
    InterfaceManager interfaceManager;
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();

        //находим дочерние объекты        
        ArExit = interfaceManager.ArExit;
            ArExit.GetComponent<Button>().onClick.AddListener(CloseWindow); 
    }
    public void ArtifactButton()
    {
        if(interfaceManager.ArtifactsPanel.activeSelf == false)
        {
            OpenWindow();                  
        }
        else
        {
            CloseWindow();            
        }
    }
    private void OpenWindow()
    {
        OnGameStartAndUpdate.CloseInterface();
        gameObject.GetComponent<ArtifactScroll>().FillScroll();
        interfaceManager.ArtifactsPanel.SetActive(true);    
        //MainVariables.inInterface = true;
        Time.timeScale = 0f; 
        OnGameStartAndUpdate.StatsUpdate();
        OnGameStartAndUpdate.OnInterfaceClose += CloseWindow;
    }
    private void CloseWindow()
    {
        gameObject.GetComponent<ArtifactScroll>().ClearScroll();
        interfaceManager.ArtifactsPanel.SetActive(false); 
        //MainVariables.inInterface = false;
        Time.timeScale = 1f; 
        OnGameStartAndUpdate.OnInterfaceClose -= CloseWindow;   
    }   
}
