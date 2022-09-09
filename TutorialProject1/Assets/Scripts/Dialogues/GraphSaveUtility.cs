using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView targetGraphView;
    private DialogueContainer containerCache;
    private List<Edge> Edges => targetGraphView.edges.ToList();
    private List<DialogueNode> Nodes => targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
    
    public static GraphSaveUtility GetInstance(DialogueGraphView newTargetGraphView)
    {
        return new GraphSaveUtility {  targetGraphView = newTargetGraphView };
    }

    public void SaveGraph(string fileName)
    {
        DialogueContainer dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        if (!SaveNodes(fileName, dialogueContainer)) return;

        SaveExposedProperties(dialogueContainer);
        //SaveCommentBlocks(dialogueContainer);

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/DialogueGraphs/{fileName}.asset", typeof(DialogueContainer));

        if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) 
        {
            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/DialogueGraphs/{fileName}.asset");
        }
        else 
        {
            DialogueContainer container = loadedAsset as DialogueContainer;
            container.NodeLinks = dialogueContainer.NodeLinks;
            container.DialogueNodeData = dialogueContainer.DialogueNodeData;
            container.ExposedProperties = dialogueContainer.ExposedProperties;
            //container.CommentBlockData = dialogueContainer.CommentBlockData;
            EditorUtility.SetDirty(container);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.SaveAssets();
    }
    public void LoadGraph(string fileName)
    {
        containerCache = Resources.Load<DialogueContainer>($"DialogueGraphs/{fileName}");
        if (containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
            return;
        }

        ClearGraph();
        GenerateDialogueNodes();
        ConnectDialogueNodes();
        AddExposedProperties();
        //GenerateCommentBlocks();
    }
    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).GUID = containerCache.NodeLinks[0].BaseNodeGUID;
        foreach (var perNode in Nodes)
        {
            //if (perNode.EntryPoint) continue;
            Edges.Where(x => x.input.node == perNode).ToList()
                .ForEach(edge => targetGraphView.RemoveElement(edge));
            targetGraphView.RemoveElement(perNode);
        }
    }

    private void GenerateDialogueNodes()
    {
        foreach (var perNode in containerCache.DialogueNodeData)
        {
            var tempNode = targetGraphView.AddNodeElement("", perNode.EntryPoint, Vector2.zero, perNode.DialogueText, perNode.NodeGUID, perNode.emotion);

            var nodePorts = containerCache.NodeLinks.Where(x => x.BaseNodeGUID == perNode.NodeGUID).ToList();
            nodePorts.ForEach(x => targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    private void ConnectDialogueNodes()
    {
        for (var i = 0; i < Nodes.Count; i++)
        {
            var k = i; //Prevent access to modified closure
            var connections = containerCache.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[k].GUID).ToList();
            for (var j = 0; j < connections.Count(); j++)
            {
                var targetNodeGUID = connections[j].TargetNodeGUID;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                LinkNodesTogether(Nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(
                    containerCache.DialogueNodeData.First(x => x.NodeGUID == targetNodeGUID).Position,
                    targetGraphView.defaultNodeSize));
            }
        }
    }
    private void LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        var tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        targetGraphView.Add(tempEdge);
    }

    private bool SaveNodes(string fileName, DialogueContainer dialogueContainerObject)
    {
        if (!Edges.Any()) return false;
        var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedSockets.Count(); i++)
        {
            var outputNode = (connectedSockets[i].output.node as DialogueNode);
            var inputNode = (connectedSockets[i].input.node as DialogueNode);
            dialogueContainerObject.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGUID = outputNode.GUID,
                PortName = connectedSockets[i].output.portName,
                TargetNodeGUID = inputNode.GUID
            });
        }

        foreach (var node in Nodes)//.Where(node => !node.EntryPoint))
        {
            dialogueContainerObject.DialogueNodeData.Add(new DialogueNodeData
            {
                NodeGUID = node.GUID,
                DialogueText = node.DialogueText,
                Position = node.GetPosition().position,
                Choices = CollectChoices(node.GUID, Edges),
                emotion = node.emotionType,
                EntryPoint = node.EntryPoint              
            });
        }

        return true;
    }
    private List<DialogueNodeData.DialogueChoice> CollectChoices(string nodeGuid, List<Edge> edges)
    {
        List<DialogueNodeData.DialogueChoice> Choices = new List<DialogueNodeData.DialogueChoice>();
        if (!edges.Any()) 
            return Choices;
            
        var connectedSockets = edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedSockets.Count(); i++)
        {
            var outputNode = (connectedSockets[i].output.node as DialogueNode);
            var inputNode = (connectedSockets[i].input.node as DialogueNode);
            if(nodeGuid == outputNode.GUID)
            {
                var choice = new DialogueNodeData.DialogueChoice();
                    
                choice.name = connectedSockets[i].output.portName;
                choice.targetGuid = inputNode.GUID;

                Choices.Add(choice);
            }
        }  
        return Choices;
    }

    private void SaveExposedProperties(DialogueContainer dialogueContainer)
    {
        dialogueContainer.ExposedProperties.Clear();
        dialogueContainer.ExposedProperties.AddRange(targetGraphView.ExposedProperties);
    }

    /*private void SaveCommentBlocks(DialogueContainer dialogueContainer)
    {
        foreach (var block in CommentBlocks)
        {
            var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.GUID)
                .ToList();

            dialogueContainer.CommentBlockData.Add(new CommentBlockData
            {
                ChildNodes = nodes,
                Title = block.title,
                Position = block.GetPosition().position
            });
        }
    }*/

    private void AddExposedProperties()
    {
        targetGraphView.ClearBlackBoardAndExposedProperties();
        foreach (var exposedProperty in containerCache.ExposedProperties)
        {
            targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }
    }
}
