using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour 
{

    public GameObject target;
    private Vector3 offset; 
    public float smooth= 5.0f; 

    void Start () 
    {        
        offset = transform.position - target.transform.position;
    }

    /*void LateUpdate () 
    {        
        transform.position = target.transform.position + offset;
    }*/

    void  Update ()
    {
        transform.position = Vector3.Lerp (transform.position, target.transform.position + offset, Time.deltaTime * smooth);
    } 
}