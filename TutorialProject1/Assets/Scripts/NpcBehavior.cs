using UnityEngine;
[RequireComponent(typeof(MyOutline))]

public class NpcBehavior : MonoBehaviour
{
    public string npcName = "";
    private MyOutline outline; 
    private GameObject objDialogueSystem; private DialogueSystem dialogueSystem;
    void Start()
    {
        outline = GetComponent<MyOutline>();

        objDialogueSystem = GameObject.Find("Interface/DialogueSystem");  
        dialogueSystem = objDialogueSystem.GetComponent<DialogueSystem>(); 

        if (npcName == "")
            npcName = gameObject.name;   
    }

    void Update()
    {
        //добавляем или скрываем обводку в зависимости от того наведен ли крусор на объект
        if (Interaction.supposedInteractionObject == gameObject)
        {
            outline.SetOutline(MyOutline.Color.blue);
        }    
        else
        {
            outline.RemoveOutline();  
        }   
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
