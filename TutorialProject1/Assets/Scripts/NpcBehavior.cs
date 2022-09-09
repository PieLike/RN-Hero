using UnityEngine;
[RequireComponent(typeof(Outline))]

public class NpcBehavior : MonoBehaviour
{
    public string npcName = "";
    private Outline outline; 
    private GameObject objDialogueSystem; private DialogueSystem dialogueSystem;
    void Start()
    {
        outline = GetComponent<Outline>(); 
        outline.OutlineWidth = 0;  
        outline.OutlineColor = Color.blue;

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
            outline.OutlineWidth = 3;
        }    
        else if (outline.OutlineWidth != 0)
            outline.OutlineWidth = 0;    
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
