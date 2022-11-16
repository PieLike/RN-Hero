using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Artifact", fileName = "artifact")]

public class Artifact : ScriptableObject
{
    //[NonSerialized] public bool enabled;
    public enum Functions {canMeleeAttack};
    public enum Stats {damage, speed, maxHP, attackSpeed};
    [SerializeField] public string artName;
    [SerializeField] public Sprite image;
    [SerializeField] public string info;
    [SerializeField] public List<Functions> functionList;
    [SerializeField] public List<StatBuff> statsList;

    [System.Serializable]
    public class StatBuff
    {
        public Stats stat;
        public float count;
    }
}
