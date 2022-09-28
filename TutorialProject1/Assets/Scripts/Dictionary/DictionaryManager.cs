using System;
using System.Data;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager instance;

    public Word[] words;
    private void Awake() 
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

    public bool AddWord(Word newWord)
    {
        if (words != null)
        {
            if (CheckExisting(newWord) == false)
            {
                Word[] newWords = new Word[words.Length + 1];
                for (int i = 0; i < words.Length; i ++)
                {
                    newWords[i] = words[i];
                }
                newWords[words.Length] = newWord;

                words = newWords;
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.Log("words = null");
            return false;
        }
    }
    public bool AddWord(string stringNewWord)
    {
        
        Word newWord = new Word{ name = stringNewWord };
        return AddWord(newWord);
    }

    public bool DeleteWord(string newWord)  //обязательно сдл
    {
        if (words != null)
        {
            if (CheckExisting(newWord) == true)
            {
                if (words.Length == 1)
                    words = new Word[0]; 

                Word[] newWords = new Word[words.Length - 1];
                for (int i = 0; i < words.Length; i ++)
                {
                    if (words[i].name == newWord)
                        continue;
                    newWords[i] = words[i];
                }

                words = newWords;
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.Log("words = null");
            return false;
        }
    }
    public bool DeleteWord(Word newWord)
    {
        string stringNewWord = newWord.name;
        return DeleteWord(stringNewWord);
    }

    public bool CheckExisting(GameObject word)
    {
        string itemName = word.GetComponent<TMPro.TMP_Text>().text.ToLower();
        return CheckExisting(itemName);
    }
    public bool CheckExisting(string stringNewWord)
    {
        Word newWord = new Word{ name = stringNewWord };
        return CheckExisting(newWord);
    }
    public bool CheckExisting(Word itemName)
    {
        foreach (Word word in words)
        {
            if(itemName == word)
            {
                return true;  
            }
        }
        return false;
    }
    public void AddWordsWithClear(DataTable dataTable)
    {
        Clear();

        if (dataTable.Rows.Count != 0)
        {
            Word[] newWords = new Word[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string stringNewWord = dataTable.Rows[i]["name"].ToString();
                Word newWord = new Word{ name = stringNewWord };
                newWords[i] = newWord;
            }

            words = newWords;
        }
        else
            Debug.Log("база данных с сохраненными словами пустая");
    }  
    public void Clear()
    {
        if (words != null)
        {            
            words = new Word[0];
        }
        else
        {
            Debug.Log("words = null");
        }
    }  

    public void SaveWordsToDataBase(string nameDataBase)
    {
        string tableName = "words";
        string columns = "name";
        string values;
        
        foreach (Word word in words)
        {
            values = word.name;
            string insertQuery = $"INSERT INTO {tableName} {columns} VALUES {values};";
            WorkWithDataBase.ExecuteQueryWithoutAnswer(insertQuery, nameDataBase);
        }      
    }
}
