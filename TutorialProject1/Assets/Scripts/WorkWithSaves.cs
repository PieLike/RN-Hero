//using System.Collections;
//using System.Collections.Generic;
using System.Data;
using UnityEngine;


public class WorkWithSaves : MonoBehaviour
{
    private static string actualDataBaseName = "vocabularyActual.bytes"; 
    private static string saveDataBaseName = "vocabularySave.bytes";
    
    static public void CreateSave()
    {
        //сначала очищаем старые данные в сохраненном словаре, затем записываем новыеиз актуального словаря
        ClearDataBase(saveDataBaseName);

        DataTable actualVocabulary;        
        actualVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", actualDataBaseName);
        
        WorkWithDataBase.InsertManyRow(saveDataBaseName, actualVocabulary);    
    }

    static private void ClearDataBase(string dataBaseName)
    {
        //очищаем дб сохраненного словаря
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, dataBaseName);        
    }  

    static public void LoadSave()
    {
        ClearDataBase(actualDataBaseName);

        DataTable SaveVocabulary;
        SaveVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", saveDataBaseName);

        WorkWithDataBase.InsertManyRow(actualDataBaseName, SaveVocabulary);
    }  

}
