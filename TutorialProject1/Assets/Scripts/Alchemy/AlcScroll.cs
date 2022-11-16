using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Alchemy))]
public class AlcScroll : MonoBehaviour
{
    private GameObject scrollViewContent;
    private GameObject prefab; List<GameObject> prefabList;
    //private DictionaryManager dictionaryManager;
    private InterfaceManager interfaceManager;
    private Alchemy alchemy;

    private void Start() 
    {
        alchemy = gameObject.GetComponent<Alchemy>();
        //dictionaryManager = alchemy.dictionaryManager;  
        interfaceManager =  alchemy.interfaceManager;   
        if (interfaceManager == null)
        {
            interfaceManager = FindObjectOfType<InterfaceManager>();  
        }

        scrollViewContent = interfaceManager.AlcContent;
        prefab = Resources.Load<GameObject>("2d_prefabs/WordPanelForAlcScroll");      

        prefabList = new List<GameObject>();  

        //FillScroll();
    }

    public void FillScroll() 
    {
        foreach (var item in alchemy.allIngredients)
        {
            if (item != null && item.word != "")
                AddScrollElement(item);
        }
    }
    public void AddScrollElement(Word item)
    {
        if (item.translate == null || item.translate.Count == 0) return;
        GameObject newIngredient = Instantiate(prefab, scrollViewContent.transform);
        var text = newIngredient.transform.Find("Text").gameObject;
            text.GetComponent<TMP_Text>().text = item.translate[0];//item.word;
        WordData wordData = newIngredient.GetComponent<WordData>();  
            wordData.word = item;
            //SetIcon(wordData.icon, item.word);
        Button plus = newIngredient.transform.Find("Plus").gameObject.GetComponent<Button>();
        if (plus != null)
        {
            plus.onClick.AddListener(() => AddElementToPot(newIngredient, item)); 
        }
        prefabList.Add(newIngredient);
    }

    private void AddElementToPot(GameObject objWord, Word word)
    {
        alchemy.AddToProps(word);
        Destroy(objWord);
    }

    /*private void SetIcon(Sprite sprite, string word)
    {
        string url = "https://img.icons8.com/100/color/" + word;
        string url2 = "https://img.icons8.com/100/emoji/" + word;
        string url3 = "https://img.icons8.com/100/" + word;
        StartCoroutine(IEnumeratorIcon(sprite, url, url2));
    }
    IEnumerator IEnumeratorIcon(Sprite sprite, string url, string url2 = "", string url3 = "") 
    {
        UnityWebRequest reader = UnityWebRequestTexture.GetTexture(url);
        yield return reader.SendWebRequest();
        if (reader.result == UnityWebRequest.Result.Success)
        {
            Texture2D myTexture = ((DownloadHandlerTexture) reader.downloadHandler).texture;
            sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
        }
        else
        {
            if (url2 != "")
            {
                IEnumeratorIcon(sprite, url2, url3); 
            }
            else
                Debug.Log("Не удалось подгрузить иконку :" + url);
        }            
    }*/

    public void ClearScroll()   //выполнять на выходе
    {
        foreach (var item in prefabList)
        {
            Destroy(item);
        }
        prefabList.Clear();
    }
}
