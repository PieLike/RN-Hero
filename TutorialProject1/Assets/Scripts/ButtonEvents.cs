using UnityEngine;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{
    private Button saveButton, loadButton;
    
    void Start()
    {
        saveButton = GameObject.Find("Interface/Menu/SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(PressSaveButton);  

        loadButton = GameObject.Find("Interface/Menu/LoadButton").GetComponent<Button>(); 
        loadButton.onClick.AddListener(PressLoadButton);   
    }

    private void PressSaveButton()
    {
        WorkWithSaves.CreateSave();
    }

    private void PressLoadButton()
    {
        WorkWithSaves.LoadSave();
    }
}
