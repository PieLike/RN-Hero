using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;

public class VocabularyActive : MonoBehaviour
{
    //public DataTable actualVocabulary, generalVocabulary;
    private string actualDataBaseName = "vocabularyActual.bytes", generalDataBaseName = "vocabularyGeneral.bytes";
    public GameObject voculabraryInterface;
    

    private void Start()
    {
        
    }

    void OnApplicationQuit()
    {
        //очищаем дб активного словаря перед выключением
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, actualDataBaseName);        
    }

    public void FillActiveVoculabraryUI()
    {
        //заполнить интерфейс словаря
        GameObject wordList;          
        wordList = GameObject.Find("VoculabraryInterfaceWordsList");

        DataTable actualVocabulary = GetActiveWordsList();
        for (int row = 0; row < actualVocabulary.Rows.Count; row++)
        {
            if (wordList.GetComponent<TMP_Text>().text == "Пусто")
                wordList.GetComponent<TMP_Text>().text = actualVocabulary.Rows[row][0].ToString();
            else
                wordList.GetComponent<TMP_Text>().text = wordList.GetComponent<TMP_Text>().text + "\n" + actualVocabulary.Rows[row][0].ToString();
        }       

    }

    private DataTable GetActiveWordsList()
    {
        DataTable actualVocabulary;

        string query = ("SELECT eng FROM words;");
        actualVocabulary = WorkWithDataBase.GetTable(query, actualDataBaseName);

        return actualVocabulary;
    }

    public void ClearActiveVoculabraryUI()
    {
        //очистить интерфейс словаря
        GameObject wordList;          
        wordList = GameObject.Find("VoculabraryInterfaceWordsList");
        wordList.GetComponent<TMP_Text>().text = "Пусто";
    }

}