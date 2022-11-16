using System;
using System.Collections.Generic;
using UnityEngine;

public class PickerWheelPotions : PickerWheelMy
{
    public bool potionIsUsed;
    StatusBar statusBar; PotionManager potionManager;
    public override void Start() 
    {
        base.Start();

        wheelPiecePrefab = Resources.Load<GameObject>("2d_prefabs/WheelPieceForPotion");
        statusBar = FindObjectOfType<StatusBar>(); 

        potionManager = FindObjectOfType<PotionManager>();
    }
    public override void Update() 
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && wheelPieces[indicatorIndex].potion.script != "")
		{
            UsePotion();
		}
    }

    public void FillWheelPieces(List<PotionManager.ActivePotion> potions)
	{
        if (wheelPieces == null)
            wheelPieces = new List<WheelPiece>();

        foreach (var item in potions)
		{
            if (item != null && item.potion != null)
			    wheelPieces.Add(new WheelPiece{ image = item.potion.image, potion = item.potion, count = item.count});
            else
                Debug.LogError("Potion from List<Potion> in Manager = null");
		}
        UpdateWheel();
    }

    void UsePotion()
    {
        potionIsUsed = false;
        Potion potion = wheelPieces[indicatorIndex].potion;

        Type type = Type.GetType(potion.script);// componentT
        if (type == null)
        {
            Debug.LogError("cant find type " + potion.script);
            return;
        }
            
        var component = gameObject.AddComponent(type);// as EnemyBehaviour;//MonoBehaviour;    //System.Type.GetType(item)   //EnemyBehaviour
        Destroy (component);  

        if (potionIsUsed) 
        {
            Debug.Log("Potion " + potion.name + " have used");
            if (statusBar != null && potion.status != null) statusBar.AddBuff(potion.status, potion.duration);
            potionManager.RemoveActivePotion(potion);
        }
        else
            Debug.Log("Potion " + potion.name + " hav not used");

    }    
}
