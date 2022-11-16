using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class FindIcon
{
    public class MyStaticMB : MonoBehaviour { }
    private static MyStaticMB myStaticMB;

    public class DictStruct
    {
        public DictionaryManager manager;
        public Word wordElement;
    }

    public static void SetIcon(string word, DictStruct dictElement = null, Image compImage = null)
    {
        string url = "https://img.icons8.com/100/color/" + word;
        string url2 = "https://img.icons8.com/100/emoji/" + word;
        string url3 = "https://img.icons8.com/100/" + word;


        Init();
        myStaticMB.StartCoroutine(IEnumeratorIcon(word, url, url2, url3, dictElement, compImage));
    }
    private static IEnumerator IEnumeratorIcon(string word, string url, string url2 = "", string url3 = "", DictStruct dictElement = null, Image compImage = null) 
    {
        UnityWebRequest reader = UnityWebRequestTexture.GetTexture(url);
        yield return reader.SendWebRequest();
        //Debug.Log(reader.result.ToString());
        //Debug.Log(reader.ToString());
        if (reader.result == UnityWebRequest.Result.Success)
        {
            Texture2D myTexture = ((DownloadHandlerTexture) reader.downloadHandler).texture;
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
            if (compImage != null)
                compImage.sprite = newSprite;
            if (dictElement != null && dictElement.manager != null && dictElement.wordElement != null)
            {
                dictElement.manager.SetIcon(dictElement.wordElement, newSprite);
            }

        }
        else
        {
            if (url2 != "")
            {
                //Init();
                myStaticMB.StartCoroutine(IEnumeratorIcon(word, url2, url3, "", dictElement, compImage)); 
            }
            else
            {
                Debug.Log("Не удалось подгрузить иконку :" + url);
                //NounProjectIcon.GetRequest(word);
            }
        }            
    }

    private static void Init()
    {
        //If the instance not exit the first time we call the static class
        if (myStaticMB == null)
        {
            //Create an empty object called MyStatic
            GameObject gameObject = new GameObject("MyStatic");

            //Add this script to the object
            myStaticMB = gameObject.AddComponent<MyStaticMB>();
        }
    }
}
