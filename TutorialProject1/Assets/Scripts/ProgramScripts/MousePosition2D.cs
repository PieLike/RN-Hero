using System;
using UnityEngine;

public class MousePosition2D : MonoBehaviour
{

    private Camera cam;
    public Vector3 mouseWorldPosition;
    public Vector2 mousePosition;
    private GameObject objFinalPoint;//, prefabFinalPoint;
    private RaycastHit2D raycastHit;

    private void Start() 
    {
        //находим объекты на сцене
        cam = Camera.main;
        //находим префабы в ресурсах
        //prefabFinalPoint = Resources.Load<GameObject>("3d_prefabs/FinalPoint2D");
        objFinalPoint = GameObject.Find("FinalPoint2D");
    }

    void Update()
    {
        mousePosition = Input.mousePosition;

        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.z = 0.0f;
        transform.position = mouseWorldPosition;

        //задаём точку движения "к чему"
        if (Input.GetMouseButton(0) && UIClick.OnMouseDown() && MainVariables.allowMovement == true)
        {
            //либо создаем новую точку либо задем новую позицию настоящей
            if (objFinalPoint == null)
            {
                //objFinalPoint = Instantiate(prefabFinalPoint);
                //objFinalPoint.transform.position = mouseWorldPosition;
            }
            else
            {                
                if(objFinalPoint.transform.position != mouseWorldPosition)
                    objFinalPoint.transform.position = mouseWorldPosition;
            }                

            if (objFinalPoint.activeSelf == false)
                objFinalPoint.SetActive(true);  
        }

    }
}
