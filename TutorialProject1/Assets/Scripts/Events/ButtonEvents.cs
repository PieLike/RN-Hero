using UnityEngine;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{
    private Button saveButton, loadButton, vocalabraryButton, dialogueContunueButton;
    private GameObject objVoculabrary, objDialogueSystem; Voculabrary voculabrary; DialogueSystem dialogueSystem;
    
    void Start()
    {
        GameObject objInterface = GameObject.Find("Interface");

        objVoculabrary = objInterface.transform.Find("Voculabrary").gameObject;
        voculabrary = objVoculabrary.GetComponent<Voculabrary>();

        objDialogueSystem = objInterface.transform.Find("DialogueSystem").gameObject;
        dialogueSystem = objDialogueSystem.GetComponent<DialogueSystem>();

        saveButton = objInterface.transform.Find("Menu/SaveButton").gameObject.GetComponent<Button>();
        saveButton.onClick.AddListener(PressSaveButton);  

        loadButton = objInterface.transform.Find("Menu/LoadButton").gameObject.GetComponent<Button>(); 
        loadButton.onClick.AddListener(PressLoadButton);  

        vocalabraryButton = objInterface.transform.Find("MainInterface/VoculabraryButton").gameObject.GetComponent<Button>(); 
        vocalabraryButton.onClick.AddListener(PressVoculabraryButton);  

        dialogueContunueButton = objDialogueSystem.transform.Find("DialoguePanel/DialogueContinue").gameObject.GetComponent<Button>(); 
        dialogueContunueButton.onClick.AddListener(PressDialogueContunue);    
    }

    private void PressSaveButton()
    {
        WorkWithSaves.CreateSave();
    }
    private void PressLoadButton()
    {
        WorkWithSaves.LoadSave();
    }
    private void PressVoculabraryButton()
    {
        voculabrary.VoculabraryButton();    
    }
    private void PressDialogueContunue()
    {
        dialogueSystem.DisplayNextSentence();    
    }
}
