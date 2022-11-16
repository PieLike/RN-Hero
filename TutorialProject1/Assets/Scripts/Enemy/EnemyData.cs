using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [HideInInspector] public float currentHP, currentSP; 
    public bool haveShield, isRotten; public bool inMovement; public bool isAttacking;
    public Enemy data;
    private LootManager lootManager;    DictionaryManager dictionaryManager;    LootDroping lootDroping;
    

    private void Start() 
    {
        lootManager = FindObjectOfType<LootManager>();
        dictionaryManager = FindObjectOfType<DictionaryManager>();
        lootDroping = GetComponent<LootDroping>();

        currentSP = data.sp;
        currentHP = data.hp; 

        if (currentSP > 0)
            haveShield = true;

        if (lootDroping != null)
            lootDroping.loots = FillLoots(); //FillLoots(lootDroping.loots);
    }

    public List<Loot> FillLoots()   //FillLoots(List<Loot> loots)
    {
        List<Loot> lootsFromManager = FillLootFromManager();  //FillLootFromManager(loots)
        List<Loot> lootsForDrop = lootsFromManager;

        if (lootsForDrop.Count < data.enemyWordsCount)  //мы загрузили текущий список лута из менеджера, но он там может быть пустым и еще не заполненным, в этом случае переходим к заполнение
                                                        //этап первый: заполнение из стартовых слов врага                                                        
        {
            foreach (string stringWord in data.startWords)
            {
                Word word = new Word{ word = stringWord };
                if (CheckIfWordAlreadyInLoot(word, lootsForDrop) == false && CheckIfWordAreadlyLearned(word) == false)
                {
                    Loot newLoot = new Loot();
                    newLoot.word = word;
                    //FillWordData(newWord);
                    lootsForDrop.Add(newLoot);
                }

                if (lootsForDrop.Count >= data.enemyWordsCount)
                    break;
            }

            if (lootsForDrop.Count < data.enemyWordsCount)  //этап второй: заполнение из дефолтных слов по полулярности
            {
                List<Word> wordsFromDataBase = DictDataBaseRequests.GetWordsByLevel();

                foreach (var wordFromDB in wordsFromDataBase)
                {
                    if (CheckIfWordAlreadyInLoot(wordFromDB, lootsForDrop) == false && CheckIfWordAreadlyLearned(wordFromDB) == false)
                    {
                        Loot newLoot = new Loot();
                        newLoot.word = wordFromDB;
                        //FillWordData(newWord);
                        lootsForDrop.Add(newLoot);
                    }

                if (lootsForDrop.Count >= data.enemyWordsCount)
                    break;
                }
            }

            if (lootsForDrop != lootsFromManager)   
            {
                //обновляем нвоый список дропа в менеджере
                lootManager.WriteEnemyLoot(data, lootsForDrop);
            }
        }
        return lootsForDrop;
    }
    private bool CheckIfWordAlreadyInLoot(Word word, List<Loot> loots)
    {
        foreach (Loot loot in loots)
        {
            if (loot.word.word == word.word)
                return true;
        }
        return false;
    }   
    private bool CheckIfWordAreadlyLearned(Word word)
    {
        if (dictionaryManager.CheckExisting(word) != (-1))
            return true;
        else
            return false;
    }

    public List<Loot> FillLootFromManager() //FillLootFromManager(List<Loot> loots)
    {
        /*if (data != null && lootManager != null)
        {
            List<Loot> lootsFromManager = lootManager.FindEnemyLoots(data);
            if (lootsFromManager != null)
            {
                loots = lootsFromManager;
            }
            else
                loots = new List<Loot>();
        }
        return loots;*/

        if (data != null && lootManager != null)
        {
            List<Loot> lootsFromManager = lootManager.FindEnemyLoots(data);
            if (lootsFromManager != null)
            {
                return lootsFromManager;
            }
            else
                return new List<Loot>();
        }
        else
            return new List<Loot>();
    }

    /*public bool AddLoot(Loot loot)
    {
        if (data != null)
        {
            data.lootWords.Add(loot);
            return true;
        }
        else
        {
            Debug.Log("EnemyData = null");
            return false;
        }
    }
    public bool DeleteLoot(Loot loot)
    {
        if (data != null)
        {
            Loot foundedLoot = CheckExisting(loot);
            if (foundedLoot!= null)
            {
                data.lootWords.Remove(foundedLoot);

                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.Log("EnemyData = null");
            return false;
        }
    }*/
    /*public Loot CheckExisting(Loot newLoot)
    {
        Loot result = data.lootWords.Find( delegate(Loot loot){ return loot.word == newLoot.word; } );
        return result;
    }*/
}
