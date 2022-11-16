using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeUp : MonoBehaviour
{
    private InterfaceManager interfaceManager;
    private void Start()
    {
        interfaceManager = FindObjectOfType<InterfaceManager>();
    }
    
    public void TakeUpWord(GameObject objectWord)   //GameObject takingWordCapsuleChild
    { 
        try
        {
            //Transform objectWord = takingWordCapsuleChild.transform.parent;
            if (objectWord != null)
            {
                //Debug.Log(objectWord.GetComponent<LootBehavior>().ToString());
                Loot loot = objectWord.GetComponent<LootBehavior>().lootData;
                //Debug.Log("Loot loot = objectWord.GetComponent<LootBehavior>().lootData");
                if (loot == null)
                {
                    Debug.Log("попытка поднять пустой объект слово");
                    return;
                }        
                //сохранить предмет в активный словарь и уничтожить
                if (Voculabrary.AddWordInDataBase(loot) == true)
                    interfaceManager.messageScript.NewMessage("Вы изучили новое слово!", loot.word.word, MessageScript.Scale.TextBig);
                    
                Destroy(objectWord);  
            }
            else
                Debug.LogError("objectWord = null");
        }
        catch (System.Exception e)
        {
            
            Debug.LogError("TakeUpWord, " + e);
        }
        
    } 
}
