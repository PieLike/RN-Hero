using UnityEngine;
//using System.Collections;

public class FollowCamera : MonoBehaviour 
{

    private GameObject target;
    private Vector3 offset; 
    [SerializeField] private float smooth = 100f; 

    void Start () 
    {       
        //находим объекты на сцене
        target = GameObject.Find("Hero");

        offset = transform.position - target.transform.position;
    }

    /*void LateUpdate () 
    {        
        transform.position = target.transform.position + offset;
    }*/

    void  Update ()
    {
        transform.position = Vector3.Lerp (transform.position, target.transform.position + offset, Time.deltaTime * smooth);
        transform.LookAt(target.transform);
    } 
}