using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{    
    public static SkillManager instance;
    [SerializeField] public List<SkillState> skills;

    [Serializable]
    public class SkillState
    {
        public Skill skill;
        public SkillData.State state;
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

    public void AddSkill(Skill _skill, SkillData.State _state)
    {
        int skillNumber = CheckExisting(_skill);
        if (skillNumber != (-1) && skills[skillNumber] != null)
        {
            skills[skillNumber].state = _state;
        }
        else
        {
            if (skills == null) skills = new List<SkillState>();
            skills.Add( new SkillState {skill = _skill, state = _state} );
        }
    }
    public void RemoveActivePotion(Skill _skill)
    {
        int skillNumber = CheckExisting(_skill);
        if (skillNumber != (-1) && skills[skillNumber] != null)
        {
            skills.Remove(skills[skillNumber]);
        }
    }

    public int CheckExisting(Skill _skill)
    {
        if (skills == null) return (-1);
        return skills.FindIndex( delegate(SkillState skillState){ return skillState.skill == _skill; } );
    }
}
