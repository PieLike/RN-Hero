using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : Node
{
    public enum DialogueType{SingleChoice, MultipleChoice}
    public string GUID;
        public string DialogueName;
    public string DialogueText;
    public bool EntryPoint = false;
    public List<DialogueNodeData.DialogueChoice> Choices;
    public DialogueType dialogueType;
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
    public void Initialize() 
    {
        //DialogueName = "DialogueName";
        Choices = new List<DialogueNodeData.DialogueChoice>();
        //DialogueText = "Dialogue text.";
    }

    /*public void AddChoice()
    {
        Choices.Add(null);    
    }

    public void ParseChoices(List<Edge> edges)
    {
        Choices.Clear();

        //Debug.Log(edges.Count().ToString());
        if (!edges.Any()) return;
        var connectedSockets = edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedSockets.Count(); i++)
        {
            var outputNode = (connectedSockets[i].output.node as DialogueNode);
            var inputNode = (connectedSockets[i].input.node as DialogueNode);
            if(GUID == outputNode.GUID)
            {
                var choice = new DialogueSystem.DialogueChoice();
                    
                choice.name = connectedSockets[i].output.portName;
                choice.targetGuid = inputNode.GUID;

                Choices.Add(choice);
            }
        }  
        Debug.Log(Choices.Count().ToString());
    }*/

    public void Draw()
    {
        //Title container
        TextField dialogueNameTextField = new TextField() {value = DialogueName};
        titleContainer.Insert(0,dialogueNameTextField);

        //Input contaner
        //Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        //inputPort.portName = "Input";
        //inputContainer.Add(inputPort);

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

}
