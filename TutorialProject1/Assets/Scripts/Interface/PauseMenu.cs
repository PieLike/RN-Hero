using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    InterfaceManager interfaceManager;
    GameObject pausePanel;

    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        pausePanel = interfaceManager.PauseMenuPanel;

        Transform ExitButton = pausePanel.transform.Find("ExitButton");
        ExitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    void ShowPanel()
    {
        pausePanel.SetActive(true);
        MainVariables.inInterface = true;
        Time.timeScale = 0f;
    }
    void HidePanel()
    {
        pausePanel.SetActive(false);
        MainVariables.inInterface = false;
        Time.timeScale = 1f;
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape) && MainVariables.inInterface == false)
        {
            if (pausePanel.activeSelf == false)
                ShowPanel();
            else
                HidePanel();
        }
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
