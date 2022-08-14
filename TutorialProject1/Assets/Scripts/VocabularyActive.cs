using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;

public class VocabularyActive : MonoBehaviour
{
    //public DataTable actualVocabulary, generalVocabulary;
    private string actualDataBaseName = "vocabularyActual.bytes", generalDataBaseName = "vocabularyGeneral.bytes";
    public GameObject voculabraryInterface, wordListInterface;
    

    private void Start()
    {
        
    }

    void OnApplicationQuit()
    {
        //очищаем дб активного словаря перед выключением
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, actualDataBaseName);        
    }

    public void VoculabraryButton()
    {
        if(voculabraryInterface.activeSelf == false)
        {
            voculabraryInterface.SetActive(true);
            FillActiveVoculabraryUI();
        }
        else
        {
            ClearActiveVoculabraryUI();
            voculabraryInterface.SetActive(false);
        }
    }

    public void FillActiveVoculabraryUI()
    {
        //заполнить интерфейс словаря
        DataTable actualVocabulary = GetActiveWordsList();
        for (int row = 0; row < actualVocabulary.Rows.Count; row++)
        {
            if (wordListInterface.GetComponent<TMP_Text>().text == "Пусто")
                wordListInterface.GetComponent<TMP_Text>().text = actualVocabulary.Rows[row][0].ToString();
            else
                wordListInterface.GetComponent<TMP_Text>().text = wordListInterface.GetComponent<TMP_Text>().text + "\n" + actualVocabulary.Rows[row][0].ToString();
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
        wordListInterface.GetComponent<TMP_Text>().text = "Пусто";
    }

}