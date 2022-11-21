using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDroping : MonoBehaviour
{    
    public List<Loot> loots;
    private GameObject itemType;
    public float force = 10f;
    public float dropSpeed = 0.2f;
    [HideInInspector] public EnemyData enemyData;
 
    private void Start()
    {
        itemType = Resources.Load<GameObject>("3d_prefabs/WordDrop");         
    }

    public void Drop()
    {
        if (enemyData != null)
            loots = enemyData.FillLoots();

        if (loots != null && loots.Count > 0)
        {
            StartCoroutine(DropCoroutine());   
        }    
        else
            DoAfterLootIsDroped();    
    }
    
    private void Bounce(GameObject item)
    {
        //пружиним объект
        float dirx, diry;
        dirx = UnityEngine.Random.Range(-5f,5f);
        diry = UnityEngine.Random.Range(3f,6f);

        item.GetComponent<Rigidbody2D>().AddForce(new Vector2(dirx*force/50,diry*force/10),ForceMode2D.Impulse); //dirx*force/10,force,dirz*force/10
    }    

    private IEnumerator DropCoroutine()
    {
        Vector2 placeDrop;
        GameObject newItem;

        //если элемент массива заполнен то создаем объект и пружиним его
        if (loots != null)
        {
            CutLoots();

            foreach (Loot loot in loots)
            {
                if (loot != null && (((HeroMainData.wordAbleCount - HeroMainData.wordActualCount) > 0) || (loot.word.learnCount > 0)) )
                {
                    yield return new WaitForSeconds(dropSpeed);
                    placeDrop = transform.position;
                    newItem = Instantiate(itemType, placeDrop, Quaternion.Euler(0f,0f,0f), transform.parent);

                    LootBehavior lootBehavior = newItem.GetComponent<LootBehavior>();
                    if (lootBehavior) 
                    {
                        lootBehavior.parentpositionY = transform.position.y;
                        lootBehavior.parentlocalScaleY = transform.localScale.y;
                        lootBehavior.lootData = loot;//new Loot{ word = new Word { word = word.word, pos = word.pos} };
                    }

                    Bounce(newItem);                
                }
            }
        }
        DoAfterLootIsDroped();
    }
    private void DoAfterLootIsDroped()
    {
        if (gameObject.tag == "Enemy")
        {
            EnemyBehavior enemyBehavior = gameObject.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null) enemyBehavior.lootDropIsDone = true;
        }
    }

    private void CutLoots()
    {
        int wordsCanLearn = HeroMainData.wordAbleCount - HeroMainData.wordActualCount;

        if (wordsCanLearn > 0 && loots.Count > wordsCanLearn)
        {
            //for (var i = wordsCanLearn; i < loots.Count; i++)
            //int i = wordsCanLearn;
            int i = 0;
            while (loots.Count > wordsCanLearn && i < loots.Count)
            {
                if (loots[i].word.learnCount > 0)
                {
                    wordsCanLearn ++;
                }
                else
                {
                    loots.Remove(loots[i]); //loots[loots.Count-1]
                }
                i++;
            }
        }
    }
}

