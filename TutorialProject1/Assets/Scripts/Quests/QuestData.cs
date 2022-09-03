using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Quest", fileName = "quest_")]

public class QuestData: ScriptableObject
{
    public enum TaskType { ReachPosition, InteractWithNpc, KillEnemy };
    [SerializeField] private int id; public int Id {get{ return id;} set{ id = value;}}
    [SerializeField]private string questName; public string QuestName {get{ return questName;} set{ questName = value;}}
    //public string questGiver;  //public string QuestGever{get{ return questGiver;} protected set{}}    

    [SerializeField]private int phases; public int Phases {get{ return phases;} set{ phases = value;}}
    [SerializeField] private List<Phase> listPhases; public List<Phase> ListPhases {get{
        
        if (listPhases != null) 
            return listPhases;
        else
        {
            return new List<Phase>(phases); 
        }        
        } set{ listPhases = value;}}

    //сдать квест
    [SerializeField]private string questTaker; public string QuestTaker {get{ return questTaker;} set{ questTaker = value;}}
    [SerializeField]private string questTaker_Position; public string QuestTaker_Position {get{ return questTaker_Position;} set{ questTaker_Position = value;}}
    [SerializeField]private string questTaker_Task; public string QuestTaker_Task {get{ return questTaker_Task;} set{ questTaker_Task = value;}}
    [NonSerialized] public bool showQuestTakerFolder = true;

    [System.Serializable]
    public class Phase
    {
        public string task, targetName, position; public int count = 1; public TaskType taskType;
        public bool showFolder = true;       
    }

}
