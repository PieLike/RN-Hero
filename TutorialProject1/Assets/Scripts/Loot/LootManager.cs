using System;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;
    public List<EnemyLoot> enemyLoots = new List<EnemyLoot>();

    [Serializable]
    public class EnemyLoot
    {
        public Enemy enemy;
        public List<Loot> loots;
    }

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    
    public List<Loot> FindEnemyLoots(Enemy _enemy)
    {
        EnemyLoot enemyLoot = enemyLoots.Find( delegate(EnemyLoot enemyLoot){ return enemyLoot.enemy == _enemy; } );

        if (enemyLoot != null && enemyLoot.loots != null && enemyLoot.loots.Count > 0)
        {
            List<Loot> result = enemyLoot.loots;
            return result;
        }
        else
            return null;
    }

    public void WriteEnemyLoot(Enemy _enemy, List<Loot> loots)
    {
        int numberEnemyLoot = enemyLoots.FindIndex( delegate(EnemyLoot enemyLoot){ return enemyLoot.enemy.fullName == _enemy.fullName; } );        

        if (numberEnemyLoot == (-1))
        {
            EnemyLoot enemyLoot = new EnemyLoot{ enemy = _enemy };
            enemyLoot.loots = loots;
            enemyLoots.Add(enemyLoot);
        }
        else
        {
            enemyLoots[numberEnemyLoot].loots.Clear();
            enemyLoots[numberEnemyLoot].loots = loots;
        }
    }

    private void Update() 
    {
        //Debug.Log("wordAbleCount = " + HeroMainData.wordAbleCount.ToString() + "wordActualCount = " + HeroMainData.wordActualCount.ToString()); 
    }
}
