using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DictScroll : MonoBehaviour
{
    private GameObject scrollViewContent;
    private GameObject prefab; List<GameObject> prefabList;
    private DictionaryManager dictionaryManager;
    private InterfaceManager interfaceManager;
    private float pointUpToLearn = 10f;

    private void Start() 
    {
        dictionaryManager = FindObjectOfType<DictionaryManager>();
        interfaceManager = FindObjectOfType<InterfaceManager>();

        scrollViewContent = interfaceManager.VIContent;
        prefab = Resources.Load<GameObject>("2d_prefabs/WordPanelForDictScroll");      

        prefabList = new List<GameObject>();  
    }

    public void FillScroll() 
    {
        if (dictionaryManager.words == null) return;
        foreach (var item in dictionaryManager.words)
        {
            if (item.learned) continue;
            
            GameObject newWord = Instantiate(prefab, scrollViewContent.transform);
            var name        = newWord.transform.Find("Name").gameObject;
                name.GetComponent<TMP_Text>().text = item.word;
            var translateText   = newWord.transform.Find("Translate").gameObject.GetComponent<TMP_Text>();
                if (item.translate != null) 
                {
                    foreach (var tr in item.translate)
                    {
                        if (translateText.text == "")
                            translateText.text = tr;
                        else    
                            translateText.text += ", " + tr; 
                    }
                }

            var wordCount     = newWord.transform.Find("WordCount").gameObject;
            var wordCountLine = wordCount.transform.Find("WordCountLine").gameObject;
                if (item.learnCount >= pointUpToLearn)
                {
                    wordCount.SetActive(false);
                    wordCountLine.SetActive(false);
                    newWord.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() => dictionaryManager.LearnWord(item)); 
                }
                else
                    wordCountLine.GetComponent<Image>().fillAmount = (item.learnCount % pointUpToLearn) / pointUpToLearn + 0.01f;
            Image icon = newWord.transform.Find("Icon").gameObject.GetComponent<Image>();
            if (item.icon != null)
                icon.sprite = item.icon;
            else 
            {
                FindIcon.DictStruct dictStruct = new FindIcon.DictStruct {  manager = dictionaryManager, wordElement = item};
                FindIcon.SetIcon(item.word, dictStruct, icon);
            }
            


            prefabList.Add(newWord);
        }
    }
    /*private void LearnWord(Word word)
    {
        dictionaryManager.LearnWord(word);
    }*/    

    public void ClearScroll()
    {
        foreach (var item in prefabList)
        {
            Destroy(item);
        }
        prefabList.Clear();
    }
}