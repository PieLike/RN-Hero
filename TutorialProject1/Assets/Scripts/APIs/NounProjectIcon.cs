using System;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Http;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Security.Cryptography;

public class NounProjectIcon
{
    public static  void GetRequest(string word)//async
    {
        //using (var client = new HttpClient())
        //{
        /*var client = new HttpClient(); 
        var values = new Dictionary<string, string>
        {
            { "term", word },
            { "oauth_consumer_key", "167727f05e214c009a8c74e81d327b6a" },
            { "oauth_consumer_secret", "24ebf4126a77499c838eb81b22fc0ca4" }
        };           
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync($"http://api.thenounproject.com/icon/", content);
        //var responseString = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(response.Headers);
        Debug.Log("IsSuccessStatusCode = " + response.IsSuccessStatusCode.ToString() + ", " + response.ToString());*/        

        var url = $"http://api.thenounproject.com/icon/{word}/";
        //var parameters = $"?ml={word}&md=p";       
        
        //url = url + parameters;
        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";

        var username   = "167727f05e214c009a8c74e81d327b6a";
        var password   = "97ad43df964c4ce4a9843725a21a11b5";
        //string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
        //request.Headers.Add("Authorization", "Basic " + encoded);

        
        //auth = OAuth("your-api-key", "your-api-secret")
        //tokenManager = new InMemoryTokenManager(consumerKey, consumerSecret);

        var authHeader = $"oauth_consumer_key={username}, oauth_signature={password}";    //oauth_consumer_secret
        request.Headers.Add("Authorization", "OAuth " + authHeader);
    //Authorization:OAuth oauth_consumer_key="XXXXXXXX-XXXX-XXXX-XXXXXXXXXXXX1EFA",
                 ///oauth_signature_method="HMAC-SHA1",
                 ///oauth_timestamp="******6814",
                 ///oauth_nonce="***9gi",
                 ///oauth_version="1.0",
                 ///oauth_signature="****************************Ebo%3D"

    //Authorization:OAuth oauth_consumer_key="XXXXXXXX-XXXX-XXXX-XXXXXXXXXXXX1EFA",
                //oauth_token="XXXXXXXX-XXXX-XXXX-XXXXXXXXXXXXC303",
                //oauth_signature_method="HMAC-SHA1",
                //oauth_timestamp="******6814",
                //oauth_nonce="***9gi",
                //oauth_version="1.0",
                //oauth_signature="****************************p5o%3D"

        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        request.UserAgent = "fruits";

        request.PreAuthenticate = true;
        request.UseDefaultCredentials = true;
        request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                string responseDatamuse = reader.ReadToEnd();

                Debug.Log(responseDatamuse.ToString());
                
                //results = JsonConvert.DeserializeObject<List<DatamuseResult>>(responseDatamuse);
            }
        }
    }
            
        
}
