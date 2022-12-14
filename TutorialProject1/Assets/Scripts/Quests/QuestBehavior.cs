using UnityEngine;
using System;

public class QuestBehavior : MonoBehaviour
{
    private string actualPosition = "A0"; private string prevPosition;    
    [NonSerialized] public string actualTask = "";
    [NonSerialized] public QuestData data;
    [NonSerialized] public int actualPhase = 0;
    private string targetName; private int targetCount = 0, actualCount;    

    public void StartQuest() 
    {
        if(data.ListPhases.Count > 0)
            NewPhase(1);
        else
            PreResult();
    }

    private void NewPhase(int phaseNumber)
    {
        actualPhase = phaseNumber;
        actualPosition = data.ListPhases[phaseNumber-1].position;
        actualTask = data.ListPhases[phaseNumber-1].task;

        switch(data.ListPhases[phaseNumber-1].taskType)
        {
            case(QuestData.TaskType.KillEnemy):
                targetName = data.ListPhases[phaseNumber-1].targetName;
                targetCount = data.ListPhases[phaseNumber-1].count;
                actualCount = 0;
                QuestEvents.OnEnemyKilled += EnemyKillingChecker; 
            break;
            case(QuestData.TaskType.ReachPosition):
                QuestEvents.OnAreaReach += AreaReacherChecker;
            break;
            case(QuestData.TaskType.InteractWithNpc):
                targetName = data.ListPhases[phaseNumber-1].targetName;
                QuestEvents.OnInteractionNpc += InteractionNpcChecker;
            break;
            default:
                QuestEvents.OnAreaReach += AreaReacherChecker;
            break;
        }          
    } 

    private void PreResult() //сдать квест
    {
        //если есть кому сдавать квест и он существует на сцене
        if (data.QuestTaker != null && data.QuestTaker != "")
        {
            actualTask = data.QuestTaker_Task;
            actualPosition = data.QuestTaker_Position;  

            GameObject objQuestTaker = GameObject.Find(data.QuestTaker);    //добавить перед этим проверку на этот мир (сцену)
            if (objQuestTaker != null)
            {
                targetName = data.QuestTaker;
                QuestEvents.OnInteractionNpc += InteractionNpcChecker;
            }    
        }
        else
            Result();
    }

    private void Result() //награда
    {        
        //изменить статус на выполнено!!! изсенить запись в бд

        Destroy(gameObject, 1f);
    }

    private void Update() 
    {
        //при изменении или первом заполнении поля string actualPosition
        if(actualPosition!= prevPosition)
        {
            GameObject objActualPosition = GameObject.Find(actualPosition);
            //если такой блок карты найден на сцене, то перемещаем точку квеста на сцене (меняем позицию)
            if (objActualPosition != null)
            {
                gameObject.transform.localPosition = objActualPosition.transform.localPosition;
            }
        }     
    }   

    private void EnemyKillingChecker(QuestEvents.KilledEnemy killedEnemy)
    {
        string killedEnemyName = killedEnemy.enemy.fullName;
        //если убитый враг это свинья то срабатывает фаза
        if (killedEnemyName == targetName || targetName == "enemy")        
        {    
            actualCount++;
            if (actualCount >= targetCount)
            {            
                QuestEvents.OnEnemyKilled -= EnemyKillingChecker;

                if(data.ListPhases.Count > actualPhase)
                    NewPhase(actualPhase + 1);
                else
                    PreResult();
            }
        } 
    }
    private void AreaReacherChecker(string reachedAreaName)
    {
        //если убитый враг это свинья то срабатывает фаза
        if (reachedAreaName == actualPosition)        
        {                
            QuestEvents.OnAreaReach -= AreaReacherChecker;

            if(data.ListPhases.Count > actualPhase)
                NewPhase(actualPhase + 1);
            else
                PreResult();
        } 
    }
    private void InteractionNpcChecker(string interactionNpcName)
    {
        //если убитый враг это свинья то срабатывает фаза
        if (interactionNpcName == targetName)        
        {                
            QuestEvents.OnInteractionNpc -= InteractionNpcChecker;

            Result();
        } 
    }

}
