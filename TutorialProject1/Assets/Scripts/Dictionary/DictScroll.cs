using System;
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
    private float pointUpToLearn = 5f;  //10

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
                            translateText.text = "(" + PosToRus(item.pos) + ") " + tr;
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
                    newWord.transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() => LearnWord(newWord, item)); 
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

    private string PosToRus(Word.Pos pos)
    {
        switch (pos)
        {
            case Word.Pos.Noun: return "??????.";
            case Word.Pos.Verb: return "????.";
            case Word.Pos.Adjective: return "????????.";
            case Word.Pos.Pronoun: return "??????????????.";
            case Word.Pos.Conjunction: return "????????";
            case Word.Pos.Adverb: return "??????????.";
            case Word.Pos.Article: return "??????????????";
            case Word.Pos.Preposition: return "??????????????";
            case Word.Pos.Numeral: return "????????.";
            default: return "";
        }        
    }

    private void LearnWord(GameObject obj, Word word)
    {
        dictionaryManager.LearnWord(word);
        prefabList.Remove(obj);
        Destroy(obj);
    }    

    public void ClearScroll()
    {
        foreach (var item in prefabList)
        {
            Destroy(item);
        }
        prefabList.Clear();
    }
}
