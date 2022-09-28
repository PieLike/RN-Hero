using System.Data;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Voculabrary : MonoBehaviour
{
    //public DataTable actualVocabulary, generalVocabulary;
    //private string actualDataBaseName = "vocabularyActual.bytes";//, generalDataBaseName = "vocabularyGeneral.bytes";
    private GameObject VIWordsList, VIPanel, VIExit;
    TMP_Text VIWordsListText;
    //private GameObject objDictionaryManager; 
    static DictionaryManager dictionaryManager;

    private void Start() 
    {
        //находим дочерние объекты
        VIPanel = transform.Find("VoculabraryInterfacePanel").gameObject;
        VIWordsList = transform.Find("VoculabraryInterfacePanel/VIWordsList").gameObject;
            VIWordsListText = VIWordsList.GetComponent<TMP_Text>();
        VIExit = transform.Find("VoculabraryInterfacePanel/VIExit").gameObject;
            VIExit.GetComponent<Button>().onClick.AddListener(Close); 

        //objDictionaryManager = transform.Find("DictionaryManager").gameObject;
        //dictionaryManager = objDictionaryManager.GetComponent<DictionaryManager>();
        dictionaryManager = FindObjectOfType<DictionaryManager>();
    }

    /*void OnApplicationQuit()
    {
        //очищаем бд активного словаря перед выключением
        string query = ("DELETE FROM words;");
        WorkWithDataBase.ExecuteQueryWithoutAnswer(query, actualDataBaseName);        
    }*/

    public void VoculabraryButton()
    {
        //вызываем окно словаря по нажатию кнопки
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
        //заполнить окно словаря
        foreach (Word word in dictionaryManager.words)
        {
            if (VIWordsListText.text == "Пусто")
                VIWordsListText.text = word.name;
            else
                VIWordsListText.text = VIWordsListText.text + "\n" + word.name;
        }       
    }
    /*public void FillActiveVoculabraryUI()
    {
        //заполнить окно словаря
        DataTable actualVocabulary = GetActiveWordsList();
        for (int row = 0; row < actualVocabulary.Rows.Count; row++)
        {
            if (VIWordsListText.text == "Пусто")
                VIWordsListText.text = actualVocabulary.Rows[row][0].ToString();
            else
                VIWordsListText.text = VIWordsListText.text + "\n" + actualVocabulary.Rows[row][0].ToString();
        }       
    }*/

    /*private DataTable GetActiveWordsList()
    {
        DataTable actualVocabulary;

        string query = ("SELECT eng FROM words;");
        actualVocabulary = WorkWithDataBase.GetTable(query, actualDataBaseName);

        return actualVocabulary;
    }*/

    public void ClearActiveVoculabraryUI()
    {
        //очистить интерфейс словаря
        VIWordsListText.text = "Пусто";
    }

    public void Close()
    {
        ClearActiveVoculabraryUI();
        VIPanel.SetActive(false);    
    }   

    public static void AddWordInDataBase(GameObject word)
    {
        string itemName = word.GetComponent<TMP_Text>().text.ToLower();
        AddWordInDataBase(itemName);       
    }
    public static void AddWordInDataBase(string word)
    {
        if (dictionaryManager.AddWord(word))
        {
            ReCountWordActualCount();
        }         
    }    

    public static void ReCountWordActualCount()
    {
        HeroMainData.wordActualCount = dictionaryManager.words.Length;
    }

    /*public static void AddWordInDataBase(GameObject interactionObject)
    {
        //ищем в общей дб словаря слово и записываем его в активный словарь
        string actualDataBaseName = "vocabularyActual.bytes", generalDataBaseName = "vocabularyGeneral.bytes";
        string itemName = interactionObject.GetComponent<TMP_Text>().text.ToLower();

        //делаем запрос на строку с нужным словом в общем словаре
        DataTable generalVocabulary = WorkWithDataBase.GetTable($"SELECT * FROM words WHERE eng = '{itemName}';", generalDataBaseName);

        //записываем слово из общего словаря в активный
        WorkWithDataBase.InsertOneRow(actualDataBaseName, generalVocabulary, "words");  


    }*/

    /*public static bool CheckExisting(GameObject interactionObject)
    {
        //проверяем есть ли такой объект у героя (в словаре)
        string actualDataBaseName = "vocabularyActual.bytes";
        string itemName = interactionObject.GetComponent<TMP_Text>().text.ToLower();

        //делаем запрос на строку с нужным словом в актуальном словаре
        DataTable actualVocabulary = WorkWithDataBase.GetTable($"SELECT * FROM words WHERE eng = '{itemName}';", actualDataBaseName);

        foreach(DataRow row in actualVocabulary.Rows)
        {
            return true;
        }
        return false;
    }*/ 
}