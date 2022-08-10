using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition25D : MonoBehaviour
{

    [SerializeField] private Camera cam;
    public Vector3 mouseWorldPosition;
    public Vector3 mousePosition;

    void Start()
    {
        cam = Camera.main;
               
    }

       void Update()
    {
        mousePosition.x = Input.mousePosition.x;
        mousePosition.y = Input.mousePosition.y;
        mousePosition.z = 10f;
        mouseWorldPosition = cam.ScreenToWorldPoint(mousePosition);
        //mouseWorldPosition.z = 0f;
        transform.position = new Vector3 (mouseWorldPosition.x, transform.position.y, mouseWorldPosition.y);


    }
}
