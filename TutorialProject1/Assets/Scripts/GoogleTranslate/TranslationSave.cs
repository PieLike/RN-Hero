using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using System.Linq;

public class TranslationSave : MonoBehaviour
{
    static string DateBaseName = "Dictionary.bytes", api_key;
    static Dictionary<string, string> dict;

    private static DataTable GetFre()
    {
        DataTable DbForCheck = WorkWithDataBase.GetTable($"SELECT * FROM fre;", DateBaseName);  
        if (DbForCheck.Rows.Count > 0)
        {
            return DbForCheck;
        }
        return null;    
    }

    public static void SaveInDB()
    {
        api_key = GetApiKey();
        if (api_key == "")
        {
            Debug.Log("апи ключ пустой");
            return;
        }

        DataTable fre = GetFre();
        string word, pos, colloq;
        int frequency;

        foreach(DataRow row in fre.Rows)
        {
            word = row["WORD"].ToString().Replace(" ","");
            frequency = Convert.ToInt32(row["RANK"]);
            pos = row["PoS"].ToString().Replace(" ","");

            if (word.Contains("("))
            {
                word = word.Replace("(","").Replace(")","");
                colloq = "1";
            }
            else
                colloq = "0";



            SaveWord(word, frequency, pos, colloq);
        }
    }
    private static void SaveWord(string word, int frequency = 0, string pos = "", string colloq = "0")
    {
        //string word = "pizza";
        //int frequency = 0;
        if (word == "" || pos == "" || frequency == 0)
            return;

        string fullPos = FullPos(pos);
        if (fullPos == "")
        {
            Debug.Log("невозможно определить часть речи " + fullPos);
            return;
        }

        if (CheckExistance("words", "text", word, "Pos", fullPos) == false)  
        {
            //запись Word
            dict = new Dictionary<string, string>
            {
                { "text", word },
                { "FrequencyInt", frequency.ToString() },
                { "Pos", fullPos },
                { "ColloqInt", colloq }
            };        
            
            DbInsert("words", dict);
            int wordId = GetId("words", "text", word);
            if (wordId == 0)
            {
                Debug.Log("id равен 0");
                return;
            }

            IEnumerable<APIRequest.Defenition> translation = APIRequest.GetYandex(word, api_key);

            if (translation != null)
            {
                foreach(APIRequest.Defenition defenitions in translation)
                {
                    Debug.Log(defenitions.text); 

                    if (CheckExistance("defenitions", "text", defenitions.text, "Pos", defenitions.pos) == false)  
                    {
                        //запись Dictionary
                        dict = new Dictionary<string, string>
                        {
                            { "text", defenitions.text },
                            { "pos", defenitions.pos },
                            { "ts", defenitions.ts },
                            { "gen", defenitions.gen },
                            { "fr", defenitions.fr },
                            { "ParentWordInt", wordId.ToString() }
                        };        
                        
                        DbInsert("defenitions", dict);
                        int defenitionId = GetId("defenitions", "text", defenitions.text);
                        if (defenitionId == 0)
                        {
                            Debug.Log("id равен 0");
                            return;
                        }
                        if (defenitions.tr != null)
                        {
                            //запись Translation
                            foreach(APIRequest.Translation translate in defenitions.tr)
                            {
                                //if (CheckExistance("translations", "text", translate.text, "Pos", translate.pos) == false)  
                                //{
                                    dict = new Dictionary<string, string>
                                    {
                                        { "text", translate.text },
                                        { "pos", translate.pos },
                                        { "gen", translate.gen },
                                        { "fr", translate.fr },
                                        { "ParentDefinitionInt", defenitionId.ToString() }
                                    };        
                                    
                                    DbInsert("translations", dict); 
                                    int translationId = GetId("translations", "text", translate.text);
                                    if (translationId == 0)
                                    {
                                        Debug.Log("id равен 0");
                                        return;
                                    }

                                    //запись Meaning
                                    if (translate.mean != null)
                                    {
                                        foreach(APIRequest.JustText meaning in translate.mean)
                                        {
                                            //if (CheckExistance("meanings", "text", meaning.text) == false)  
                                            //{
                                                dict = new Dictionary<string, string>
                                                {
                                                    { "text", meaning.text },
                                                    { "ParentTranslationInt", translationId.ToString() }
                                                };
                                                DbInsert("meanings", dict);
                                            //}
                                        }
                                    }

                                    //запись Synonyms
                                    if (translate.syn != null)
                                    {
                                        foreach(APIRequest.JustText synonym in translate.syn)
                                        {
                                            //if (CheckExistance("synonyms", "text", synonym.text) == false)  
                                            //{
                                                dict = new Dictionary<string, string>
                                                {
                                                    { "text", synonym.text },
                                                    { "ParentTranslationInt", translationId.ToString() }
                                                };
                                                DbInsert("synonyms", dict);
                                            //}
                                        } 
                                    }    

                                    //запись Example
                                    if (translate.ex != null)
                                    {
                                        foreach(APIRequest.Example example in translate.ex)
                                        {                            
                                            //if (CheckExistance("examples", "text", example.text) == false)  
                                            //{                                               
                                                if (example.tr != null)
                                                {
                                                    string exampleTranslation = "";
                                                    foreach(APIRequest.JustText text in example.tr)
                                                        exampleTranslation = text.text;  

                                                    dict = new Dictionary<string, string>
                                                    {
                                                        { "text", example.text },
                                                        { "Translation", exampleTranslation },
                                                        { "ParentTranslationInt", translationId.ToString() }
                                                    };        
                                                
                                                    DbInsert("examples", dict);
                                                }
                                            //}
                                        }
                                    }

                                //}
                            }
                        }
                    }
                    else
                        Debug.Log("Слово " + defenitions.text + " " + defenitions.pos + " уже записано");
                } 
            }  
        } 
        else
            Debug.Log("Слово " + word + " " + fullPos + " уже записано");   
    }

    private static string FullPos(string pos)
    {
        switch(pos)
        {
            case("N"): return "Noun";           //существительное
            case("V"): return "Verb";           //глагол
            case("J"): return "Adjective";      //прилагательное
            case("D"): return "Determiner";     //определитель
            case("R"): return "Adverb";         //наречие
            case("P"): return "Pronoun";        //местоимение
            case("A"): return "Article";        //артикль            
            case("C"): return "Conjunction";    //союз 
            case("U"): return "Interjection";   //междометие  
            case("I"): return "Prepositions";   //предлоги            
            case("M"): return "Numerals";       //числительное
            case("X"): return "Not";            //NOT N'T
            case("T"): return "To";             //TO A TAE
            case("E"): return "These";          //these
            default : return ""; 
        }
    }

    private static void DbInsert(string tableName, Dictionary<string, string> dict)
    {
        string insertQuery = $"INSERT INTO " + tableName + "(местополей) VALUES (местозначений);", columnsList = "", valuesList = "";
        foreach (var el in dict)
        {            
            if (columnsList == "")
            {
                if (el.Key.Contains("Int"))
                {                    
                    columnsList = el.Key.Replace("Int", "");
                    valuesList = ReplaceQuotes(el.Value);
                }
                else
                {
                    columnsList = el.Key;
                    valuesList = "'" + ReplaceQuotes(el.Value) + "'";
                }
            }
            else
            {
                if (el.Key.Contains("Int"))
                {                    
                    columnsList = columnsList + "," + el.Key.Replace("Int", "");
                    valuesList = valuesList + "," + ReplaceQuotes(el.Value);
                }
                else
                {
                    columnsList = columnsList + "," + el.Key;
                    valuesList = valuesList + "," + "'" + ReplaceQuotes(el.Value) + "'";
                }
            }
        }
        insertQuery = insertQuery.Replace("местополей", columnsList);        
        insertQuery = insertQuery.Replace("местозначений", valuesList);
        //Debug.Log(insertQuery);
        WorkWithDataBase.ExecuteQueryWithoutAnswer(insertQuery, DateBaseName);
    }

    private static bool CheckExistance(string tableName, string column, string value, string column2 = "", string value2 = "")
    {
        DataTable DbForCheck;
        //Debug.Log($"SELECT * FROM {ReplaceQuotes(tableName)} WHERE {column} = '{value}';");
        if (column2 == "")
            DbForCheck = WorkWithDataBase.GetTable($"SELECT * FROM {tableName} WHERE {column} = '{ReplaceQuotes(value)}';", DateBaseName);
        else
            DbForCheck = WorkWithDataBase.GetTable($"SELECT * FROM {tableName} WHERE {column} = '{ReplaceQuotes(value)}' AND {column2} = '{ReplaceQuotes(value2)}';", DateBaseName);

        if (DbForCheck.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    private static string ReplaceQuotes(string text)
    {
        if (text == null)
            return "";
        if (text != "")
            text = text.Replace("'", "''");
        return text;
    }

    private static int GetId(string tableName, string column, string value)
    {
        DataTable DbForCheck = WorkWithDataBase.GetTable($"SELECT Id FROM {tableName} WHERE {column} = '{ReplaceQuotes(value)}';", DateBaseName);  
        if (DbForCheck.Rows.Count > 0)
        {
            int id = Convert.ToInt32(DbForCheck.Rows[0][0]);
            return id;
        }
        return 0;  
    }

    private static string GetApiKey()
    {
        String line;
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader("C:\\Users\\PieLike\\Desktop\\API key.txt");
            //Read the first line of text
            line = sr.ReadLine();
            //Continue to read until you reach end of file
            if (line != null)
            {
                sr.Close();
                return line;
            }
            else
            {
                sr.Close();
                return "";
            }    
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }

        return "";
    }
}
