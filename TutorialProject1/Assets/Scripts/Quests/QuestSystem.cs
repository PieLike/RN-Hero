using UnityEngine;
using System.Data;
using System;
using TMPro;

public class QuestSystem : MonoBehaviour
{
   private string actualDataBaseName = "questActual.bytes";
   private GameObject objQuest;//, questsList;
   private GameObject AQPanel, AQText;
   private GameObject priorityQuest;

    private void Start() 
    {
        //questsList = GameObject.Find("Quests");
        objQuest = Resources.Load<GameObject>("3d_prefabs/Quest");
        //CreateActualQuests();
        QuestEvents.OnQuestsChange += CreateActualQuests; 

        AQPanel = GameObject.Find("Interface/MainInterface/ActiveQuestPanel");
        AQText = AQPanel.transform.Find("AQText").gameObject;
    }

   void OnApplicationQuit()
    {
        //очищаем активную бд словаря перед выключением
        string query = ("DELETE FROM quests;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, actualDataBaseName);        
    }

    public void CreateActualQuests()
    {        
        //ClearActualQuests();
        //заполняем DataTable, потом шерстим его, если квест активен, то создаем его объект
        DataTable actualQuests = GetActiveQuestsList();
        foreach (DataRow row in actualQuests.Rows)
        {
            if(row["status"].ToString() == "активен")
            {
                //string questName = row["name"].ToString();
                int id = Convert.ToInt32(row["id"]); 
                int phase = Convert.ToInt32(row["phase"]);
                CreateQuestObj(id, phase);    
            }
        }
        ChosePriorityQuest();
        SetTaskFromPriority();
    }

    private void CreateQuestObj(int id, int phase)
    {
        if (FindQuest(id) == false) //если такого квеста не найдено на сцене
        {
            //создаем объект квеста
            GameObject newQuest = Instantiate(objQuest, Vector3.zero, Quaternion.Euler(0,0,0));
            newQuest.transform.parent = gameObject.transform;
            QuestBehavior questBehavior = newQuest.AddComponent<QuestBehavior>();

            string questName        = string.Concat("quest_", id.ToString()); 
            newQuest.name = questName;

            string fullQuestName    = string.Concat("Quests/", questName);      
            questBehavior.data = Resources.Load<QuestData>(fullQuestName);

            questBehavior.StartQuest();
        }
    }

    private DataTable GetActiveQuestsList()
    {
        //заполняем DataTable АКТИВНЫМИ квестами
        DataTable actualQuests;

        string query = ("SELECT * FROM quests WHERE status = 'активен';");
        actualQuests = WorkWithDataBase.GetTable(query, actualDataBaseName);

        return actualQuests;
    }

    private bool FindQuest(int id)
    {
        string questName = string.Concat("quest_", id.ToString()); 

        Transform[] quests = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (var quest in quests)
        {
            if (quest.name == questName)
                return true;

        }
        return false;
    }

    private void SetTaskFromPriority()
    {
        if(priorityQuest != null)
        {
            QuestBehavior questBehavior = priorityQuest.GetComponentInChildren<QuestBehavior>();
            if (questBehavior != null && questBehavior.actualTask != "")
            {
                string task = questBehavior.actualTask;
                SetTask(task);  
            }  
        }
    }
    private void SetTask(string task)
    {
        AQText.GetComponent<TMP_Text>().text = task;
    }

    private void ChosePriorityQuest()
    {
        /*GameObject[] quests = GameObject.FindGameObjectsWithTag("Quest");
        if (quests.Length > 0)
        {            
            priorityQuest = quests[0];
            for(int i = 0; i < quests.Length; i++)
            {
                if(priorityQuest.)           
            }
        }*/
        priorityQuest = GameObject.FindGameObjectWithTag("Quest");
    }

    private void OnDisable() 
    {
        QuestEvents.OnQuestsChange -= CreateActualQuests;    
    }
}
