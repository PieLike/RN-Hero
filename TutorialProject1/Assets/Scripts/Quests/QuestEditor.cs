using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
    [CustomEditor(typeof(QuestData))]
    public class QuestEditor : Editor
    {
        private static GUILayoutOption TextWidth = GUILayout.MaxWidth(100);

        public override void OnInspectorGUI()
        {
            QuestData questData = (QuestData)target;            

            DrawQuestMainInfo(questData);

            List<QuestData.Phase> listPhases;
            if(questData.ListPhases != null)
                listPhases = questData.ListPhases;
            else
                listPhases = new List<QuestData.Phase>();

            questData.Phases = Mathf.Max(0, EditorGUILayout.IntField("Phases", questData.Phases));   //listPhases.Count

            while (questData.Phases > listPhases.Count)
            {
                listPhases.Add(null);
                //QuestData.AddPhase(null);
                listPhases[listPhases.Count-1] = new QuestData.Phase();
            }
            while (questData.Phases < listPhases.Count)
                listPhases.RemoveAt(listPhases.Count - 1);
            for(int i = 0; i < listPhases.Count; i++)
            {        
                EditorGUILayout.Space();  
                listPhases[i].showFolder = EditorGUILayout.Foldout(listPhases[i].showFolder, "Phase " + (i+1).ToString(), true);
                if (listPhases[i].showFolder)
                {
                    listPhases[i] = DrawPhase(listPhases[i]);
                }
            }

            EditorGUILayout.Space();
            questData.showQuestTakerFolder = EditorGUILayout.Foldout(questData.showQuestTakerFolder, "Hand in", true);;
            if (questData.showQuestTakerFolder)
                DrawHandin(questData);

            questData.ListPhases = listPhases;

            bool somethingChanged = EditorGUI.EndChangeCheck();
            if(somethingChanged)
            {
                EditorUtility.SetDirty(questData);
            }
        }

        private static void DrawQuestMainInfo(QuestData questData)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Id", TextWidth);
            questData.Id = EditorGUILayout.IntField(questData.Id);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", TextWidth);
            questData.QuestName = EditorGUILayout.TextField(questData.QuestName);
            EditorGUILayout.EndHorizontal();
        }

        private static QuestData.Phase DrawPhase(QuestData.Phase NewPhase)
        {    
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type", TextWidth);
            NewPhase.taskType = (QuestData.TaskType)EditorGUILayout.EnumPopup(NewPhase.taskType, GUILayout.MinWidth(200));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Task", TextWidth);
            NewPhase.task = EditorGUILayout.TextField(NewPhase.task, GUILayout.MaxHeight(40));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Position", TextWidth);
            NewPhase.position = EditorGUILayout.TextField(NewPhase.position);
            EditorGUILayout.EndHorizontal();

            if(NewPhase.taskType != QuestData.TaskType.ReachPosition)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Target name", TextWidth);
                NewPhase.targetName = EditorGUILayout.TextField(NewPhase.targetName);
                EditorGUILayout.EndHorizontal();

                if (NewPhase.taskType != QuestData.TaskType.InteractWithNpc)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Count", TextWidth);
                    NewPhase.count = EditorGUILayout.IntField(NewPhase.count);
                    EditorGUILayout.EndHorizontal();
                }
            }

            return NewPhase;
        }

        private static void DrawHandin(QuestData questData)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Target", TextWidth);
            questData.QuestTaker = EditorGUILayout.TextField(questData.QuestTaker);
            EditorGUILayout.EndHorizontal();

            if(questData.QuestTaker != "")
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Task", TextWidth);
                questData.QuestTaker_Task = EditorGUILayout.TextField(questData.QuestTaker_Task, GUILayout.MaxHeight(40));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Position", TextWidth);
                questData.QuestTaker_Position = EditorGUILayout.TextField(questData.QuestTaker_Position);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif   