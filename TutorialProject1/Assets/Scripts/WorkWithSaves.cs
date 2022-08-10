using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WorkWithSaves : MonoBehaviour
{
    private string actualDataBaseName = "vocabularyActual.bytes", saveDataBaseName = "vocabularySave.bytes";
    
    public void CreateSave()
    {
        //сначала очищаем старые данные в сохраненном словаре, затем записываем новыеиз актуального словаря
        ClearSaveDataBase();

        DataTable actualVocabulary;        
        actualVocabulary = WorkWithDataBase.GetTable("SELECT * FROM words;", actualDataBaseName);
        
        WorkWithDataBase.InsertManyRow(saveDataBaseName, actualVocabulary);    
    }

    private void ClearSaveDataBase()
    {
        //очищаем дб сохраненного словаря
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, saveDataBaseName);        
    }  

    public void LoadSave()
    {

    }  

}
