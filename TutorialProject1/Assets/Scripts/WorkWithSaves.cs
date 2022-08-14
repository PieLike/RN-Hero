using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public class WorkWithSaves : MonoBehaviour
{
    private string actualDataBaseName = "vocabularyActual.bytes"; 
    private string saveDataBaseName = "vocabularySave.bytes";
    
    public void CreateSave()
    {
        //сначала очищаем старые данные в сохраненном словаре, затем записываем новыеиз актуального словаря
        ClearDataBase(saveDataBaseName);

        DataTable actualVocabulary;        
        actualVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", actualDataBaseName);
        
        WorkWithDataBase.InsertManyRow(saveDataBaseName, actualVocabulary);    
    }

    private void ClearDataBase(string dataBaseName)
    {
        //очищаем дб сохраненного словаря
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, dataBaseName);        
    }  

    public void LoadSave()
    {
        ClearDataBase(actualDataBaseName);

        DataTable SaveVocabulary;
        SaveVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", saveDataBaseName);

        WorkWithDataBase.InsertManyRow(actualDataBaseName, SaveVocabulary);
    }  

}
