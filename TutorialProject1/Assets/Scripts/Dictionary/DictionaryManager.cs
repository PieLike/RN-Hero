using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager instance;

    public List<Word> words;
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

    public bool AddWord(Word newWord, bool fillWordData = false)
    {
        //if (words != null)
        //{
            int numberWord = CheckExisting(newWord);
            if (words == null || numberWord == (-1))
            {
                if (fillWordData && newWord.filled == false)
                    newWord = FillWordData(newWord);
                newWord.learnCount = 1;

                if (words == null) words = new List<Word>();
                words.Add(newWord);
                Experience.AddExp(MainVariables.expForWord); //expForNewWord
                return true;
            }
            else if (numberWord != (-1))
            {
                //AddLearnCount(newWord);
                words[numberWord].learnCount ++;
                Experience.AddExp(MainVariables.expForWord);
                return false;
            }
            return false;
        /*}
        else
        {
            Debug.Log("words = null");
            return false;
        }*/
    }
    public bool AddWord(Loot lootNewWord, bool fillWordData = false)
    {
        
        Word newWord = lootNewWord.word;
        return AddWord(newWord, fillWordData);
    }

    public bool DeleteWord(Word newWord)//(string newWord) 
    {
        if (words != null)
        {
            int numberWord = CheckExisting(newWord);
            if (numberWord != (-1))
            {
                words.Remove(words[numberWord]);

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
    public int CheckExisting(Word _word)
    {
        if (words == null) return (-1);
        return words.FindIndex( delegate(Word word){ return word.word == _word.word; } );
    }
    public void AddWordsWithClear(DataTable dataTable)
    {
        Clear();

        if (dataTable.Rows.Count != 0)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string stringNewWord = dataTable.Rows[i]["text"].ToString();
                int newlearnCount = dataTable.Rows[i]["count"].ToString() == "" ? 0 : Convert.ToInt32(dataTable.Rows[i]["count"]);
                Word newWord = new Word{ word = stringNewWord, learnCount = newlearnCount };
                newWord = FillWordData(newWord);

                if (words == null) words = new List<Word>();
                words.Add(newWord);
            }
        }
        else
            Debug.Log("база данных с сохраненными словами пустая");
    }  
    public void Clear()
    {
        if (words != null)
        {            
            words.Clear();
        }
        else
        {
            Debug.Log("words = null");
        }
    }  

    public void SaveWordsToDataBase(string nameDataBase)
    {
        string tableName = "words";
        string columns = "text, count";
        string values;
        
        foreach (Word word in words)
        {
            values = "'" + word.word + "'," + word.learnCount;
            string insertQuery = $"INSERT INTO {tableName} {columns} VALUES {values};";
            WorkWithDataBase.ExecuteQueryWithoutAnswer(insertQuery, nameDataBase);
        }      
    }

    /*public bool AddLearnCount(string newWord)
    {
        if (words != null)
        {
            int foundedWord = words.FindIndex( delegate(Word word){ return word.word == newWord; } );
            if (foundedWord != (-1))
            {
                words[foundedWord].learnCount ++;
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
    public bool AddLearnCount(GameObject word)
    {
        string itemName = word.GetComponent<TMPro.TMP_Text>().text.ToLower();
        return AddLearnCount(itemName);
    }
    public bool AddLearnCount(Word word)
    {
        string itemName = word.word;
        return AddLearnCount(itemName);
    }*/

    public Word FillWordData(Word word)    
    {
        //Debug.Log(word.word + ", " + word.pos);
        DataTable saveVocabulary;
        string data = "words.Text as Text, words.Pos as Pos, translations.Text as Translation, words.Frequency as Frequency, words.Colloq as Colloq, translations.Fr as TranslationFrequency"; 
        saveVocabulary = WorkWithDataBase.GetTable($"SELECT {data} FROM words LEFT JOIN translations ON words.Id = translations.ParentWord WHERE words.Text = '{word.word}' AND translations.Pos = '{word.pos.ToString()}' order by translations.fr DESC", "Dictionary.bytes");

        if(saveVocabulary.Rows.Count > 0)
        {
            word.translate = new List<string>();
            word.addictionTranslate = new List<string>();
            word.addictionTranslate2 = new List<string>();
            for (int i = 0; i < saveVocabulary.Rows.Count; i++)
            {
                //string translationFrequency = saveVocabulary.Rows[i]["TranslationFrequency"].ToString();
                int translationFrequency = Convert.ToInt32(saveVocabulary.Rows[i]["TranslationFrequency"]);
                if (translationFrequency == 10 || (translationFrequency == 5 && word.translate.Count == 0) || (translationFrequency == 1 && word.translate.Count == 0))
                    //word.translate[i] = saveVocabulary.Rows[i]["Translation"].ToString();  
                    word.translate.Add(saveVocabulary.Rows[i]["Translation"].ToString());
                else if(translationFrequency == 5 || (translationFrequency == 1 && word.addictionTranslate.Count == 0))
                    //word.addictionTranslate[i] = saveVocabulary.Rows[i]["Translation"].ToString();  
                    word.addictionTranslate.Add(saveVocabulary.Rows[i]["Translation"].ToString());
                else// if (translationFrequency == "1")  
                    //word.addictionTranslate2[i] = saveVocabulary.Rows[i]["Translation"].ToString();  
                    word.addictionTranslate2.Add(saveVocabulary.Rows[i]["Translation"].ToString());          
            }
            word.frequency = Convert.ToInt32(saveVocabulary.Rows[0]["Frequency"]);
            word.pos = (Word.Pos) Enum.Parse(typeof(Word.Pos), saveVocabulary.Rows[0]["Pos"].ToString());
            word.colloq = Convert.ToBoolean(Convert.ToInt32(saveVocabulary.Rows[0]["Colloq"]));

            FindIcon.DictStruct dictStruct = new FindIcon.DictStruct {  manager = this, wordElement = word};
            FindIcon.SetIcon(word.word, dictStruct);

            word.filled = true;
        }
        else
            Debug.Log("Перевода не найдено: " + word.word);
        
        return word;
    }

    public List<Word> ReturnByParent(string parent)
    {
        List<Word> words = new List<Word>();
        //заполняем массив слов словами из общей базы данных по parent = имея объекта
        string wordsDataBaseName = "Dictionary.bytes";
        string query = ($"SELECT words.Text, words.Pos FROM sons LEFT JOIN words On sons.SonId = words.Id, (SELECT words.Id FROM words WHERE words.Text = '{parent.ToLower()}') as ParentIdTable WHERE ParentId = ParentIdTable.Id");              
        DataTable sonsDataTable = WorkWithDataBase.GetTable(query, wordsDataBaseName);   

        foreach (DataRow row in sonsDataTable.Rows)
        {
            Word newWord = new Word();
            newWord.word = row["Text"].ToString(); 
            newWord.pos = (Word.Pos) Enum.Parse(typeof(Word.Pos), row["Pos"].ToString()); 
            words.Add(newWord);  
        }
        return words;
    }

    public Word TakeRandom()
    {
        if (words != null)
        {
            int random = UnityEngine.Random.Range(0, words.Count);
            return words[random];
        }
        else
        {
            Debug.Log("words = null");
            return null;
        }
    }

    public void SetIcon(Word _word, Sprite sprite)
    {
        int numberWord = CheckExisting(_word);
        if (numberWord != (-1))
            words[numberWord].icon = sprite;
    }

    public void LearnWord(Word word)
    {
        if (words != null)
        {
            int numberWord = CheckExisting(word);
            if (numberWord != (-1))
            {
                words[numberWord].learned = true;
                //words.Remove(foundedWord);
                Experience.AddExp(MainVariables.expForWordLearn);

                Voculabrary.ReCountWordActualCount(); 
            }
        }               
    }
}
