using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Blink))]
public class HeroHealth : MonoBehaviour
{
    private Blink blink;
    private InterfaceManager interfaceManager; RespawnHero respawnHero;
    private static Image hpLine; private float smoothTime = 0.25f, Velocity = 0.0f; TMP_Text hpNumber;
    //private static float prevHp;
    public bool slowHealEnabled; private float slowHealSpeed = 0.3f; private float pointHealRemaning; 
    private void Start() 
    {
        HeroMainData.plainMaxHP = 10;
        HeroMainData.plainMaxMP = 10;
        
        interfaceManager = FindObjectOfType<InterfaceManager>();
            hpLine = interfaceManager.HpLine.GetComponent<Image>();
            hpNumber = interfaceManager.HpNumber.GetComponent<TMP_Text>();

        HeroMainData.currentHP = HeroMainData.plainMaxHP + HeroMainData.buffMaxHP;
        blink = gameObject.GetComponent<Blink>();

        respawnHero = GetComponent<RespawnHero>();

        //UpdateHp();
    }
    /*private static void UpdateHp()
    {
        if (HeroMainData.currentHP != prevHp)
        {
            prevHp = HeroMainData.currentHP;
        } 
    }*/
    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage " + damage);
        HeroMainData.currentHP -= damage;
        //UpdateHp();

        if (HeroMainData.currentHP > 0)
        {
            if (blink != null)
                blink.DoBlink(false);
            //enemyView.DoParticleSystemDamage(impactPosition);
        }
        else
        {                
            //Debug.Log("у героя закончились жизни");
            interfaceManager.messageScript.ShowOkMessage(respawnHero.Respawn, "Герой погиб в край...");
        }
    }
    public void HealHP (float points)
    {
        HeroMainData.currentHP += points;
        //UpdateHp();
    }

    void Update()
    {
        float hpUpdate = HeroMainData.currentHP / (HeroMainData.plainMaxHP + HeroMainData.buffMaxHP);
        if (hpLine.fillAmount != hpUpdate)
        {
            float smoothHpUpdate = Mathf.SmoothDamp(hpLine.fillAmount, hpUpdate, ref Velocity, smoothTime);   
            hpLine.fillAmount = smoothHpUpdate;

            //hpNumber.text = HeroMainData.currentHP.ToString() + "/" + (HeroMainData.plainMaxHP + HeroMainData.buffMaxHP).ToString();
        }
    }

    public void EnableSlowHeal(float points)
    {
        if (slowHealEnabled == false)
        {
            slowHealEnabled = true;
            StartCoroutine(SlowHealCourotine(slowHealSpeed, points));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SlowHealCourotine(slowHealSpeed, pointHealRemaning + points));
        }
    }

    private IEnumerator SlowHealCourotine(float healSpeed, float totalHealPoint)
    {
        pointHealRemaning = totalHealPoint; //присваиваем один раз вначале и потом отнимает по единице постепенно

        for (var i = 0; i < totalHealPoint; i++)
        {
            yield return new WaitForSeconds(healSpeed);
            if (HeroMainData.currentHP < (HeroMainData.plainMaxHP + HeroMainData.buffMaxHP))
                HeroMainData.currentHP ++;
            pointHealRemaning --;
        }  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spell")
        {
            SpellBehavior spellBehavior = other.gameObject.GetComponent<SpellBehavior>();
            if(spellBehavior != null && spellBehavior.frendly == false)
            {
                if(spellBehavior.spell.damage > 0)
                    TakeDamage(spellBehavior.spell.damage);

                //if(spellBehavior.spell.enemyImpact > 0)
                //    TakeImpact(spellBehavior.spell.enemyImpact, spellBehavior.startPosition);
            }
        }
    }
    
}
