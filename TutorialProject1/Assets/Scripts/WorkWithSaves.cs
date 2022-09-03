using System.Data;
using UnityEngine;


public class WorkWithSaves
{
    private static string vocActualDBName = "vocabularyActual.bytes", vocSaveDBName = "vocabularySave.bytes"; 
    private static string questActualDBName = "questActual.bytes", questSaveDBName = "questSave.bytes";
    
    static public void CreateSave()
    {
        //сначала очищаем старые данные в сохраненном словаре, затем записываем новые из актуального словаря
        ClearVoculabraryDataBase(vocSaveDBName);
        ClearQuestsDataBase(questSaveDBName);

        DataTable actualVocabulary;        
        actualVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", vocActualDBName);        
        WorkWithDataBase.InsertManyRow(vocSaveDBName, actualVocabulary, "words"); 

        DataTable actualQuests;        
        actualQuests = WorkWithDataBase.GetTable("SELECT * FROM quests;", questActualDBName);        
        WorkWithDataBase.InsertManyRow(questSaveDBName, actualQuests, "quests");     
    }
    static public void LoadSave()
    {        
        ClearVoculabraryDataBase(vocActualDBName);        
        ClearQuestsDataBase(questActualDBName);

        DataTable saveVocabulary;
        saveVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", vocSaveDBName);
        WorkWithDataBase.InsertManyRow(vocActualDBName, saveVocabulary, "words");                

        DataTable saveQuests;
        saveQuests = WorkWithDataBase.GetTable("SELECT * FROM quests;", questSaveDBName);
        WorkWithDataBase.InsertManyRow(questActualDBName, saveQuests, "quests");        
        QuestEvents.SendQuestsChange();
    }  

     static private void ClearVoculabraryDataBase(string dataBaseName)
    {
        //очищаем дб сохраненного словаря
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, dataBaseName);        
    } 
    static private void ClearQuestsDataBase(string dataBaseName)
    {
        //очищаем дб сохраненного словаря
        string query = ("DELETE FROM quests;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, dataBaseName);        
    }       

}
