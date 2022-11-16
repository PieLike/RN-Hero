using UnityEngine;

public class PotionFireballSpell : PotionBehavior
{
    Spells spells;
    ScriptableObjSpell fireballSpell;
    HeroView heroView;

    public override void Awake() 
    {
        spells = FindObjectOfType<Spells>();
        fireballSpell = Resources.Load<ScriptableObjSpell>("Spells/fireballspell/fireballspell");
        heroView = FindObjectOfType<HeroView>();

        if (Use() == true)
            gameObject.GetComponent<PickerWheelPotions>().potionIsUsed = true;
    }
    public override bool Use()
    {
        spells.activeSpell = fireballSpell;  
        spells.SpellBegining(10f);
        //heroView.BeginReload(Time.time, Time.time + 10f);   //GetSpellDuration()

        return true;
    }
}
