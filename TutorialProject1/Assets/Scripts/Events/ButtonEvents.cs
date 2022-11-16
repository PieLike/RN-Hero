using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    private SaveManager saveManager;
    public Button dialogueContunueButton;
    private InterfaceManager interfaceManager;
    void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        saveManager = FindObjectOfType<SaveManager>();

        //кнпока сохранения
        Button saveButton = interfaceManager.SaveButton.GetComponent<Button>();
        saveButton.onClick.AddListener(() => saveManager.CreateSave());  

        //кнопка загрузки
        Button loadButton = interfaceManager.LoadButton.GetComponent<Button>();
        loadButton.onClick.AddListener(() => saveManager.LoadSave());  

        //кнопка вызова словаря
        Voculabrary voculabrary = interfaceManager.Voculabrary.GetComponent<Voculabrary>();
        Button vocalabraryButton = interfaceManager.VoculabraryButton.GetComponent<Button>(); 
        vocalabraryButton.onClick.AddListener(() => voculabrary.VoculabraryButton());  

        //кнопка алхимия
        Alchemy alchemy = interfaceManager.Alchemy.GetComponent<Alchemy>();
        Button alchemyButton = interfaceManager.AlchemyButton.GetComponent<Button>(); 
        alchemyButton.onClick.AddListener(() => alchemy.AlchemyButton()); 

        //кнопка инвентарь
        Inventory inventory = interfaceManager.Inventory.GetComponent<Inventory>();
        Button inventoryButton = interfaceManager.InventoryButton.GetComponent<Button>(); 
        inventoryButton.onClick.AddListener(() => inventory.InventoryButton()); 

        //кнопка вызова правил
        Button rulesButton = interfaceManager.RulesButton.GetComponent<Button>(); 
        rulesButton.onClick.AddListener(() => GetTranslate());  

        //кнопка атрефактов
        ArtifactMenu artifactMenu = interfaceManager.Inventory.GetComponent<ArtifactMenu>();
        Button artifactButton = interfaceManager.ArtifactButton.GetComponent<Button>(); 
        artifactButton.onClick.AddListener(() => artifactMenu.ArtifactButton()); 

        //кнопка следующего предложения диалога на окошке диалога с нпс
        dialogueSystem = interfaceManager.DialogueSystem.GetComponent<DialogueSystem>();  

        FundDialogueButton();
        FundChoiceButton();         
    }

    public void FundDialogueButton()
    {
        dialogueContunueButton = interfaceManager.DialogueContinue.GetComponent<Button>();  
        dialogueContunueButton.onClick.AddListener(() => { dialogueSystem.DisplayNextSentence();});  
    }
    public void FundChoiceButton()
    {
        //кнопки выбора диалога  
        Button[] choiceButtons = new Button[3];
        choiceButtons[0] = interfaceManager.DChoice1.GetComponent<Button>(); 
        choiceButtons[0].onClick.AddListener(() => dialogueSystem.ChoicePick1()); 

        choiceButtons[1] = interfaceManager.DChoice2.GetComponent<Button>();
        choiceButtons[1].onClick.AddListener(() => dialogueSystem.ChoicePick2()); 

        choiceButtons[2] = interfaceManager.DChoice3.GetComponent<Button>(); ;
        choiceButtons[2].onClick.AddListener(() => dialogueSystem.ChoicePick3());
    }

    private void GetTranslate()
    {
        //NounProjectIcon.GetRequest("pizza");
        /*var results = Datamuse.GetRequest("forest", Datamuse.reauestType.both);
        foreach(var result in results)
        {
            Debug.Log(result.word + ", " + result.pos);
        }*/
    TranslationSave.SaveAllInDB();
        //APIWordnik.GetRequest("fruit");
    //APIRequest.GetYandex("fly");
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
