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

        DialogueSystem.Dialogue dialogue = new DialogueSystem.Dialogue();
        dialogue.name = npcName;
        dialogue.sentencs = new string[3];
        dialogue.sentencs[0] = "1123123";
        dialogue.sentencs[1] = "212312312";
        dialogue.sentencs[2] = "3123123";       

        dialogueSystem.StartDialogue(dialogue);
    }
}
