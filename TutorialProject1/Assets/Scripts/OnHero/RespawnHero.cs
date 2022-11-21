using UnityEngine;

public class RespawnHero : MonoBehaviour
{
    private Vector2 startPosition;
    private PotionManager potionManager; //HeroHealth heroHealth;
    private void Start() 
    {
        startPosition = transform.position;

        potionManager = FindObjectOfType<PotionManager>();
        //heroHealth = GetComponent<HeroHealth>();

    }
            
    public void Respawn()
    {
        MainVariables.isDead = false;
        transform.position = startPosition;
        potionManager.ClearActivePotions();
        HeroMainData.currentHP = HeroMainData.buffMaxHP + HeroMainData.plainMaxHP;
    }
}
