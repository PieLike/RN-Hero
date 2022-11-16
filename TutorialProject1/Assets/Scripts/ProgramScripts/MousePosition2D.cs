using System;
using UnityEngine;

public class MousePosition2D : MonoBehaviour
{

    private Camera cam;
    public Vector3 mouseWorldPosition;
    public Vector2 mousePosition;
    private GameObject objFinalPoint;//, prefabFinalPoint;
    private RaycastHit2D raycastHit;
    public static GameObject supposedInteractionObject;

    private void Start() 
    {
        //находим объекты на сцене
        cam = Camera.main;
        //находим префабы в ресурсах        
        objFinalPoint = GameObject.Find("FinalPoint2D");
    }

    void Update()
    {
        if (MainVariables.inInterface == false)
        {                
            mousePosition = Input.mousePosition;

            mouseWorldPosition = cam.ScreenToWorldPoint(mousePosition);
            mouseWorldPosition.z = 0.0f;
            transform.position = mouseWorldPosition;

            //задаём точку движения "к чему"
            if (Input.GetMouseButton(1) && UIClick.OnMouseDown() && MainVariables.allowMovement == true)
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
        else if (supposedInteractionObject != null)
        {
            supposedInteractionObject = null; 
        }
        //Debug.Log(supposedInteractionObject.ToString());
    }

    private void OnTriggerStay2D(Collider2D other) {
        //заполняем слот наведенного объекта тем, что попал под курсор
        foreach(string tag in MainVariables.interactionTags)
        {
            if(other.gameObject.tag == tag)
            {
                if (other.name == "AdditionalCollider")
                    supposedInteractionObject = other.transform.parent.gameObject;
                else
                    supposedInteractionObject = other.gameObject;
                return;
            } 
        }  
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        //очищаем слот наведенного объекта
        if (supposedInteractionObject != null)
            supposedInteractionObject = null; 
    }    
}
