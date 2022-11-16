using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Voculabrary : MonoBehaviour
{
    private GameObject VIPanel, VIExit;
    static DictionaryManager dictionaryManager;
    InterfaceManager interfaceManager;

    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();

        //находим дочерние объекты        
        VIPanel = interfaceManager.VoculabraryInterfacePanel;         
        VIExit = interfaceManager.VIExit;
            VIExit.GetComponent<Button>().onClick.AddListener(Close); 
        
        dictionaryManager = FindObjectOfType<DictionaryManager>();
    }

    public void VoculabraryButton()
    {
        //вызываем окно словаря по нажатию кнопки
        if(VIPanel.activeSelf == false)
        {
            VIPanel.SetActive(true);
            gameObject.GetComponent<DictScroll>().FillScroll();
        }
        else
        {
            gameObject.GetComponent<DictScroll>().ClearScroll();
            VIPanel.SetActive(false);
        }
    }

    public void Close()
    {
        gameObject.GetComponent<DictScroll>().ClearScroll();
        VIPanel.SetActive(false);    
    }   

    public static bool AddWordInDataBase(Loot word)
    {    
        //Debug.Log("AddWordInDataBase");
        if (dictionaryManager.AddWord(word, true))
        {
            ReCountWordActualCount();
            return true;
        }    
        else return false;     
    }    

    public static void ReCountWordActualCount()
    {
        HeroMainData.wordActualCount = dictionaryManager.words.Count;
    }
}