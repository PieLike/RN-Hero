using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChecker : MonoBehaviour
{
    public List<Spawn> spawnPoints;
    float duration = 5;
    public class Spawn
    {
        public Transform gameObj;
        public SpawnPoint spawnPoint;
    }

    private void Awake() 
    {
        spawnPoints = new List<Spawn>();
        foreach (Transform child in transform)
        {
            if (child.tag == "SpawnPoint")
            {
                SpawnPoint _spawnPoint = child.GetComponent<SpawnPoint>();
                if (_spawnPoint != null)
                {
                    Spawn spawn = new Spawn{gameObj = child, spawnPoint = _spawnPoint};
                    spawnPoints.Add(spawn);
                }
            }  
        }
    }

    private void Start() 
    {
        QuestEvents.OnEnemyKilled += WhenEnemyKilled; 
    }

    private void OnEnable() 
    {
        ReDoSpawnAll();
    }
    private void OnDisable() 
    {
        if (spawnPoints != null)
        {
            foreach (Spawn spawn in spawnPoints)
            {
                spawn.spawnPoint.DestroySpawnedEnemies();
            }
        }
    }
    private void ReDoSpawnAll()
    {
        if (spawnPoints != null)
        {
            foreach (Spawn spawn in spawnPoints)
            {
                spawn.spawnPoint.ReDoSpawn();
            }
        }
    }

    public void StopAllSpawnEnemies()
    {
        if (spawnPoints != null)
        {
            foreach (Spawn spawn in spawnPoints)
            {
                spawn.spawnPoint.StopSpawnEnemies();
            }


            WavesCounter.Clear();
            WavesCounter.Hide();
        }
    }
    public void StartAllSpawnEnemies()
    {
        if (spawnPoints != null)
        {
            foreach (Spawn spawn in spawnPoints)
            {
                spawn.spawnPoint.StartSpawnEnemies();
            }

            if (spawnPoints.Count > 0)
            {
                WavesCounter.Show();
            }
        }        
    }

    public void WhenEnemyKilled(QuestEvents.KilledEnemy killedEnemy)
    {
        if (spawnPoints != null && killedEnemy.spawnPoint.parent == transform)  //spawnPoints = spawnPoints //то есть если враг убили в этом спайнчекере
        {
            foreach (Spawn spawn in spawnPoints)
            {
                if (spawn.spawnPoint.spawnedEnemies != null)
                {
                    if (spawn.spawnPoint.spawnedEnemies.Count > 0)
                    {
                        foreach (SpawnPoint.SpawnedEnemy spawnedEnemy in spawn.spawnPoint.spawnedEnemies)
                        {
                            if (spawnedEnemy.enemyObj != null && spawnedEnemy.enemyBehavior.isDead == false) 
                            {
                                return;
                            }
                        }                        
                    }
                }
            }
            WavesCounter.AddCount();
            StartCoroutine(NewSpawnCoroutine());
        }
    }

    private IEnumerator NewSpawnCoroutine()
    {
        yield return new WaitForSeconds(duration);

        ReDoSpawnAll();
        StartAllSpawnEnemies();
    }
}
