using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class SkillSlotBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Animator animator;
    [NonSerialized] public InfoWindow infoWindow;   float infoDelay = 0.3f;
    Transform itemInSlot; SkillData skillData;
    SkillWindow skillWindow;
        

    private void Start() 
    {
        Transform SkillSlots = transform.parent;
        Transform SkillsPanel = SkillSlots.transform.parent;
        Transform Skills = SkillsPanel.transform.parent;
        skillWindow = Skills.GetComponent<SkillWindow>();

        animator = gameObject.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (itemInSlot == null) itemInSlot = transform.Find("Skill(Clone)"); 
        if (skillData == null) skillData = itemInSlot.GetComponent<SkillData>();

        if (skillData != null && skillData.state != SkillData.State.locked)
        {
            animator.SetBool("bigSize", true);
        }

            //StartCoroutine(MouseOverCoroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("bigSize", false);

        //StopAllCoroutines();
        infoWindow.HideWindow();
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if (itemInSlot == null) itemInSlot = transform.Find("Potion(Clone)"); 
        if (skillData == null) skillData = itemInSlot.GetComponent<SkillData>();
        if (skillData != null)
        {
            if (skillData.state == SkillData.State.unlocked && HeroMainData.skillPoints > 0)
            {
                Skill skill = skillData.data;

                infoWindow.HideWindow();            
                
                if (skillWindow.LearnSkill(this.transform, skill))
                {
                    skillData.state = SkillData.State.learned;                     
                }   
            }            
        }
        else
            Debug.LogError("skillData = null");
    }

    private IEnumerator MouseOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(infoDelay); 
        infoWindow.ActivateWindow(skillData.data.skillName, skillData.data.info);      
    }
}
