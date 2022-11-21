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
        Shuffle(alchemy.allIngredients);
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

    //private static Random rng = new Random();
    public static void Shuffle<T>(IList<T> list)  
    {  
        System.Random rnd = new System.Random();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rnd.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public void ClearScroll()   //выполнять на выходе
    {
        foreach (var item in prefabList)
        {
            Destroy(item);
        }
        prefabList.Clear();
    }
}
