using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameStartAndUpdate : MonoBehaviour
{
    private void Start() 
    {
        if (MainVariables.autoLoad)
        {
            SaveManager saveManager = FindObjectOfType<SaveManager>();
            saveManager.LoadSave();   
        }
    }
    private void Update() 
    {
        if(MainVariables.inMovement == false)
            MainVariables.forceNewMovementLockWhenMove = false;
            
        if(MainVariables.forceNewMovementLockWhenMove || MainVariables.inInterface || MainVariables.inTalking || MainVariables.inSpelling)
        {
            if (MainVariables.allowMovement != false)
            MainVariables.allowMovement = false;
        }
        else
        {
            if (MainVariables.allowMovement != true)
                MainVariables.allowMovement = true;
        }
    }
}

