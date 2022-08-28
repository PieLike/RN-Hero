using UnityEngine;
using System;
using System.Data;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(EnglishGameInput))]

public class EnglishGame : MonoBehaviour
{
    private GameObject EGOriginalWord, EGCheckmark, EGPanel, EGExit;
    //private TMP_InputField tmp_Text;
    private string previousInput = "";
    private string translate = "";//, originalWord;
    private GameObject englishGameObject;
    private bool closeByEndingNew = false;
    private EnglishGameInput EGinput;
    private bool typedWordIsRight = false;
    
    private void Start() 
    {
        //находим дочерние объекты
        EGPanel = transform.Find("EnglishGamePanel").gameObject;
        EGCheckmark = transform.Find("EnglishGamePanel/EGCheckmark").gameObject;
        EGOriginalWord = transform.Find("EnglishGamePanel/EGOriginalWord").gameObject;
        EGExit = transform.Find("EnglishGamePanel/EGExit").gameObject;
            EGExit.GetComponent<Button>().onClick.AddListener(CloseEnglishGame);  

        EGinput = gameObject.GetComponent<EnglishGameInput>();   
    }

    public void StartGame(GameObject gameObject, bool closeByEnding) //для объектов
    {
        closeByEndingNew = closeByEnding;

        string[] wordsByParent = ReturnByParent(gameObject.name, false);
        if (wordsByParent.Length == 0)  
            Debug.LogError("Невозможно найти слова по родителю в базе данных: " + gameObject.name);
        else
        { 
            //выбираем случайное слово из массива и отсылаем его в импут
            int i = UnityEngine.Random.Range(0, wordsByParent.Length);
            if (SettingActive(wordsByParent[i]))
                englishGameObject = gameObject; 
        }
        typedWordIsRight = false;            
    }
    public void StartGame(string objName) //для заклинаний
    {
        string[] wordsByParent = ReturnByParent(objName, false);
        if (wordsByParent.Length == 0)  
            Debug.LogError("Невозможно найти слова по родителю в базе данных: " + objName);
        else
        { 
            //выбираем случайное слово из массива и отсылаем его в импут
            int i = UnityEngine.Random.Range(0, wordsByParent.Length);
            if (SettingActive(wordsByParent[i]))
                englishGameObject = gameObject; 
        }
        typedWordIsRight = false; 
    }

    private string[] ReturnByParent(string parent, bool returnEng)
    {
        //ищем слова по слову родителю в БД
        string[] words;
        string generalDataBaseName = "vocabularyGeneral.bytes";
        DataTable generalVocabulary;

        if (returnEng)  
            //ищем английские слова у родителя  
            generalVocabulary = WorkWithDataBase.GetTable($"SELECT eng FROM words WHERE parent = '{parent.ToLower()}';", generalDataBaseName);              
        else
            generalVocabulary = WorkWithDataBase.GetTable($"SELECT rus FROM words WHERE parent = '{parent.ToLower()}';", generalDataBaseName);

        if (generalVocabulary.Rows.Count > 0)
        {
            //цикл для записи слов из datatable в массив string
            words = new string[generalVocabulary.Rows.Count];
            for(int i = 0; i < generalVocabulary.Rows.Count; i++)
                words[i] = generalVocabulary.Rows[i][0].ToString();
        }
        else
        {
            //иначе нулевой массив
            words = new string[0];
        }

        return words;
    }
    
    public bool SettingActive(string originalWord)
    {
        if (String.IsNullOrEmpty(originalWord))
        {
            Debug.LogError("Проблемы с выводом слова");
            return false;
        }
        else
        {
            translate = ReturnTranslate(originalWord, true);
            if (translate == "")
            {
                Debug.LogError("Невозможно найти слово в базе данных: " + originalWord);
                return false;
            }
            else
            //если есть оригинал слова и его перевод то запускаем интерфейс EG
            {
                EGPanel.SetActive(true);
                EGOriginalWord.GetComponent<TMP_Text>().text = originalWord;

                MainVariables.inInterface = true;

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
                if (EGinput.fullWord.ToLower() == translate.ToLower())
                {
                    typedWordIsRight = true;

                    EGCheckmark.GetComponent<ForCheckmark>().GoGreen();
                    //CloseEnglishGame(); 
                }
                previousInput = EGinput.fullWord;
            }
        }
    }    
    public static string ReturnTranslate(string originalWord, bool englishInput)
    {
        //возвращаем перевод слова по его иностранному аналогу (рус или енг)
        string generalDataBaseName = "vocabularyGeneral.bytes";
        DataTable generalVocabulary;

        if (englishInput)  
            //вводим англисйкое слово    
            generalVocabulary = WorkWithDataBase.GetTable($"SELECT eng FROM words WHERE rus = '{originalWord.ToLower()}';", generalDataBaseName);              
        else
            generalVocabulary = WorkWithDataBase.GetTable($"SELECT rus FROM words WHERE eng = '{originalWord.ToLower()}';", generalDataBaseName);

        if (generalVocabulary.Rows.Count > 0)
            return generalVocabulary.Rows[0][0].ToString();
        else
            return "";
    }

    public void CloseEnglishGame()
    {
        EGinput.Clear();    //очищаем массивы введенного слова
        EGPanel.SetActive(false);
        MainVariables.inInterface = false; 
        //если требуется удалить объект после EG - удаляем (вызываем CallDead() ) 
        if (closeByEndingNew && (englishGameObject != null) && (englishGameObject.tag == "Enemy") && typedWordIsRight == true)
            englishGameObject.GetComponent<EnemyBehavior>().CallDead();    
    }

    
}
