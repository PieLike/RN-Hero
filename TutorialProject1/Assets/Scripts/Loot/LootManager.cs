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
        EnemyLoot enemyLoot = enemyLoots.Find( delegate(EnemyLoot enemyLoot){ return enemyLoot.enemy == _enemy; } );

        if (enemyLoot == null)
        {
            enemyLoot = new EnemyLoot{ enemy = _enemy };
        }
        else
        {
            enemyLoot.loots.Clear();
        }
        enemyLoot.loots = loots;
    }
}
