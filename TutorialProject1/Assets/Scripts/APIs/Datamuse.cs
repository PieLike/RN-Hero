using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;

public class Datamuse : MonoBehaviour
{
    public enum reauestType{meanslike, triggers, both};

    public static List<Word> GetRequest(string word, reauestType type)
    {
        List<Word> wordsFrequency = new List<Word>();

        if (type == reauestType.meanslike || type == reauestType.both)
        {
            List<DatamuseResult> results = new List<DatamuseResult>();            

            var url = $"https://api.datamuse.com/words";
            var parameters = $"?ml={word}&md=p";       
            
            url = url + parameters;
            
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string responseDatamuse = reader.ReadToEnd();
                    
                    results = JsonConvert.DeserializeObject<List<DatamuseResult>>(responseDatamuse);

                    if (results != null)
                    {
                        DataTable dataTable;
                        Word wordFrequency;
                        Word.Pos pos;
                        
                        foreach (DatamuseResult result in results)
                        {                        
                            foreach (string tag in result.tags)
                            {
                                wordFrequency = new Word();

                                pos = FullPos(tag);
                                if (pos == Word.Pos.none)
                                    continue;
                                else if (pos == Word.Pos.These)
                                    dataTable = TranslationSave.GetDataTable("words","text", result.word.ToLower(),"","", true);
                                else
                                    dataTable = TranslationSave.GetDataTable("words","text", result.word.ToLower(), "pos", pos.ToString(), true);

                                wordFrequency = TranslationSave.DataTableToWord(dataTable);
                                if (wordFrequency != null)
                                    wordsFrequency.Add(wordFrequency); 
                            }
                        }
                    }
                }
            }  
        }

        if (type == reauestType.triggers || type == reauestType.both)
        {
            List<DatamuseResult> results = new List<DatamuseResult>();            

            var url = $"https://api.datamuse.com/words";
            var parameters = $"?rel_trg={word}&md=p";       
            
            url = url + parameters;
            
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string responseDatamuse = reader.ReadToEnd();
                    
                    results = JsonConvert.DeserializeObject<List<DatamuseResult>>(responseDatamuse);

                    if (results != null)
                    {
                        DataTable dataTable;
                        Word wordFrequency;
                        Word.Pos pos;
                        
                        foreach (DatamuseResult result in results)
                        {                        
                            foreach (string tag in result.tags)
                            {
                                wordFrequency = new Word();

                                pos = FullPos(tag);
                                if (pos == Word.Pos.none)
                                    continue;
                                else if (pos == Word.Pos.These)
                                    dataTable = TranslationSave.GetDataTable("words","text", result.word.ToLower(),"","", true);
                                else
                                    dataTable = TranslationSave.GetDataTable("words","text", result.word.ToLower(), "pos", pos.ToString(), true);

                                wordFrequency = TranslationSave.DataTableToWord(dataTable);
                                if (wordFrequency != null)
                                    wordsFrequency.Add(wordFrequency); 
                            }
                        }
                    }
                }
            }
        }

        if (wordsFrequency != null)
            return wordsFrequency;
        else
            return null;
    }
    
    [Serializable] public class DatamuseResult
    {
        [JsonProperty("word")]     public string word { get; set; }
        [JsonProperty("score")]    public int score { get; set; }
        [JsonProperty("tags")]    public IEnumerable<string> tags { get; set; }
    }
    /*[Serializable] public class Tag
    {
        
    }*/

    private static Word.Pos FullPos(string pos)
    {
        switch(pos)
        {
            case("n"): return Word.Pos.Noun;           //существительное
            case("v"): return Word.Pos.Verb;           //глагол
            case("adj"): return Word.Pos.Adjective;    //прилагательное
            case("adv"): return Word.Pos.Adverb;       //наречие
            case("u"): return Word.Pos.These;          //other использую these чтобы не париться
            default : return Word.Pos.none; 
        }
    }
}
