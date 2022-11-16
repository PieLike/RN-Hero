using UnityEngine;
using System;
using System.Data;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(EnglishGameInput))]

public class EnglishGame : MonoBehaviour
{
    private GameObject EGOriginalWord, EGCheckmark, EGPanel, EGExit;
    //private TMP_InputField tmp_Text;
    private string previousInput = "";
    private List<string> translate;//, originalWord;
    private GameObject englishGameObject;
    private bool closeByEndingNew = false;
    private EnglishGameInput EGinput;
    private bool typedWordIsRight = false;
    private InterfaceManager interfaceManager;
    private DictionaryManager dictionaryManager;
    private PickerWheelEnglishType pickerWheel;
    
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
        dictionaryManager = FindObjectOfType<DictionaryManager>();

        EGPanel = interfaceManager.EnglishGamePanel;
        EGCheckmark = interfaceManager.EGCheckmark;
        EGOriginalWord = interfaceManager.EGOriginalWord;
        EGExit = interfaceManager.EGExit;
            EGExit.GetComponent<Button>().onClick.AddListener(CloseEnglishGame);  

        EGinput = gameObject.GetComponent<EnglishGameInput>(); 

        pickerWheel = interfaceManager.PickerWheelEnglshType.GetComponent<PickerWheelEnglishType>();

        /*Word randomWord = dictionaryManager.TakeRandom();
        if (randomWord != null)
            SettingActive(randomWord);*/
    }

    public void StartGame(GameObject gameObject, bool closeByEnding) //для объектов
    {
        closeByEndingNew = closeByEnding;
        englishGameObject = gameObject;

        if (MainVariables.modeWithoutEG == true)
        {
            typedWordIsRight = true;
            StartCoroutine(WaitASecond(0.1f));
            //CloseEnglishGame();
            return; 
        }        

        string enemyName;
        if (gameObject.name.Contains('('))
            enemyName = gameObject.name.Split('(')[0];
        else
            enemyName = gameObject.name;
        enemyName = enemyName.Replace(" ", "").ToLower();

        List<Word> wordsByParent = new List<Word>();
        wordsByParent = dictionaryManager.ReturnByParent(enemyName);
        if (wordsByParent.Count == 0)  
            Debug.LogError("Невозможно найти слова по родителю в базе данных: " + enemyName);
        else
        { 
            //выбираем случайное слово из массива и отсылаем его в импут
            int i = UnityEngine.Random.Range(0, wordsByParent.Count);
            SettingActive(wordsByParent[i]);
        }
        typedWordIsRight = false;            
    }
    public void StartGame(string objName) //для заклинаний
    {
        if (MainVariables.modeWithoutEG == true)
        {
            typedWordIsRight = true;
            StartCoroutine(WaitASecond(0.1f));
            //CloseEnglishGame();
            return; 
        }    

        List<Word> wordsByParent = new List<Word>();
        wordsByParent = dictionaryManager.ReturnByParent(objName);
        if (wordsByParent.Count == 0)  
            Debug.LogError("Невозможно найти слова по родителю в базе данных: " + objName);
        else
        { 
            //выбираем случайное слово из массива и отсылаем его в импут
            int i = UnityEngine.Random.Range(0, wordsByParent.Count);
            SettingActive(wordsByParent[i]);
        }
        typedWordIsRight = false; 
    }

    
    
    public bool SettingActive(Word originalWord)
    {
        string stringOriginalWord = originalWord.word;
        if (originalWord.filled == false)
            originalWord = dictionaryManager.FillWordData(originalWord);

        if (String.IsNullOrEmpty(stringOriginalWord))
        {
            Debug.LogError("Проблемы с выводом слова");
            return false;
        }
        else
        {
            translate = originalWord.translate;
            if (originalWord.translate == null || originalWord.translate.Count == 0 || originalWord.translate[0] == "")
            {
                Debug.LogError("Не нашлось перевода у слова: " + stringOriginalWord);
                return false;
            }
            else
            //если есть оригинал слова и его перевод то запускаем интерфейс EG
            {
                EGPanel.SetActive(true);
                EGOriginalWord.GetComponent<TMP_Text>().text = stringOriginalWord;

                pickerWheel.FillWheelPieces(stringOriginalWord);

                //MainVariables.inInterface = true;

                //при успешном вызове даём об этом знать
                return true;
            }
        }  
    }
    public void Update() 
    {
        if (EGPanel.activeSelf == true)
        {
            //сначала смотрим изменился ли умпут по вравнению с предыдущим (чтобы оптимизировать)
            //потом идет проверка - введен ли в импут перевод слова
            if(previousInput != EGinput.fullWord)
            {
                foreach (string tr in translate)
                {
                    if (EGinput.fullWord.ToLower() == tr.ToLower())
                    {
                        typedWordIsRight = true;

                        EGCheckmark.GetComponent<ForCheckmark>().GoGreen();
                        StartCoroutine(WaitASecond(1));
                        //CloseEnglishGame(); 
                        break;
                    }
                }                
                previousInput = EGinput.fullWord;
            }
        }
    } 

    public void CloseEnglishGame()
    {        
        EGinput.Clear();    //очищаем массивы введенного слова
        EGPanel.SetActive(false);
        //MainVariables.inInterface = false; 
        //если требуется удалить объект после EG - удаляем (вызываем CallDead() ) 
        if (closeByEndingNew && (englishGameObject != null) && (englishGameObject.tag == "Enemy") && typedWordIsRight == true)
        {
            Debug.Log("CloseEnglishGame calldead");
            //englishGameObject.GetComponent<EnemyBehavior>().CallDead(); 
            englishGameObject.GetComponent<EnemyBehavior>().isDead = true;   
        }
    }

    private IEnumerator WaitASecond(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds); 
        CloseEnglishGame();       
    }    
}
