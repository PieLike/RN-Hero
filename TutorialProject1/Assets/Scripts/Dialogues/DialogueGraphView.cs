using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.UIElements.DropdownMenuAction;
using Subtegral.DialogueSystem.DataContainers;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(150,200); //x,y
        public Blackboard Blackboard = new Blackboard();
        public List<ExposedProperty> ExposedProperties { get; private set; } = new List<ExposedProperty>();
    
    public DialogueGraphView()
    {
        AddGridBackground();
        AddStyles();
        AddManipulators();

        AddNodeElement("", true, new Vector2(100,200));
    }
    
    public DialogueNode AddNodeElement( string nodeName = "", bool entrypoint = false, Vector2 position = default(Vector2),
                                        string nodeText = "", string nodeGUID = "", DialogueSystem.emotions emotion = DialogueSystem.emotions.neutral)
    {
        DialogueNode node;
        if (entrypoint)
            node = CreateEntryPointNode(nodeName, nodeGUID);      
        else
            node = CreateNewDialogueNode(nodeName, nodeGUID);
   
        node.SetPosition(new Rect(position, defaultNodeSize)); //x,y,width,height   
        node.AddTextInNode(nodeText);
        node.SetEmotionType(emotion);

        

        List<DialogueSystem.emotions> m_PopupValues = new System.Collections.Generic.List<DialogueSystem.emotions>
        { DialogueSystem.emotions.neutral, DialogueSystem.emotions.happy, DialogueSystem.emotions.angry };

        var enumPopup = new UnityEditor.UIElements.PopupField<DialogueSystem.emotions>( choices: m_PopupValues, defaultValue: node.emotionType,
                                                                                        formatSelectedValueCallback: value => node.SetEmotionType(value));
        enumPopup.style.width = 70; 
        node.titleContainer.Add(enumPopup);

        var button = new Button( () => AddChoicePort(node) );
        button.text = "New choice";
        node.titleContainer.Add(button);

        node.Initialize();
        //node.Draw();
        node.RefreshExpandedState();
        AddElement(node);

        return node;
    }

    private DialogueNode CreateEntryPointNode(string nodeName = "Start Node", string nodeGUID = "")
    {
        DialogueNode node = new DialogueNode{ title = nodeName, EntryPoint = true };
        if (nodeGUID == "")
            node.GUID = Guid.NewGuid().ToString();
        else
            node.GUID = nodeGUID;   
        //var outputPort = GeneratePort(node, false);
        //outputPort.portName = "First";
        //node.outputContainer.Add(outputPort);

        return node;
    }
    public DialogueNode CreateNewDialogueNode(string nodeName = "", string nodeGUID = "")
    {
        DialogueNode node = new DialogueNode{ title = nodeName, EntryPoint = false };
        if (nodeGUID == "")
            node.GUID = Guid.NewGuid().ToString();
        else
            node.GUID = nodeGUID;   

        var inputPort = GeneratePort(node, true, Port.Capacity.Multi);
        inputPort.portName = "Input";
        node.inputContainer.Add(inputPort);

        return node;
    }    
    
    public void AddChoicePort(DialogueNode node, string overriddenPortName = "")
    {
        var newOutputPort = GeneratePort(node, false);  
            var portLabel = newOutputPort.contentContainer.Q<Label>("type");
            newOutputPort.contentContainer.Remove(portLabel);      
        var outputPortCount = node.outputContainer.Query("connector").ToList().Count;
        string outputPortName = string.IsNullOrEmpty(overriddenPortName)?
            $"Choice {outputPortCount + 1}"
            : overriddenPortName;

        var textField = new TextField()
        {
            name = string.Empty,
            value = outputPortName
        };
        textField.RegisterValueChangedCallback(evt => newOutputPort.portName = evt.newValue);
        newOutputPort.contentContainer.Add(new Label("  "));
        newOutputPort.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(node, newOutputPort))
        {
            text = "X"
        };
        newOutputPort.contentContainer.Add(deleteButton);

        newOutputPort.portName = outputPortName;
        node.outputContainer.Add(newOutputPort);        

        node.RefreshExpandedState();        
    }    

    private void RemovePort(DialogueNode node, Port socket)
    {
        var targetEdge = edges.ToList()
            .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        node.outputContainer.Remove(socket);        

        //node.RefreshPorts();
        node.RefreshExpandedState();
    }

    private Port GeneratePort(DialogueNode node, bool ImputPort, Port.Capacity capacity = Port.Capacity.Single)
    {
        if (ImputPort)
            return node.InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(float)); //Arbitrary type (случайный тип)
        else
            return node.InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(float)); //Arbitrary type (случайный тип)
    }   

    private IManipulator CreateNodeContextMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(menuBuilder => 
        {   
            //menuBuilder.menu.AppendAction("Add Start Node", actionEvent => AddNodeElement("", true, actionEvent.eventInfo.localMousePosition), Status.Normal);
            menuBuilder.menu.AppendAction("Add Node", actionEvent => AddNodeElement("", false, actionEvent.eventInfo.localMousePosition), Status.Normal);
        });    

        return contextualMenuManipulator;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>(); //совместимые порты
        ports.ForEach((port) => 
        {   
            if(startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });        
        //return base.GetCompatiblePorts(startPort, nodeAdapter);
        return compatiblePorts;
    }

    private void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0, gridBackground);
    }
    private void AddStyles()
    {
        StyleSheet styleSheet = EditorGUIUtility.Load("Styles/GraphViewStyle.uss") as StyleSheet;
        styleSheets.Add(styleSheet);
    }

    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateNodeContextMenu());
    }
    public void AddPropertyToBlackBoard(ExposedProperty property, bool loadMode = false)
    {
        var localPropertyName = property.PropertyName;
        var localPropertyValue = property.PropertyValue;
        if (!loadMode)
        {
            while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
                localPropertyName = $"{localPropertyName}(1)";
        }

        var item = ExposedProperty.CreateInstance();
        item.PropertyName = localPropertyName;
        item.PropertyValue = localPropertyValue;
        ExposedProperties.Add(item);

        var container = new VisualElement();
        var field = new BlackboardField {text = localPropertyName, typeText = "string"};
        container.Add(field);

        var propertyValueTextField = new TextField("Value:")
        {
            value = localPropertyValue
        };
        propertyValueTextField.RegisterValueChangedCallback(evt =>
        {
            var index = ExposedProperties.FindIndex(x => x.PropertyName == item.PropertyName);
            ExposedProperties[index].PropertyValue = evt.newValue;
        });
        var sa = new BlackboardRow(field, propertyValueTextField);
        container.Add(sa);
        Blackboard.Add(container);
    }

    public void ClearBlackBoardAndExposedProperties()
    {
        ExposedProperties.Clear();
        Blackboard.Clear();
    }
}
