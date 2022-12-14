using System;
using System.Collections.Generic;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public string NpcName;
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
}
