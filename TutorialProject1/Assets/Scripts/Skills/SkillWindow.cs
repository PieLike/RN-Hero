using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindow : MonoBehaviour
{
    [NonSerialized] public InterfaceManager interfaceManager;  PotionManager potionManager;
    SkillScroll skillScroll;
    TMP_Text pointText;
    //private Animator animator;
    private void Start() 
    {
        interfaceManager = FindObjectOfType<InterfaceManager>(); 
        potionManager = FindObjectOfType<PotionManager>(); 
        skillScroll = GetComponent<SkillScroll>();
        
        interfaceManager.SkillExit.GetComponent<Button>().onClick.AddListener(CloseSkillWindow);
        interfaceManager.SkillButton.GetComponent<Button>().onClick.AddListener(SkillButton);

        Transform pointTextObj = interfaceManager.SkillsPanel.transform.Find("Text");
        pointText = pointTextObj.GetComponent<TMP_Text>();

        //animator = interfaceManager.SkillsPanel.GetComponent<Animator>();
    }

    public void OpenSkillWindow()
    {       
        interfaceManager.SkillsPanel.SetActive(true);
        MainVariables.inInterface = true;

        skillScroll.FillScroll();
        pointText.text = "У вас " + HeroMainData.skillPoints.ToString() + " очков прокачки";
    }
    private void CloseSkillWindow()
    {        
        skillScroll.ClearScroll();

        interfaceManager.SkillsPanel.SetActive(false);        
        MainVariables.inInterface = false;
    }
    public void SkillButton()
    {
        if(interfaceManager.SkillsPanel.activeSelf == false)
        {
            OpenSkillWindow();            
        }
        else
        {
            CloseSkillWindow();
        }
    }

    public bool LearnSkill(Transform skillSlot, Skill skill)
    {
        Transform lockObj = skillSlot.Find("Lock");
        Image lockImage = lockObj.GetComponent<Image>();
        lockImage.enabled = false;
        skillScroll.skillManager.AddSkill(skill, SkillData.State.learned);

        if (skill.type == Skill.SkillType.potion && skill.potion != null)
            potionManager.AddPotion(skill.potion);
        else if (skill.type == Skill.SkillType.ability)
        {
            switch (skill.ability)
            {
                case Skill.AbilityType.potionCount:
                    HeroMainData.maxPotionWithSelf ++;
                    break;
                default:
                    break;
            }
            
        }

        HeroMainData.skillPoints --;
        pointText.text = "У вас " + HeroMainData.skillPoints.ToString() + " очков прокачки";
        return true;
    }
}
