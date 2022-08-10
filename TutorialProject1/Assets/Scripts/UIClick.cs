using UnityEngine;
using System.Data;
using System.IO;
using UnityEngine.EventSystems;

static class UIClick
{
    static UIClick()
    {

    }

    public static bool OnMouseDown()
    {        
        if (EventSystem.current.IsPointerOverGameObject() || CheckTachUI()){
            return false;
        }
        return true;
    }
    private static bool CheckTachUI()
    {
        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                // ui touched
                return true;
            }
        }
        return false;
    }
    
}