using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemies;
    public List<SpawnedEnemy> spawnedEnemies;
    [Range(0.1f, 10f)] public float spawnPause;

    public struct SpawnedEnemy
    {
        public GameObject enemyObj;
        public EnemyBehavior enemyBehavior;
        public EnemyAI enemyAI;
    }

    private void Awake() 
    {        
        if (spawnedEnemies == null) spawnedEnemies = new List<SpawnedEnemy>();

        GetComponent<SpriteRenderer>().enabled = false;
    }
    public void ReDoSpawn()
    {
        if (enemies != null && enemies.Count > 0)
        {
            StartCoroutine(SpawnCoroutine());        
        }
    }

    public IEnumerator SpawnCoroutine()
    {
        Spawn(enemies[0]);        
        for (var i = 1; i < enemies.Count; i++)
        {
            yield return new WaitForSeconds(spawnPause);
            Spawn(enemies[i]);   
        } 
    }

    public void Spawn(GameObject enemy)
    {
        SpawnedEnemy spawnedEnemy = new SpawnedEnemy();
        spawnedEnemy.enemyObj = Instantiate(enemy, transform.position, Quaternion.identity, transform);

        spawnedEnemy.enemyBehavior = spawnedEnemy.enemyObj.GetComponent<EnemyBehavior>();
        spawnedEnemy.enemyAI = spawnedEnemy.enemyObj.GetComponent<EnemyAI>();

        if (spawnedEnemies == null) spawnedEnemies = new List<SpawnedEnemy>();
        spawnedEnemies.Add(spawnedEnemy);

        //enemies.Remove(enemy);

        /*foreach (string item in enemy.scripts)
        {
            var type = System.Type.GetType(item);// componentT
            newEnemy.AddComponent(type);// as EnemyBehaviour;//MonoBehaviour;    //System.Type.GetType(item)   //EnemyBehaviour
        }*/
    }

    public void DestroySpawnedEnemies()
    {        
        if (spawnedEnemies != null)
        {
            foreach (SpawnedEnemy spawnedEnemy in spawnedEnemies)
            {
                if (spawnedEnemy.enemyObj != null)
                    Destroy(spawnedEnemy.enemyObj);
            }
            spawnedEnemies.Clear();
        }
    }

    public void StopSpawnEnemies()
    {
        if (spawnedEnemies != null)
        {
            foreach (SpawnedEnemy spawnedEnemy in spawnedEnemies)
            {
                if (spawnedEnemy.enemyObj != null)
                {
                    spawnedEnemy.enemyAI.allowToMove = false;
                }
            }
        }
    }
    public void StartSpawnEnemies()
    {
        if (spawnedEnemies != null)
        {
            foreach (SpawnedEnemy spawnedEnemy in spawnedEnemies)
            {
                if (spawnedEnemy.enemyObj != null)
                    spawnedEnemy.enemyAI.allowToMove = true;
            }
        }
    }
}
