#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogueNode : Node
{
    public string GUID;
    public string DialogueName, DialogueText;
    private string NpcName = "НПС";
    public bool EntryPoint = false, heroLine = true;
    public List<DialogueNodeData.DialogueChoice> Choices;
    public DialogueSystem.emotions emotionType;

    public void AddTextInNode(string text = "")
    {
        var textField = new TextField("");
        textField.RegisterValueChangedCallback(evt =>
        {
            DialogueText = evt.newValue;
            //title = evt.newValue;
        });
        textField.SetValueWithoutNotify(text);
        mainContainer.Add(textField);
        DialogueText = text;
    }    
    public void Initialize(string npcName = "") 
    {
        //DialogueName = "DialogueName";
        Choices = new List<DialogueNodeData.DialogueChoice>();
        //DialogueText = "Dialogue text.";
        if (npcName != "")
            NpcName = npcName;
    }

    public void Draw()
    {
        //Title container
        TextField dialogueNameTextField = new TextField() {value = DialogueName};
        titleContainer.Insert(0,dialogueNameTextField);

        //Extensions container
        VisualElement customDataContainer = new VisualElement();
        Foldout textFoldout = new Foldout() { text = "Dialogue Text" };
        TextField dialogueTextTextField = new TextField() {value = DialogueText};
        textFoldout.Add(dialogueTextTextField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);

        RefreshExpandedState();
    }

    public string SetEmotionType(DialogueSystem.emotions emotion)
    {
        emotionType = emotion;
        return emotionType.ToString();
    }
    public void SetLineType(bool line)
    {
        heroLine = line;

        if (line)
            title = "Главный герой";
        else
            title = NpcName;                                                            
    }

}
#endif