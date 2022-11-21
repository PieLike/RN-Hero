using UnityEngine;
//using System;

public class SpellBehavior : MonoBehaviour
{
    public bool frendly;
    public ScriptableObjSpell spell;
    public Vector2 startPosition;
    ScriptableObjSpell.SpellType behaviorType;
    private double roundAbsStartRelativePointY;

    public void NonPhysicProjectile(ScriptableObjSpell newSpell, bool rotated, Vector2 position)     //BookOfSpells.spell
    {
        spell = newSpell;
        startPosition = position; 
        behaviorType = ScriptableObjSpell.SpellType.NonPhysicProjectile;
        roundAbsStartRelativePointY = MyMathCalculations.RoundAbs(transform.InverseTransformPoint(0, 0, 0).y);

        //выпрямляем вращение дочерних элементов в 0,0,0 по мировым координатам
        //чтобы доп спрайты не поворачивались при использовании заклинания
        UnRotateChildrens();
        //if (rotated)
        MoveShadow();
            
    }

    private void Update() 
    {
        switch (behaviorType)
        {
            case (ScriptableObjSpell.SpellType.NonPhysicProjectile) :
                //nonPhysicProjectile
                if (MyMathCalculations.RoundAbs(transform.InverseTransformPoint(0, 0, 0).y) != roundAbsStartRelativePointY + System.Math.Abs(spell.distance))
                    transform.Translate(Vector3.up * Time.deltaTime * spell.speed);
                else
                    OnDead();
                break;
        }
    }

    private void UnRotateChildrens()
    {
        Transform[] childTransform = GetComponentsInChildren<Transform>(false);
        foreach (var item in childTransform)
            if (item != null && item != transform && item.name != "Trace")
            {
                item.eulerAngles = new Vector3(0,0,0);
            }
    }

    private void MoveShadow()
    {
        Transform[] childTransform = GetComponentsInChildren<Transform>(false);
        foreach (var item in childTransform)
            if (item != null && item != transform && item.name == "Shadow")
            {
                //item.eulerAngles = new Vector3(0,0,90f);                
                item.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            }    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        //if (other.gameObject.tag != "Loot" && other.gameObject.tag != "Spell" && other.gameObject.tag != "EnemyAttackArea") 
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Hero" || other.gameObject.tag == "Enemy") 
        {
            if ((other.gameObject.tag != "Hero" && frendly) || (other.gameObject.tag != "Enemy" && frendly == false))
            {
                if(other.gameObject.GetComponent<SpriteRenderer>() != null)
                    OnDead();
            }
        }
    }

    private void OnDead()
    {
        FindObjectOfType<AudioManager>().Play(spell.name);
        Destroy(gameObject);
    }
}
