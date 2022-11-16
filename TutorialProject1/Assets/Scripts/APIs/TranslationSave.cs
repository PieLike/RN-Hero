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

    public static void SaveAllInDB()
    {
        api_key = GetApiKey();
        if (api_key == "")
        {
            Debug.Log("апи ключ пустой");
            return;
        }

        DataTable fre = GetFre();
        Word newWordFrequency = new Word();
        //string word, pos, colloq;
        //int frequency;

        foreach(DataRow row in fre.Rows)
        {
            newWordFrequency.word = row["WORD"].ToString().Replace(" ","").ToLower();
            newWordFrequency.frequency = Convert.ToInt32(row["RANK"]);
            string charPos = row["PoS"].ToString().Replace(" ","");

            if (newWordFrequency.word.Contains("("))
            {
                newWordFrequency.word = newWordFrequency.word.Replace("(","").Replace(")","");
                newWordFrequency.colloq = true;
            }
            else
                newWordFrequency.colloq = false;

            Word.Pos fullPos = FullPos(charPos);
            if (fullPos == Word.Pos.none)
            {
                Debug.Log("невозможно определить часть речи " + fullPos.ToString());
                return;
            }
            else    
                newWordFrequency.pos = fullPos;

            //SaveWord(word, frequency, pos, colloq);
            SaveWord(newWordFrequency);
        }
    }
    private static void SaveWord(Word newWordFrequency)//(string word, int frequency = 0, string pos = "", string colloq = "0")
    {
        if (newWordFrequency.word == "" || newWordFrequency.pos == Word.Pos.none || newWordFrequency.frequency == 0)
            return;        

        if (CheckExistance("words", "text", newWordFrequency.word, "Pos", newWordFrequency.pos.ToString()) == false)  
        {
            string stringColloq;
            if (newWordFrequency.colloq == true)
                stringColloq = "1";
            else 
                stringColloq = "0";
            //запись Word
            dict = new Dictionary<string, string>
            {
                { "text", newWordFrequency.word },
                { "FrequencyInt", newWordFrequency.frequency.ToString() },
                { "Pos", newWordFrequency.pos.ToString() },
                { "ColloqInt", stringColloq }
            };        
            
            DbInsert("words", dict);
            int wordId = GetId("words", "text", newWordFrequency.word, "pos", newWordFrequency.pos.ToString());
            if (wordId == 0)
            {
                Debug.Log("id равен 0");
                return;
            }

            IEnumerable<APIRequest.Defenition> translation = APIRequest.GetYandex(newWordFrequency.word, api_key);

            if (translation != null)
            {
                foreach(APIRequest.Defenition defenitions in translation)
                {
                    //if (CheckExistance("defenitions", "text", defenitions.text, "Pos", defenitions.pos) == false)  
                    //{
                        //запись Dictionary
                        /*dict = new Dictionary<string, string>
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
                        }*/
                        Word.Pos defPos = FullPos(defenitions.pos);
                        if (defPos != Word.Pos.none && defenitions.tr != null && PosAccord(defPos, newWordFrequency.pos) == true)
                        {
                            //запись Translation
                            foreach(APIRequest.Translation translate in defenitions.tr)
                            {
                                //if (CheckExistance("translations", "text", translate.text, "Pos", translate.pos) == false)  
                                //{
                                if (translate.text != "" && translate.pos != "")
                                {
                                    dict = new Dictionary<string, string>
                                    {
                                        { "Text", translate.text },
                                        { "Pos", defPos.ToString() },//FullPos(translate.pos).ToString() },
                                        { "Gen", translate.gen },
                                        { "Fr", translate.fr },
                                        { "ParentTs", defenitions.ts },
                                        { "ParentWordInt", wordId.ToString() }
                                    };        
                                    
                                    DbInsert("translations", dict); 
                                    int translationId = GetId("translations", "text", translate.text, "parentword", wordId.ToString());
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
                                }
                            }
                        }
                    //}
                    //else
                    //    Debug.Log("Слово " + defenitions.text + " " + defenitions.pos + " уже записано");
                } 
            }  
        } 
        else
            Debug.Log("Слово " + newWordFrequency.word + " " + newWordFrequency.pos + " уже записано");   
    }

    private static Word.Pos FullPos(string pos)
    {
        if (pos == null)
            return Word.Pos.none;


        switch(pos.ToUpper())
        {
            case("N"): case ("NOUN"):           return Word.Pos.Noun;           //существительное
            case("V"): case ("VERB"):           return Word.Pos.Verb;           //глагол
            case("J"): case ("ADJECTIVE"):      return Word.Pos.Adjective;      //прилагательное
             case("D"): case ("DETERMIANER"):   return Word.Pos.Determiner;     //определитель                    //Numeral, Noun, Pronoun
            case("R"): case ("ADVERB"):         return Word.Pos.Adverb;         //наречие
            case("P"): case ("PRONOUN"):        return Word.Pos.Pronoun;        //местоимение
             case("A"): case ("ARTICLE"):       return Word.Pos.Article;        //артикль            
            case("C"): case ("CONJUNCTION"):    return Word.Pos.Conjunction;    //союз 
            case("U"): case ("INTERJECTION"):   return Word.Pos.Interjection;   //междометие  
             case("I"): case ("PREPOSITION"):   return Word.Pos.Preposition;   //предлоги            
             case("M"): case ("NUMERAL"):       return Word.Pos.Numeral;       //числительное
             case("X"): return Word.Pos.Not;            //NOT N'T
             case("T"): return Word.Pos.To;             //TO A TAE
             case("E"): return Word.Pos.These;          //these
            default : return Word.Pos.none; 
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

    public static DataTable GetDataTable(string tableName, string column, string value, string column2 = "", string value2 = "", bool searchForMultiply = false)
    {
        DataTable DbForCheck;
        if (searchForMultiply == true && (value.EndsWith('s') || value.EndsWith('S')))
        {
            string valueWithoutS;
            if (value.EndsWith("es") || value.EndsWith("ES"))
                valueWithoutS = value.Remove(value.Length - 2);
            else
                valueWithoutS = value.Remove(value.Length - 1);

            if (column2 == "")
                DbForCheck = WorkWithDataBase.GetTable($"SELECT * FROM {tableName} WHERE ({column} = '{ReplaceQuotes(value)}' OR {column} = '{ReplaceQuotes(valueWithoutS)}') AND {column2} = '{ReplaceQuotes(value2)}';", DateBaseName);
            else
                DbForCheck = WorkWithDataBase.GetTable($"SELECT * FROM {tableName} WHERE {column} = '{ReplaceQuotes(value)}' AND {column2} = '{ReplaceQuotes(value2)}';", DateBaseName);  

        }
        else
        {
            if (column2 == "")
                DbForCheck = WorkWithDataBase.GetTable($"SELECT * FROM {tableName} WHERE {column} = '{ReplaceQuotes(value)}';", DateBaseName);
            else
                DbForCheck = WorkWithDataBase.GetTable($"SELECT * FROM {tableName} WHERE {column} = '{ReplaceQuotes(value)}' AND {column2} = '{ReplaceQuotes(value2)}';", DateBaseName);
        }
        return DbForCheck;
    }

    private static bool CheckExistance(string tableName, string column, string value, string column2 = "", string value2 = "")
    {
        DataTable DbForCheck = GetDataTable(tableName, column, value, column2, value2);        

        if (DbForCheck.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }    

    private static int GetId(string tableName, string column1, string value1, string column2 = "", string value2 = "")
    {
        DataTable DbForCheck = GetDataTable(tableName, column1, value1, column2, value2);  

        if (DbForCheck.Rows.Count > 0)
        {
            int id = Convert.ToInt32(DbForCheck.Rows[0][0]);
            return id;
        }
        return 0;  
    }

    public static Word DataTableToWord(DataTable dataTable)
    {
        Word word = new Word();
        if (dataTable.Rows.Count > 0)
        {
            word.word = dataTable.Rows[0]["Text"].ToString();
            word.pos = (Word.Pos) Enum.Parse(typeof(Word.Pos), dataTable.Rows[0]["Pos"].ToString());
            word.frequency = Convert.ToInt32(dataTable.Rows[0]["Frequency"]);
            string stringColloq = dataTable.Rows[0]["Colloq"].ToString();
            if (stringColloq == "1")
                word.colloq = true;
            else
                word.colloq = false;

            return word;
        }
        else return null;
    }

    private static string ReplaceQuotes(string text)
    {
        if (text == null)
            return "";
        if (text != "")
            text = text.Replace("'", "''");
        return text;
    }

    private static bool PosAccord(Word.Pos defPos, Word.Pos wordPos)
    {
        switch (wordPos)
        {
            case Word.Pos.Noun:
                if (defPos == Word.Pos.Noun) return true;
                else return false;
            case Word.Pos.Adjective:
                if (defPos == Word.Pos.Adjective) return true;
                else return false;
            case Word.Pos.Verb:
                if (defPos == Word.Pos.Verb) return true;
                else return false;
            case Word.Pos.Adverb:
                if (defPos == Word.Pos.Adverb) return true;
                else return false;

            case Word.Pos.Pronoun:
                if (defPos == Word.Pos.Pronoun) return true;
                else return false;
            case Word.Pos.Preposition:
                if (defPos == Word.Pos.Preposition) return true;
                else return false;
            case Word.Pos.Conjunction:
                if (defPos == Word.Pos.Conjunction) return true;
                else return false;
            case Word.Pos.Numeral:
                if (defPos == Word.Pos.Numeral) return true;
                else return false;

            case Word.Pos.Determiner:
                if (defPos == Word.Pos.Noun ||
                    defPos == Word.Pos.Pronoun ||
                    defPos == Word.Pos.Article ||
                    defPos == Word.Pos.Numeral) return true;
                else return false;

            default:
                return false;
        }
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
