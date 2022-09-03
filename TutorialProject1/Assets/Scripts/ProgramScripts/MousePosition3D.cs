using System;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{

    private Camera cam;
    [NonSerialized] public Vector3 mouseWorldPosition;
    [NonSerialized] public Vector2 mousePosition;
    private LayerMask layerIgnoreRaycast;
    private GameObject finalPoint, prefabFinalPoint;

    private void Start() 
    {
        //находим объекты на сцене
        cam = Camera.main;//GameObject.Find("MainCamera").GetComponent<Camera>();
        //находим префабы в ресурсах
        prefabFinalPoint = Resources.Load<GameObject>("3d_prefabs/FinalPoint");

        layerIgnoreRaycast = LayerMask.GetMask("Default"); 
    }

    void Update()
    {
        mousePosition = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit,100, layerIgnoreRaycast)){
            transform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        //задаём точку движения "к чему"
        if (Input.GetMouseButton(0) && UIClick.OnMouseDown() && MainVariables.inSpelling == false && MainVariables.inInterface == false)
        {
            Vector3 raycast = new Vector3(raycastHit.point.x, 0f ,raycastHit.point.z);
            //либо создаем новую точку либо задем новую позицию настоящей
            if (finalPoint == null)
                finalPoint = Instantiate(prefabFinalPoint, raycast, Quaternion.Euler(0,0,0));
            else
            {                
                if(finalPoint.transform.position != raycast)
                    finalPoint.transform.position = raycast;
            }                

            if (finalPoint.activeSelf == false)
                finalPoint.SetActive(true);  
        }

    }
}
