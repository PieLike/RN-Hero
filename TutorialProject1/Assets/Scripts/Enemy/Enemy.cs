using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
//[CreateAssetMenu(menuName = "Enemy", fileName = "enemy_")]
public class Enemy// : ScriptableObject
{
    [SerializeField] public string fullName;
    [SerializeField] public Sprite sprite;
    [SerializeField] [Range(0f, 100f)] public float damage; 
    [SerializeField] [Range(0f, 10f)] public float damageImpact; 
    [SerializeField] [Range(0f, 1000f)] public float speed; 
    [SerializeField] [Range(0.1f, 10f)] public float attackSpeed; 
    [SerializeField] [Range(1f, 100f)] public float hp; 
    [SerializeField] [Range(0f, 100f)] public float sp;
    [SerializeField] public MainVariables.atackType vulnerability = MainVariables.atackType.none;    //уязвимость
    //[SerializeField] public List<Loot> lootWords;
    [SerializeField] public int enemyWordsCount = 10;
    [SerializeField] public string[] startWords;
    [SerializeField] public bool isAgressive;
    //[SerializeField] public string walkingSound;
    [SerializeField] public Sound[] sounds;
    //[SerializeField] public string[] scripts;

}
