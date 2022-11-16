using System;
using UnityEngine;
using System.IO;
using System.Net;
public class APIWordnik
{
    public static void GetRequest(string word)
    {
        string ApiKey = "xbqzmyi4pzgvhmmgqy9dy8ij3bhgcttk5hi6q88mu64ml2mhj";
        
        //var url = $"https://api.wordnik.com/v5/words.json/reverseDictionary";
        //var parameters = $"?api_key={ApiKey}&query={word}&limit=10";
        
        //HttpClient client = new HttpClient();        
        

        /*FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);
        Debug.Log("IsSuccessStatusCode = " + response.IsSuccessStatusCode.ToString() + ", " + response.ToString()); */

        string url = "https://api.wordnik.com/v3/words.json/reverseDictionary?query=fruit&limit=10&api_key=" + ApiKey;
        //url + parameters;
        Debug.Log(url);
        WebRequest request = WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";

        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseFromWordnik = reader.ReadToEnd();
                Console.WriteLine(responseFromWordnik);
            }
        }      
        
    }
}
