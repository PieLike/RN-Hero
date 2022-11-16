using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(MessageScript))]
public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    public GameObject Interface;
    public GameObject MainInterface, Voculabrary, UsingSpell, EnglishGame, Menu, DialogueSystem, YouGotMessage,  Alchemy, Inventory, PotionChoice, InfoWindow;    //Interface
    public GameObject VoculabraryButton, PickSpell, ActiveQuestPanel, AQText, Experience, RulesButton, AlchemyButton, InventoryButton, HeroState, HpPanel, HpLine, HpNumber, MpPanel, MpLine, MpNumber, ArtifactButton, Waves, SkillButton;   //MainInterface
    public GameObject VoculabraryInterfacePanel, VIList, VIExit, VIViewport, VIContent; //Voculabrary
    public GameObject UsingSpellPanel, USName, USCount; //UsingSpell
    public GameObject EnglishGamePanel, EGOriginalWord, EGExit, EGInputPanel, EGCheckmark, PickerWheelEnglshType;  //EnglishGame
    public GameObject SaveButton, LoadButton;   //Menu
    public GameObject DialogueChoicePanel, DChoice1, DChoice2, DChoice3, DialoguePanel2, DialogueContinue, DialogueName;   //DialogueSystem
    public GameObject YGPanel, YGName, YGText; //DialogueSystem
    public GameObject ExperiencePanel, ExpLine, OkMessagePanel, OkLabel, OkButton; //YouGotMessage
    public GameObject AlcPanel, AlcProgressPanel, AlcProgressLine, AlcPotionImage, AlcWords, AlcViewport, AlcContent, Ingredients, AlcExit, AlcBack, AlcPotionPanel, PotionSlots, APExit, Stats, Stats2;    //Alchemy
    public GameObject EquipmentPanel, ArmorSlot, WeaponSlot, InventoryPanel, InvSlots, NewItemsPanel, NIName, NISlots, TrashButton, PackButton, ArtifactsPanel, ArtifactSlots, ArExit;  //Inventory
    public GameObject PCPanel, PotionsPickerWheel;  //PotionChoice
    public GameObject InfoPanel;    //InfoWindow
    public GameObject Skills, SkillsPanel, SkillSlots, SkillExit;

    public MessageScript messageScript; public NewItemMessageScript newItemMessageScript; public InfoWindow infoWindowScript;
    
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Interface = gameObject;

        MainInterface       = Interface.transform.Find("MainInterface").gameObject;
            VoculabraryButton   = MainInterface.transform.Find("VoculabraryButton").gameObject;
//            PickSpell           = MainInterface.transform.Find("PickSpell").gameObject;
            ActiveQuestPanel    = MainInterface.transform.Find("ActiveQuestPanel").gameObject;
                AQText              = ActiveQuestPanel.transform.Find("AQText").gameObject; 
                ActiveQuestPanel.SetActive(false);
            RulesButton         = MainInterface.transform.Find("RulesButton").gameObject;
            //Experience          = MainInterface.transform.Find("Experience").gameObject;                
            AlchemyButton       = MainInterface.transform.Find("AlchemyButton").gameObject;
            InventoryButton     = MainInterface.transform.Find("InventoryButton").gameObject;
            HeroState           = MainInterface.transform.Find("HeroState").gameObject;
                HpPanel             = HeroState.transform.Find("HpPanel").gameObject;
                    HpLine             = HpPanel.transform.Find("HpLine").gameObject;
                    HpNumber           = HpPanel.transform.Find("HpNumber").gameObject;
                MpPanel             = HeroState.transform.Find("MpPanel").gameObject;
                    MpLine             = MpPanel.transform.Find("MpLine").gameObject;
                    MpNumber           = MpPanel.transform.Find("MpNumber").gameObject;
                ExperiencePanel     = HeroState.transform.Find("ExperiencePanel").gameObject;
                    ExpLine             = ExperiencePanel.transform.Find("ExpLine").gameObject;
            ArtifactButton      = MainInterface.transform.Find("ArtifactButton").gameObject;
            Waves               = MainInterface.transform.Find("Waves").gameObject;
            SkillButton         = MainInterface.transform.Find("SkillButton").gameObject;

        Voculabrary         = Interface.transform.Find("Voculabrary").gameObject;
            VoculabraryInterfacePanel = Voculabrary.transform.Find("VoculabraryInterfacePanel").gameObject;    
                //VIWordsList     = VoculabraryInterfacePanel.transform.Find("VIWordsList").gameObject;   
                VIList          = VoculabraryInterfacePanel.transform.Find("VIList").gameObject;  
                    VIViewport = VIList.transform.Find("VIViewport").gameObject;
                        VIContent = VIViewport.transform.Find("VIContent").gameObject;
                VIExit          = VoculabraryInterfacePanel.transform.Find("VIExit").gameObject;    

        UsingSpell          = Interface.transform.Find("UsingSpell").gameObject;
            UsingSpellPanel     = UsingSpell.transform.Find("UsingSpellPanel").gameObject;
                USName              = UsingSpellPanel.transform.Find("USName").gameObject;
                USCount             = UsingSpellPanel.transform.Find("USCount").gameObject;

        EnglishGame         = Interface.transform.Find("EnglishGame").gameObject;
            EnglishGamePanel    = EnglishGame.transform.Find("EnglishGamePanel").gameObject;
                EGOriginalWord      = EnglishGamePanel.transform.Find("EGOriginalWord").gameObject;
                EGExit              = EnglishGamePanel.transform.Find("EGExit").gameObject;
                EGInputPanel        = EnglishGamePanel.transform.Find("EGInputPanel").gameObject;
                EGCheckmark         = EnglishGamePanel.transform.Find("EGCheckmark").gameObject;
                PickerWheelEnglshType         = EnglishGamePanel.transform.Find("PickerWheel").gameObject;

        Menu                = Interface.transform.Find("Menu").gameObject;
            SaveButton          = Menu.transform.Find("SaveButton").gameObject;
            LoadButton          = Menu.transform.Find("LoadButton").gameObject;

        DialogueSystem      = Interface.transform.Find("DialogueSystem").gameObject;
            DialogueChoicePanel = DialogueSystem.transform.Find("DialogueChoicePanel").gameObject;
                DChoice1            = DialogueChoicePanel.transform.Find("DChoice1").gameObject;
                DChoice2            = DialogueChoicePanel.transform.Find("DChoice2").gameObject;
                DChoice3            = DialogueChoicePanel.transform.Find("DChoice3").gameObject;
            DialoguePanel2      = DialogueSystem.transform.Find("DialoguePanel2").gameObject;
                DialogueContinue    = DialoguePanel2.transform.Find("DialogueContinue").gameObject;
                DialogueName        = DialoguePanel2.transform.Find("DialogueName").gameObject;

        YouGotMessage       = Interface.transform.Find("YouGotMessage").gameObject;
            YGPanel             = YouGotMessage.transform.Find("YGPanel").gameObject;
                YGName              = YGPanel.transform.Find("YGName").gameObject;
                YGText              = YGPanel.transform.Find("YGText").gameObject; 
            OkMessagePanel      = YouGotMessage.transform.Find("OkMessagePanel").gameObject;
                OkLabel              = OkMessagePanel.transform.Find("OkLabel").gameObject;  
                OkButton             = OkMessagePanel.transform.Find("OkButton").gameObject;  

        Alchemy             = Interface.transform.Find("Alchemy").gameObject;
            AlcPanel            = Alchemy.transform.Find("AlcPanel").gameObject; 
                AlcProgressPanel    = AlcPanel.transform.Find("AlcProgressPanel").gameObject;  
                    AlcProgressLine     = AlcProgressPanel.transform.Find("AlcProgressLine").gameObject; 
                AlcPotionImage         = AlcPanel.transform.Find("AlcPotionImage").gameObject;                 
                AlcWords            = AlcPanel.transform.Find("AlcWords").gameObject;  
                    AlcViewport         = AlcWords.transform.Find("AlcViewport").gameObject;
                        AlcContent          = AlcViewport.transform.Find("AlcContent").gameObject;  
                Ingredients         = AlcPanel.transform.Find("Ingredients").gameObject;   
                AlcExit             = AlcPanel.transform.Find("AlcExit").gameObject;
                AlcBack             = AlcPanel.transform.Find("AlcBack").gameObject;
            AlcPotionPanel      = Alchemy.transform.Find("AlcPotionPanel").gameObject;
                PotionSlots         = AlcPotionPanel.transform.Find("PotionSlots").gameObject;
                APExit              = AlcPotionPanel.transform.Find("APExit").gameObject;

        Inventory           = Interface.transform.Find("Inventory").gameObject;
            EquipmentPanel      = Inventory.transform.Find("EquipmentPanel").gameObject;
                ArmorSlot           = EquipmentPanel.transform.Find("ArmorSlot").gameObject;
                WeaponSlot          = EquipmentPanel.transform.Find("WeaponSlot").gameObject; 
            InventoryPanel      = Inventory.transform.Find("InventoryPanel").gameObject;
                InvSlots            = InventoryPanel.transform.Find("InvSlots").gameObject;
            NewItemsPanel       = Inventory.transform.Find("NewItemsPanel").gameObject;
                NIName              = NewItemsPanel.transform.Find("NIName").gameObject;
                NISlots             = NewItemsPanel.transform.Find("NISlots").gameObject;
                    TrashButton         = NISlots.transform.Find("TrashButton").gameObject;
                    PackButton          = NISlots.transform.Find("PackButton").gameObject;
            ArtifactsPanel      = Inventory.transform.Find("ArtifactsPanel").gameObject;
                ArtifactSlots       = ArtifactsPanel.transform.Find("ArtifactSlots").gameObject;
                ArExit              = ArtifactsPanel.transform.Find("ArExit").gameObject; 
                Stats               = ArtifactsPanel.transform.Find("Stats").gameObject;
                Stats2              = ArtifactsPanel.transform.Find("Stats2").gameObject;

        PotionChoice            = Interface.transform.Find("PotionChoice").gameObject; 
            PCPanel                 = PotionChoice.transform.Find("PCPanel").gameObject;
                PotionsPickerWheel       = PCPanel.transform.Find("PickerWheel").gameObject;

        InfoWindow              = Interface.transform.Find("InfoWindow").gameObject; 
            InfoPanel               = InfoWindow.transform.Find("InfoPanel").gameObject;

        Skills                  = Interface.transform.Find("Skills").gameObject;  
        SkillsPanel                 = Skills.transform.Find("SkillsPanel").gameObject;
        SkillSlots                      = SkillsPanel.transform.Find("SkillSlots").gameObject;
        SkillExit                       = SkillsPanel.transform.Find("SkillExit").gameObject;



        newItemMessageScript = gameObject.GetComponent<NewItemMessageScript>();
        messageScript = gameObject.GetComponent<MessageScript>();
        infoWindowScript = InfoWindow.GetComponent<InfoWindow>();
            infoWindowScript.infoPanel = InfoPanel;
    }    

    private void Start() 
    {
        Camera mainCamera = Camera.main;
        MainInterface.GetComponent<Canvas>().worldCamera = mainCamera;
        Voculabrary.GetComponent<Canvas>().worldCamera = mainCamera;
        UsingSpell.GetComponent<Canvas>().worldCamera = mainCamera;
        EnglishGame.GetComponent<Canvas>().worldCamera = mainCamera;
        Menu.GetComponent<Canvas>().worldCamera = mainCamera;
        YouGotMessage.GetComponent<Canvas>().worldCamera = mainCamera;
        Alchemy.GetComponent<Canvas>().worldCamera = mainCamera;
        Inventory.GetComponent<Canvas>().worldCamera = mainCamera;
    }

    private void Update() 
    {
        if( VoculabraryInterfacePanel.activeSelf || EnglishGamePanel.activeSelf || AlcPanel.activeSelf)
        {
            if (MainVariables.inInterface == false)
                MainVariables.inInterface = true;  
            if (MainVariables.inMovement == true)
            MainVariables.inMovement = false;
        }
        else if(MainVariables.inInterface == true)
        {
            MainVariables.inInterface = false;
        }
            
    }
}
