using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DictDataBaseRequests : MonoBehaviour
{
    private static DictionaryManager dictionaryManager;    
    void Start()
    {
        if (dictionaryManager == null ) dictionaryManager = FindObjectOfType<DictionaryManager>();
    }

    public static List<Word> GetWordsByLevel(int level = 0)
    {
        int skipFirstWords = 100;
        int factor = 100;
        int levelForMath = level;

        if (levelForMath == 0)
            levelForMath = HeroMainData.level;

        if (levelForMath == 0)
            levelForMath = 1;
            

        return GetWordsByFrequency((levelForMath-1)*factor +skipFirstWords, (levelForMath)*factor +skipFirstWords, 10); 
    }

    static List<Word> GetWordsByFrequency(int intMin, int intMax, int count)
    {
        List<Word> newWords = new List<Word>();

        //заполняем массив слов словами из общей базы данных по parent = имея объекта
        string wordsDataBaseName = "Dictionary.bytes";
        string query = ($"SELECT Text, Pos, Frequency FROM words WHERE Frequency > {intMin} AND Frequency < {intMax} AND (Pos = 'Noun' OR Pos = 'Verb' OR Pos = 'Adjective') ");              
        DataTable sonsDataTable = WorkWithDataBase.GetTable(query, wordsDataBaseName);   

        foreach (DataRow row in sonsDataTable.Rows)
        {
            if (newWords.Count >= count) 
                break;

            Word newWord = new Word();
            newWord.word = row["Text"].ToString(); 

            if (dictionaryManager == null ) dictionaryManager = FindObjectOfType<DictionaryManager>();
            if (dictionaryManager.CheckExisting(newWord) != (-1))
                break;

            newWord.pos = (Word.Pos) Enum.Parse(typeof(Word.Pos), row["Pos"].ToString()); 
            newWord.frequency = Convert.ToInt32(row["Frequency"]);
            newWords.Add(newWord);  
        }

        return newWords;
    }

}
