using System.Data;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private static string vocSaveDBName = "vocabularySave.bytes";
    private static string questActualDBName = "questActual.bytes", questSaveDBName = "questSave.bytes";
    
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void CreateSave()
    {
        //сначала очищаем старые данные в сохраненном словаре, затем записываем новые из актуального словаря
        ClearVoculabraryDataBase(vocSaveDBName);
        ClearQuestsDataBase(questSaveDBName);

        ManagerToDataBase(vocSaveDBName, "dictionary");

        DataTable actualQuests;        
        actualQuests = WorkWithDataBase.GetTable("SELECT * FROM quests;", questActualDBName);        
        WorkWithDataBase.InsertManyRow(questSaveDBName, actualQuests, "quests");     
    }
    public void LoadSave()
    {        
        //ClearVoculabraryDataBase(vocActualDBName);        
        ClearQuestsDataBase(questActualDBName);

        DataTable saveVocabulary;
        saveVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", vocSaveDBName);
        DataTableToManager(saveVocabulary, "dictionary");                

        DataTable saveQuests;
        saveQuests = WorkWithDataBase.GetTable("SELECT * FROM quests;", questSaveDBName);
        WorkWithDataBase.InsertManyRow(questActualDBName, saveQuests, "quests");    

        QuestEvents.SendQuestsChange();
    }  

    private void ClearVoculabraryDataBase(string dataBaseName)
    {
        //очищаем дб сохраненного словаря
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, dataBaseName);        
    } 
    private void ClearQuestsDataBase(string dataBaseName)
    {
        //очищаем дб сохраненного словаря
        string query = ("DELETE FROM quests;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, dataBaseName);        
    }  

    private void DataTableToManager(DataTable dataTable, string manager)
    {
        manager = manager.ToLower();
        switch (manager)
        {
            case "dictionary":
                DictionaryManager dictionaryManager = FindObjectOfType<DictionaryManager>(); 
                dictionaryManager.AddWordsWithClear(dataTable);

                break;
            default:
                break;
        }        
    }  

    private void ManagerToDataBase(string nameDataBase, string manager)
    {
        manager = manager.ToLower();
        switch (manager)
        {
            case "dictionary":
                DictionaryManager dictionaryManager = FindObjectOfType<DictionaryManager>(); 
                dictionaryManager.SaveWordsToDataBase(nameDataBase);

                break;
            default:
                break;
        }        
    }    

}
