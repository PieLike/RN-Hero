using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    public static PotionManager instance;
    public List<Potion> potions;// = new List<Potion>();
    [SerializeField] public List<ActivePotion> activePotions;

    [Serializable]
    public class ActivePotion
    {
        public Potion potion;
        public int count;
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

    /*private void Start() 
    {
        activePotions = new List<ActivePotion>();
        foreach (var item in potions)
        {
            activePotions.Add(new ActivePotion { potion = item, count = 1 } );
        }
    }*/

    public void AddActivePotion(Potion _potion)
    {
        int activePotionNumber = CheckExistingActive(_potion);
        if (activePotionNumber != (-1) && activePotions[activePotionNumber] != null)
        {
            activePotions[activePotionNumber].count ++;
        }
        else
        {
            if (activePotions == null) activePotions = new List<ActivePotion>();
            activePotions.Add( new ActivePotion {potion = _potion, count = 1} );
        }
    }
    public void RemoveActivePotion(Potion _potion)
    {
        int activePotionNumber = CheckExistingActive(_potion);
        if (activePotionNumber != (-1) && activePotions[activePotionNumber] != null)
        {
            activePotions[activePotionNumber].count --;
        }
        if (activePotions[activePotionNumber].count < 1)
        {
            activePotions.Remove( activePotions[activePotionNumber] );
        }
    }


    public void AddPotion(Potion potion)
    {
        if (CheckExistingAll(potion) == (-1))
        {
            if (potions == null) potions = new List<Potion>();
            potions.Add(potion);
        }
    }

    public int CheckExistingAll(Potion _potion)
    {
        if (potions == null) return (-1);
        return potions.FindIndex( delegate(Potion potion){ return potion == _potion; } );
    }

    public int CheckExistingActive(Potion _potion)
    {
        if (activePotions == null) return (-1);
        return activePotions.FindIndex( delegate(ActivePotion activePotion){ return activePotion.potion == _potion; } );
    }

    public void ClearActivePotions()
    {
        activePotions.Clear();
    }
}
