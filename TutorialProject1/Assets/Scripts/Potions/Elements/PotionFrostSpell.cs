using UnityEngine;

public class PotionFrostSpell : PotionBehavior
{
    Spells spells;
    ScriptableObjSpell frostSpell;
    HeroView heroView;

    public override void Awake() 
    {
        spells = FindObjectOfType<Spells>();
        frostSpell = Resources.Load<ScriptableObjSpell>("Spells/frostspell/frostspell");
        heroView = FindObjectOfType<HeroView>();

        if (Use() == true)
            gameObject.GetComponent<PickerWheelPotions>().potionIsUsed = true;
    }
    public override bool Use()
    {
        spells.activeSpell = frostSpell;  
        spells.SpellBegining(10f);
        //heroView.BeginReload(Time.time, Time.time + 10f);   //GetSpellDuration()

        return true;
    }
    /*private float GetSpellDuration()
    {
        switch (level)
        {
            case 1:
                return 10f;
            case 2:
                return 20f;
            case 3:
                return 30f;
            default:
                return 10f;
        }        
    }*/
}
