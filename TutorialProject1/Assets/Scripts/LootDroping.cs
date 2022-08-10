using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootDroping : MonoBehaviour
{    
    //public GameObject[] items;
    public string[] words;
    public GameObject itemType;
    public float force = 5;
    public float dropSpeed = 0.2f;

    public void Drop()
    {
        

            StartCoroutine(DropCoroutine());

            /*placeDrop = transform.position;
            newItem = Instantiate(itemType, placeDrop, Quaternion.Euler(0,0,0));
            newItem = ToNameObject(newItem, word);
            Bounce(newItem);*/
            
    }

    private void Bounce(GameObject item)
    {
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

        foreach (string word in words){
            yield return new WaitForSeconds(dropSpeed);
            placeDrop = transform.position;
            newItem = Instantiate(itemType, placeDrop, Quaternion.Euler(0,0,0));
            newItem = ToNameObject(newItem, word);
            Bounce(newItem);
        }
    }
}

