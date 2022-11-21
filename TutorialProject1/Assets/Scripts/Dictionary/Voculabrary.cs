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
            VIExit.GetComponent<Button>().onClick.AddListener(CloseDict); 
        
        dictionaryManager = FindObjectOfType<DictionaryManager>();
    }

    public void VoculabraryButton()
    {
        //вызываем окно словаря по нажатию кнопки
        if(VIPanel.activeSelf == false)
        {
            OpenDict();
        }
        else
            CloseDict();
    }

    private void OpenDict()
    {
        OnGameStartAndUpdate.CloseInterface();
        VIPanel.SetActive(true);
        gameObject.GetComponent<DictScroll>().FillScroll();
        MainVariables.inInterface = true;
        Time.timeScale = 0f;   
        OnGameStartAndUpdate.OnInterfaceClose += CloseDict;
    }   
    private void CloseDict()
    {
        gameObject.GetComponent<DictScroll>().ClearScroll();
        VIPanel.SetActive(false); 
        MainVariables.inInterface = false;
        Time.timeScale = 1f;   
        OnGameStartAndUpdate.OnInterfaceClose -= CloseDict;
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
        int count = 0;
        foreach (var item in dictionaryManager.words)
        {
            if (item.learned == false) count++;
        }
        HeroMainData.wordActualCount = count;
    }
}