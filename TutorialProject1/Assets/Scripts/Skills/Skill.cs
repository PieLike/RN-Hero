using UnityEngine;

[CreateAssetMenu(menuName = "Skill", fileName = "skill_")]
public class Skill : ScriptableObject
{
    [SerializeField] public string skillName;
    [SerializeField] public Sprite image;
    [SerializeField] public string info;
    [SerializeField] public SkillType type;
    [SerializeField] public Potion potion;
    [SerializeField] public AbilityType ability;

    public enum SkillType {potion, ability}
    public enum AbilityType {potionCount}
}
