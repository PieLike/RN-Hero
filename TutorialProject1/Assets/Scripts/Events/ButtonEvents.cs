using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{
    private GameObject objDialogueSystem; private DialogueSystem dialogueSystem;
    private SaveManager saveManager;
    public Button dialogueContunueButton;
    InterfaceManager interfaceManager;
    void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        saveManager = FindObjectOfType<SaveManager>();

        GameObject objInterface = GameObject.Find("Interface");
        
        //кнпока сохранения
        Button saveButton = objInterface.transform.Find("Menu/SaveButton").gameObject.GetComponent<Button>();
        saveButton.onClick.AddListener(() => saveManager.CreateSave());  

        //кнопка загрузки
        Button loadButton = objInterface.transform.Find("Menu/LoadButton").gameObject.GetComponent<Button>(); 
        loadButton.onClick.AddListener(() => saveManager.LoadSave());  

        //кнопка вызова словаря
        GameObject objVoculabrary = objInterface.transform.Find("Voculabrary").gameObject;
        Voculabrary voculabrary = objVoculabrary.GetComponent<Voculabrary>();

        Button vocalabraryButton = objInterface.transform.Find("MainInterface/VoculabraryButton").gameObject.GetComponent<Button>(); 
        vocalabraryButton.onClick.AddListener(() => voculabrary.VoculabraryButton());  

        //кнопка вызова правил
        Button rulesButton = objInterface.transform.Find("MainInterface/RulesButton").gameObject.GetComponent<Button>(); 
        rulesButton.onClick.AddListener(() => GetTranslate());  

        //кнопка следующего предложения диалога на окошке диалога с нпс
        objDialogueSystem = objInterface.transform.Find("DialogueSystem").gameObject;
        dialogueSystem = objDialogueSystem.GetComponent<DialogueSystem>();   

        FundDialogueButton();
        FundChoiceButton();         
    }

    public void FundDialogueButton()
    {
        //if(dialogueContunueButton != null) return;
        dialogueContunueButton = objDialogueSystem.transform.Find("DialoguePanel2/DialogueContinue").gameObject.GetComponent<Button>(); 
        dialogueContunueButton.onClick.AddListener(() => { dialogueSystem.DisplayNextSentence();});  
    }
    public void FundChoiceButton()
    {
        //кнопки выбора диалога  
        Button[] choiceButtons = new Button[3];
        /*for (int i = 0; i < 3; i++)
        {
            choiceButtons[i] = objDialogueSystem.transform.Find($"DialogueChoicePanel/DChoice{i+1}").gameObject.GetComponent<Button>(); 
            choiceButtons[i].onClick.AddListener(() => dialogueSystem.ChoicePick(i));   
            Debug.Log(i.ToString());             
        }*/
        choiceButtons[0] = objDialogueSystem.transform.Find("DialogueChoicePanel/DChoice1").gameObject.GetComponent<Button>(); 
        choiceButtons[0].onClick.AddListener(() => dialogueSystem.ChoicePick1()); 

        choiceButtons[1] = objDialogueSystem.transform.Find("DialogueChoicePanel/DChoice2").gameObject.GetComponent<Button>(); 
        choiceButtons[1].onClick.AddListener(() => dialogueSystem.ChoicePick2()); 

        choiceButtons[2] = objDialogueSystem.transform.Find("DialogueChoicePanel/DChoice3").gameObject.GetComponent<Button>(); 
        choiceButtons[2].onClick.AddListener(() => dialogueSystem.ChoicePick3());
    }

    private void GetTranslate()
    {
        TranslationSave.SaveInDB();
        //APIWordnik.GetRequest("fruit");
        //APIRequest.GetYandex("pizza");
        /*var YandexTask = APIRequest.Get5Recipies("pizza");
        if (YandexTask == null)
            Debug.Log("Запрос на апи ничего не возвращает");
        else
        {
            var translate = YandexTask.GetAwaiter().GetResult();
            //Debug.Log(translate.sentences.ToString());
            foreach(APIRequest.Sentence sentences in translate)
            {
                Debug.Log(sentences.trans);
            }
        }*/
    }

}
