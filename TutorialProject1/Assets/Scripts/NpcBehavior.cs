using UnityEngine;
[RequireComponent(typeof(MyOutline))]

public class NpcBehavior : MonoBehaviour
{
    public string npcName = "";
    private MyOutline outline; 
    //private GameObject objDialogueSystem; 
    private DialogueSystem dialogueSystem;
    private InterfaceManager interfaceManager;
    void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();

        outline = GetComponent<MyOutline>();

        dialogueSystem = interfaceManager.DialogueSystem.GetComponent<DialogueSystem>();


        if (npcName == "")
            npcName = gameObject.name;   
    }

    void Update()
    {
        //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
        /*if (MousePosition2D.supposedInteractionObject == gameObject)
        {
            outline.SetOutline(MyOutline.Color.blue);
        }    
        else
        {
            outline.RemoveOutline();  
        }   */
    }
    public void Interact()
    {        
        QuestEvents.SendInteractionNpc(npcName);      

        string fileName = "New Narrative 2";
        DialogueContainer dialogueGraph = Resources.Load<DialogueContainer>($"DialogueGraphs/{fileName}"); 
        var dialogue = dialogueSystem.ConvertGraphToDialogue(dialogueGraph);

        dialogue.name = npcName;        

        dialogueSystem.StartDialogue(dialogue);
    }
}
