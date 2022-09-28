using UnityEngine;

[CreateAssetMenu(menuName = "Spell", fileName = "spell_")]

public class ScriptableObjSpell : ScriptableObject
{
    [SerializeField] public string spellname;
    [SerializeField] public string fullname;
    [SerializeField] public SpellType type;
    [SerializeField] [Range(0, 50)] public int speed;
    [SerializeField] [Range(0.0f, 32.0f)] public float distance;    
    [SerializeField] public float damage;
    [SerializeField] public int bullets;
    [SerializeField] [Range(0, 10)] public int selfImpact;
    [SerializeField] [Range(0, 10)] public int enemyImpact;
    [SerializeField] [Range(0.1f, 1.0f)] public float usingInterval;

    public enum SpellType {NonPhysicProjectile, DirectedOnEnemy};
}
