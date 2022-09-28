using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;

public class LootDroping : MonoBehaviour
{    
    private string[] words = new string[20];
    private GameObject itemType;
    public float force = 10f;
    public float dropSpeed = 0.2f;
    public string parent = "";
 
    private void Start()
    {
        //находим объекты в resourses
        itemType = Resources.Load<GameObject>("3d_prefabs/WordDrop");           
    }

    public void Drop()
    {
        if (parent == "")
        {
            Debug.LogError("Не заполнено поле parent у объекта, выбрасывающего лут");
            return;
        }

        if (gameObject.tag == "Enemy")
            FillWords(gameObject.name);
        else if (gameObject.tag == "Chest")
            FillWords(parent);

        StartCoroutine(DropCoroutine());           
    }

    private void FillWords(string parent)
    {
        //заполняем массив слов словами из общей базы данных по parent = имея объекта
        string wordsDataBaseName = "vocabularyGeneral.bytes";
        string query = ($"SELECT eng FROM words WHERE parent = '{parent.ToLower()}';");
        DataTable wordsDataTable = WorkWithDataBase.GetTable(query, wordsDataBaseName);   

        for(int row = 0; row < wordsDataTable.Rows.Count; row++)
        {
            words[row] = wordsDataTable.Rows[row][0].ToString();    
        }
    }

    private void Bounce(GameObject item)
    {
        //пружиним объект
        float dirx, diry;
        dirx = Random.Range(-5f,5f);
        diry = Random.Range(3f,6f);

        item.GetComponent<Rigidbody2D>().AddForce(new Vector2(dirx*force/50,diry*force/10),ForceMode2D.Impulse); //dirx*force/10,force,dirz*force/10
    }

    private GameObject ToNameObject(GameObject item, string word)
    {  
        item.GetComponent<TMP_Text>().text = word;
        return item;
    }

    private IEnumerator DropCoroutine()
    {
        Vector2 placeDrop;
        GameObject newItem;

        //если элемент массива заполнен то создаем объект и пружиним его
        foreach (string word in words)
        {
            if (word != null)
            {
                yield return new WaitForSeconds(dropSpeed);
                placeDrop = transform.position;
                newItem = Instantiate(itemType, placeDrop, Quaternion.Euler(0f,0f,0f));
                newItem = ToNameObject(newItem, word);

                LootBehavior lootBehavior = newItem.GetComponent<LootBehavior>();
                if (lootBehavior) 
                {
                    lootBehavior.parentpositionY = gameObject.transform.position.y;
                    lootBehavior.parentlocalScaleY = gameObject.transform.localScale.y;
                }

                Bounce(newItem);                
            }
        }
        DoAfterLootIsDroped();
    }
    private void DoAfterLootIsDroped()
    {
        if (gameObject.tag == "Enemy")
        {
            EnemyBehavior enemyBehavior = gameObject.GetComponent<EnemyBehavior>();
            if (enemyBehavior) enemyBehavior.lootDropIsDone = true;
        }
    }
}

