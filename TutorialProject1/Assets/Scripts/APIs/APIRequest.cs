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

public static class APIRequest 
{
    private static string ApiKey = "";
    //for google
#region
	public static async Task<IEnumerable<Sentence>> Get5Recipies(string word)
    {
        List<Sentence> recipes = new List<Sentence>();
        //Result recipeList;

        var url = $"https://clients5.google.com/translate_a/t";
        //var parameters = $"?client=dict-chrome-ru&sl=en&tl=ru&q={word}";

        Dictionary<string, string> Params = new Dictionary<string, string>()
        {
            {"",""}
        };

        HttpClient client = new HttpClient();
        FormUrlEncodedContent content = new FormUrlEncodedContent(Params);
        Uri uri = new Uri(url);
        //client.BaseAddress = new Uri(url);
        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.PostAsync(uri, content);//.ConfigureAwait(false);
        //response.ConfigureAwait(false);  

        Debug.Log(response.ToString());
       
        /*if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            recipeList = JsonConvert.DeserializeObject<Result>(jsonString);

            Debug.Log(jsonString.ToString());
            if (recipeList != null)
            {
                Debug.Log("gfgf");
                recipes.AddRange(recipeList.sentences);
            }
        }*/
        
        return recipes;
    }
#endregion

    //for yandex
    public static IEnumerable<Defenition> GetYandex(string word, string api_key = "")
    {
        ApiKey = api_key;
        if (ApiKey == "")
        {
            ApiKey = GetApiKey();
            if (ApiKey == "")
            {
                Debug.Log("Проблемы с чтением апи ключа");
                return null;
            }
        }
        //var url = $"https://translate.yandex.net/api/v1.5/tr/translate";
        //var url = $"https://translate.yandex.net/api/v1.5/tr.json/translate";
        //var url = $"https://translate.api.cloud.yandex.net/translate/v2/translate";
        //var url = $"https://yandextranslatezakutynskyv1.p.rapidapi.com/translate";
        var url = $"https://translate.api.cloud.yandex.net/translate/v1/translate";
        
        var YandexTask = GetRequest2(word);
        if (YandexTask == null)
        {
            Debug.Log("Запрос на апи ничего не возвращает");
            return null;
        }
        else
        {
            var translation = YandexTask.GetAwaiter().GetResult();
            return translation;
            //Debug.Log(translate.sentences.ToString());
            /*foreach(APIRequest.Defenition defenitions in translation)
            {
                Debug.Log(defenitions.text);
                return defenitions;
            }*/
        }
        //var answer = 
        //GetRequest(url);
        //var answerResult = answer.Result;
        //Debug.Log(JsonConvert.DeserializeObject<Result>(answer).ToString());
    }

    static async Task<JObject> GetRequest(string url)
    {     
        string[] texts = new string[]{"pizza", "time"};
        var JSON = new FormUrlEncodedContent(new[]
        {
            //{ "apiKey", ApiKey },
            new KeyValuePair<string,string>("folderId", "b1gjgoqi64qm7ogmba7l"),
            new KeyValuePair<string,string>("text", JsonConvert.SerializeObject(new {Values= texts})),
            new KeyValuePair<string,string>("target", "ru")
            //new KeyValuePair<string,string>("texts", JsonConvert.SerializeObject(new {Values= texts})),
            //new KeyValuePair<string,string>("targetLanguageCode", "ru")
        });  
        //var contentData = new StringContent(JsonConvert.SerializeObject(new {Values= texts}), Encoding.UTF8, "application/json");        
        
        HttpClient client = new HttpClient();
        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Headers =
            {
                { "Authorization", "Api-Key " + ApiKey },
                { "ContentType", "application/json" }
            },    
            Content = JSON
        };       

        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
        Debug.Log(response.ToString());

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Debug.Log(body.ToString());

        /*using (var response = await client.SendAsync(request).ConfigureAwait(false))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Debug.Log("424");
            Debug.Log(body.ToString());
        }*/
            
        return null;
    }

    public static async Task<IEnumerable<Defenition>> GetRequest2(string word)
    {
        List<Defenition> defenitions = new List<Defenition>();
        YandexResult fullTranslate;

        var url = $"https://dictionary.yandex.net/api/v1/dicservice.json/lookup";

        HttpClient client = new HttpClient();

        //string[] texts = new string[]{"pizza", "time"};
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string,string>("key", ApiKey ),
            new KeyValuePair<string,string>("text", word),
            new KeyValuePair<string,string>("lang", "en-ru")
        });  

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);
        Debug.Log("IsSuccessStatusCode = " + response.IsSuccessStatusCode.ToString() + ", " + response.ToString());
       
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            fullTranslate = JsonConvert.DeserializeObject<YandexResult>(jsonString);

            Debug.Log(jsonString.ToString());
            if (fullTranslate != null)
            {
                defenitions.AddRange(fullTranslate.def);
            }
        }
        
        return defenitions;
    }

    [Serializable] public class YandexResult
    {
        [JsonProperty("def")]     public IEnumerable<Defenition> def { get; set; }
    }
    [Serializable] public class Defenition
    {
        [JsonProperty("text")]     public string text { get; set; }
        [JsonProperty("pos")]     public string pos { get; set; }
        [JsonProperty("ts")]     public string ts { get; set; }
        [JsonProperty("tr")]     public IEnumerable<Translation> tr { get; set; }
        [JsonProperty("gen")]     public string gen { get; set; }
        [JsonProperty("fr")]     public string fr { get; set; }
    }
    [Serializable] public class Translation
    {
        [JsonProperty("text")]     public string text { get; set; }
        [JsonProperty("pos")]     public string pos { get; set; }
        [JsonProperty("gen")]     public string gen { get; set; }
        [JsonProperty("fr")]     public string fr { get; set; }
        [JsonProperty("syn")]     public IEnumerable<JustText> syn { get; set; }//public IEnumerable<Synonym> syn { get; set; }
        [JsonProperty("mean")]     public IEnumerable<JustText> mean { get; set; }
        [JsonProperty("ex")]     public IEnumerable<Example> ex { get; set; }
    }
    /*[Serializable] public class Synonym
    {
        [JsonProperty("text")]     public string text { get; set; }
        [JsonProperty("pos")]     public string pos { get; set; }
        [JsonProperty("syn")]     public Dictionary<string, string> syn { get; set; }
        [JsonProperty("mean")]     public Dictionary<string, string> mean { get; set; }
        [JsonProperty("ex")]     public List<IEnumerable<Example>> ex { get; set; }
    }*/
    [Serializable] public class Example
    {
        [JsonProperty("text")]     public string text { get; set; }
        [JsonProperty("tr")]     public IEnumerable<JustText> tr { get; set; }
    }
    [Serializable] public class JustText
    {
        [JsonProperty("text")]     public string text { get; set; }
    }

//}

    /*static async Task<JObject> GetRequest(string url, Dictionary<string, string> Params)
    {
        

        HttpClient client = new HttpClient();
        //try
        //{
            Uri uri = new Uri(url);
            //FormUrlEncodedContent content = new FormUrlEncodedContent(Params);
            //HttpResponseMessage response = await client.PostAsync(uri, content).ConfigureAwait(false);

            byte[] byteArray = Encoding.UTF8.GetBytes("lang=en-ru&text=pizza");
            var request = HttpWebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Api-Key " + ApiKey);
            request.ContentLength = byteArray.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteArray,0,byteArray.Length);
            }
            //stream.Close();

            try
            {
                WebResponse response = await request.GetResponseAsync().ConfigureAwait(false);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseContent = reader.ReadToEnd();
                    JObject adResponse =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(responseContent);

                    Debug.Log(adResponse.ToString());
                    return adResponse;
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (StreamReader reader = new StreamReader(webException.Response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                        Debug.Log(responseContent.ToString());
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(responseContent); ;
                    }
                }
            }
            return null;

            //return response;
        }
        catch(Exception error)
        {
            Debug.Log("my error: " + error.ToString());  
        }
        finally
        {
            client.Dispose();
        }*/

        //return null;
    //}

    [Serializable]
    public class Result
    {
        [JsonProperty("sentences")]     public IEnumerable<Sentence> sentences { get; set; }
        [JsonProperty("dict")]          public IEnumerable<Dict> dict { get; set; }
        [JsonProperty("src")]           public string src { get; set; }
        [JsonProperty("confidence")]    public string confidence { get; set; }
        //[JsonProperty("spell")]         public string spell { get; set; }
        [JsonProperty("ld_result")]     public IEnumerable<Ld_result> ld_result { get; set; }
    }
    [Serializable]
    public class Sentence
    {
        [JsonProperty("trans")]     public string trans { get; set; }
        [JsonProperty("orig")]      public string orig { get; set; }
        [JsonProperty("backend")]   public int backend { get; set; }
    }
    [Serializable]
    public class Dict
    {
        [JsonProperty("pos")]       public string pos { get; set; }
        //[JsonProperty("terms")]     public string terms { get; set; }
        //[JsonProperty("entry")]     public string entry { get; set; }
        [JsonProperty("base_form")] public string base_form { get; set; }
        [JsonProperty("pos_enum")]  public int pos_enum { get; set; }
    }
    [Serializable]
    public class Ld_result
    {
        //[JsonProperty("srclangs")]              public string srclangs { get; set; }
        //[JsonProperty("srclangs_confidences")]  public string srclangs_confidences { get; set; }
        //[JsonProperty("extended_srclangs")]     public string extended_srclangs { get; set; }
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