using System;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;
    public Loot[] loots;

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
}
