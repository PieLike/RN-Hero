using UnityEngine;

public class SkillData : MonoBehaviour
{
    public enum State {locked, unlocked, learned}
    [SerializeField] public Skill data;
    [SerializeField] public State state;
}
