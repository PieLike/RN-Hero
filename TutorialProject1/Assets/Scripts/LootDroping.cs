using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;
//using System.Globalization;

public class LootDroping : MonoBehaviour
{    
    //public GameObject[] items;
    private string[] words = new string[20];
    public GameObject itemType;
    public float force = 5;
    public float dropSpeed = 0.2f;
 
    public void Drop()
    {
        FillWords();
        StartCoroutine(DropCoroutine());           
    }

    private void FillWords()
    {
        //заполняем массив слов словами из общей базы данных по parent = имея объекта
        string wordsDataBaseName = "vocabularyGeneral.bytes";
        string query = ($"SELECT eng FROM words WHERE parent = '{gameObject.name.ToLower()}';");
        DataTable wordsDataTable = WorkWithDataBase.GetTable(query, wordsDataBaseName);   

        for(int row = 0; row < wordsDataTable.Rows.Count; row++)
        {
            words[row] = wordsDataTable.Rows[row][0].ToString();    
        }
    }

    private void Bounce(GameObject item)
    {
        //пружиним объект
        float dirx, dirz;
        dirx = Random.Range(-1,1);
        dirz = Random.Range(-1,1);

        item.GetComponent<Rigidbody>().AddForce(new Vector3(dirx*force/10,force,dirz*force/10),ForceMode.Impulse);
    }

    private GameObject ToNameObject(GameObject item, string word)
    {
        item.GetComponent<TMP_Text>().text = word;
        return item;
    }

    private IEnumerator DropCoroutine()
    {
        Vector3 placeDrop;
        GameObject newItem;

        //если элемент массива заполнен то создаем объект и пружиним его
        foreach (string word in words)
        {
            if (word != null)
            {
                yield return new WaitForSeconds(dropSpeed);
                placeDrop = transform.position;
                newItem = Instantiate(itemType, placeDrop, Quaternion.Euler(0,0,0));
                newItem = ToNameObject(newItem, word);
                Bounce(newItem);                
            }
        }
    }
}

