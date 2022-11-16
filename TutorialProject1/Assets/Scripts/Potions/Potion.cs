using UnityEngine;

[CreateAssetMenu(menuName = "Potion", fileName = "potion_")]

public class Potion : ScriptableObject
{
    [SerializeField] public string potionName;
    [SerializeField] public Sprite image;
    [SerializeField] public string script;
    //[SerializeField] int level;
    [SerializeField] public string info;
    [SerializeField] public StatusObject status;
    [SerializeField] public float duration;
}
