using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;

public class LootDroping : MonoBehaviour
{    
    private string[] words = new string[20];
    private GameObject itemType;
    public float force = 500f;
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
        float dirx, dirz;
        dirx = Random.Range(-5,5);
        dirz = Random.Range(-3,3);

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

