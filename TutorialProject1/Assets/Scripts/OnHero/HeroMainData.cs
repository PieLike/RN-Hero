public class HeroMainData
{
    //public static float maxHP, maxMP, damage, speed, speedAttack;
    public static float plainMaxHP, plainMaxMP, plainDamage, plainSpeed, plainSpeedAttack;
    public static float buffMaxHP, buffMaxMP, buffDamage, buffSpeed, buffSpeedAttack;
    public static float currentHP, currentMP;
    public static int _level; public static int level {get{ return _level;} set{ _level = value; wordAbleCount = value *2; }}
    public static int wordAbleCount, wordActualCount; //wordAbleCount = level * 5
    public static int inventorySlotCount = 10;
    public static float experience = 0f;     
    //public static int enemyWordsCount = 5; 

    public static int maxPotionWithSelf = 1;
    public static int skillPoints;
}
