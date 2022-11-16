using UnityEngine;
using System;
//using Unity.VectorGraphics;

[CreateAssetMenu(menuName = "Item", fileName = "item_")]
public class Item : ScriptableObject
{
    //public enum ItemType { armor, weapon, potion, other } 
    //[SerializeField] public string itemName;
    [SerializeField] public ItemData.ItemType itemType;
    [SerializeField] public Sprite image;   [SerializeField] public Sprite sprite;
    [SerializeField] [Range(0f, 50f)] public float damage;
    [SerializeField] [Range(0.1f, 10f)] public float radiusAttack;
    [SerializeField] [Range(0.1f, 10f)] public float speedAttack;
}
