using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MousePosition3D : MonoBehaviour
{

    [SerializeField] private Camera cam;
    public Vector3 mouseWorldPosition;
    public Vector2 mousePosition;

       void Update()
    {
        mousePosition = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)){
            transform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }


    }
}
