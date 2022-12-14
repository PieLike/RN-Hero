using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    public string NodeGUID, DialogueText;
    public Vector2 Position;
    public bool EntryPoint = false, HeroLine = true;
    public DialogueSystem.emotions emotion;

    [SerializeField] public List<DialogueChoice> choices; public List<DialogueChoice> Choices {get{
        
        if (choices != null) 
            return choices;
        else
        {
            return new List<DialogueChoice>(); 
        }        
        } set{ choices = value;}}

    [Serializable]
    public class DialogueChoice
    {
        public string name;
        public string targetGuid;
    }
}
