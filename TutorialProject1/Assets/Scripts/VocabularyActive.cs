using System.Data;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VocabularyActive : MonoBehaviour
{
    //public DataTable actualVocabulary, generalVocabulary;
    private string actualDataBaseName = "vocabularyActual.bytes";//, generalDataBaseName = "vocabularyGeneral.bytes";
    private GameObject VIWordsList, VIPanel, VIExit;

    private void Start() 
    {
        //находим дочерние объекты
        VIPanel = transform.Find("VoculabraryInterfacePanel").gameObject;
        VIWordsList = transform.Find("VoculabraryInterfacePanel/VIWordsList").gameObject;
        VIExit = transform.Find("VoculabraryInterfacePanel/VIExit").gameObject;
            VIExit.GetComponent<Button>().onClick.AddListener(Close); 
    }

    void OnApplicationQuit()
    {
        //очищаем дб активного словаря перед выключением
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, actualDataBaseName);        
    }

    public void VoculabraryButton()
    {
        if(VIPanel.activeSelf == false)
        {
            VIPanel.SetActive(true);
            FillActiveVoculabraryUI();
        }
        else
        {
            ClearActiveVoculabraryUI();
            VIPanel.SetActive(false);
        }
    }

    public void FillActiveVoculabraryUI()
    {
        //заполнить интерфейс словаря
        DataTable actualVocabulary = GetActiveWordsList();
        for (int row = 0; row < actualVocabulary.Rows.Count; row++)
        {
            if (VIWordsList.GetComponent<TMP_Text>().text == "Пусто")
                VIWordsList.GetComponent<TMP_Text>().text = actualVocabulary.Rows[row][0].ToString();
            else
                VIWordsList.GetComponent<TMP_Text>().text = VIWordsList.GetComponent<TMP_Text>().text + "\n" + actualVocabulary.Rows[row][0].ToString();
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
        VIWordsList.GetComponent<TMP_Text>().text = "Пусто";
    }

    public void Close()
    {
        ClearActiveVoculabraryUI();
        VIPanel.SetActive(false);    
    }
}